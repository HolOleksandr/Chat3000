const indexedDBName = 'WASMOfflinePWA';
const objectStoreName = 'OfflinePostRequests';

self.importScripts('./service-worker-assets.js');
self.addEventListener('install', event => event.waitUntil(onInstall(event)));
self.addEventListener('activate', event => event.waitUntil(onActivate(event)));
self.addEventListener('fetch', event => event.respondWith(onFetch(event)));

const cacheNamePrefix = 'offline-cache-';
const cacheName = `${cacheNamePrefix}${self.assetsManifest.version}`;
const offlineAssetsInclude = [ /\.dll$/, /\.pdb$/, /\.wasm/, /\.html/, /\.js$/, /\.json$/, /\.css$/, /\.woff$/, /\.png$/, /\.jpe?g$/, /\.gif$/, /\.ico$/, /\.blat$/, /\.dat$/ ];
const offlineAssetsExclude = [ /^service-worker\.js$/ ];

async function onInstall(event) {
    console.info('Service worker: Install');

    const assetsRequests = self.assetsManifest.assets
        .filter(asset => offlineAssetsInclude.some(pattern => pattern.test(asset.url)))
        .filter(asset => !offlineAssetsExclude.some(pattern => pattern.test(asset.url)))
        .map(asset => new Request(asset.url, { integrity: asset.hash, cache: 'no-cache' }));
    await caches.open(cacheName).then(cache => cache.addAll(assetsRequests));

    checkNetworkState();
}

async function onActivate(event) {
    console.info('Service worker: Activate');

    const cacheKeys = await caches.keys();
    await Promise.all(cacheKeys
        .filter(key => key.startsWith(cacheNamePrefix) && key !== cacheName)
        .map(key => caches.delete(key)));
}

async function onFetch(event) {
    if (event.request.method === 'GET') {
        const fetchResponse = fetch(event.request.clone());

        event.respondWith(
            caches.open(cacheName)
                .then(cache => {
                    return fetchResponse
                        .then(networkResponse => {
                            if (networkResponse.ok) {
                                cache.put(event.request, networkResponse.clone());
                            }
                            return networkResponse;
                        })
                        .catch(() => {
                            return cache.match(event.request);
                        });
                })
        );
    } else if (event.request.method === 'POST') {
        var reqUrl = event.request.url;
        var authHeader = event.request.headers.get('Authorization');

        return Promise.resolve(event.request.text()).then((payload) => {

            if (navigator.onLine) {
                return fetch(reqUrl, {
                    method: 'POST',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json',
                        'Authorization': authHeader
                    },
                    body: payload
                });
            } else {
                saveIntoIndexedDB(reqUrl, authHeader, payload);

                const myOptions = { status: 200, statusText: 'Work' };
                return new Response(payload, myOptions);
            }
        });
    }
}

function saveIntoIndexedDB(url, authHeader, payload) {

    const DBOpenRequest = indexedDB.open(indexedDBName);

    DBOpenRequest.onsuccess = (event) => {

        const postRequest = [
            {
                url: url,
                authHeader: authHeader,
                payload: payload
            }
        ];

        db = event.target.result;
        const transaction = db.transaction([objectStoreName], 'readwrite');
        const objectStore = transaction.objectStore(objectStoreName);
        const objectStoreRequest = objectStore.add(postRequest[0]);

        objectStoreRequest.onsuccess = (event) => {
            console.log("Request saved to IndexedDB");
        }
    }
}
function checkNetworkState() {
    setInterval(function () {
        if (navigator.onLine) {
            sendOfflinePostRequestsToServer()
        }
    }, 3000);
}

async function sendOfflinePostRequestsToServer() {
    const DBOpenRequest = indexedDB.open(indexedDBName);

    DBOpenRequest.onupgradeneeded = (event) => {

        db = event.target.result;

        if (!db.objectStoreNames.contains(objectStoreName)) {
            objectStore = db.createObjectStore(objectStoreName,
                { keyPath: "id", autoIncrement: true });
        }
    }

    DBOpenRequest.onsuccess = (event) => {

        db = event.target.result;

        const transaction = db.transaction([objectStoreName]);
        const objectStore = transaction.objectStore(objectStoreName);
        var allRecords = objectStore.getAll();
        let currentRecord = null;

        allRecords.onsuccess = function () {

            if (allRecords.result && allRecords.result.length > 0) {
                for (var i = 0; i < allRecords.result.length; i++) {

                    currentRecord = allRecords.result[i];

                    fetch(currentRecord.url, {
                        method: 'POST',
                        headers: {
                            'Accept': 'application/json',
                            'Content-Type': 'application/json',
                            'Authorization': currentRecord.authHeader
                        },
                        body: currentRecord.payload
                    }).then((response) => {
                        if (response.ok) {
                            const transaction = db.transaction([objectStoreName], 'readwrite');
                            const objectStore = transaction.objectStore(objectStoreName);
                            // remove details from IndexedDB
                            objectStore.delete(currentRecord.id);
                        } else {
                            console.log('An error occured while trying to send a POST request from IndexedDB.')
                        }
                    });
                }
            }
        }
    }
}