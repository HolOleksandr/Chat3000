let peerInstance;


export function createPeer() {
    const peer = new Peer();
    peerInstance = peer;
    console.log("peer create: " + peer);
    return new Promise((resolve, reject) => {
        peer.on('open', function (id) {
            console.log("peer create ID: " + id);
            resolve(id);
        });
        peer.on('call', function (call) {
            console.log('Receiving call...');
            navigator.mediaDevices
                .getUserMedia({ video: true, audio: true })
                .then(function (stream) {
                    localStream = stream;
                    setLocalStream(localStream);

                    call.answer(localStream);
                    call.on('stream', function (stream) {
                        remoteStream = stream;
                        setRemoteStream(remoteStream);
                        console.log('SET stream on peer');
                    });
                })
                .catch(function (err) {
                    console.log('Failed to get local stream', err);
                });
        });
        //peer.on('destroy', function (call) {
        //    console.log('Destroing call...');
        //    navigator.mediaDevices.stop()
                
        //        .catch(function (err) {
        //            console.log('Failed to stop local stream', err);
        //        });
        //});
        peer.on('error', function (err) {
            console.log('Failed ', err);
            reject(err);
        });
    });


    
    
}

export async function callPeer(id) {
    await startLocalStream();
    await callPeerInner(id);
}

async function callPeerInner(id) {
    navigator.mediaDevices
        .getUserMedia({ video: true, audio: true })
        .then(function (stream) {
            localStream = stream;
            setLocalStream(localStream);
            console.log('Im calling ', peerInstance);
            const call = peerInstance.call(id, localStream);
            call.on('stream', function (stream) {
                remoteStream = stream;
                setRemoteStream(remoteStream);
            });
        })
        .catch(function (err) {
            console.log('Failed to get local stream', err);
        });
}

export async function destroyPeer(peerId) {
    if (peerInstance) {
        peerInstance.destroy();
        localStream.getTracks().forEach(function (track) {
            if (track.readyState === 'live') {
                track.stop();
            }
        });
        
    }
    
}

let localStream;
let remoteStream;

export async function startLocalStream() {
    console.log("Requesting local stream.");
    localStream = await navigator.mediaDevices.getUserMedia({ video: true, audio: true });
    return localStream;
}

export function setLocalStream(stream) {
    console.log('Setting local stream ');
    const localVideo = document.getElementById('localVideo');
    localVideo.srcObject = stream;
}

export function setRemoteStream(stream) {
    console.log('Setting remote stream ' );
    const remoteVideo = document.getElementById('remoteVideo');
    remoteVideo.srcObject = stream;
}

