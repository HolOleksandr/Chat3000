let pdfInstance;

export async function loadPDF(container, document) {
    PSPDFKit.load({
        container: container,
        document: document
    }).then(function (instance) {
        pdfInstance = instance;
        instance.setToolbarItems((items) => [
            ...items,
            { type: "form-creator" }
        ]);
    });
}

export async function getPDFbytes() {
    const arrayBuffer = await pdfInstance.exportPDF();
    console.log(arrayBuffer);
    const byteArray = new Uint8Array(arrayBuffer);
    const blob = new Blob([byteArray], { type: 'application/pdf' });

    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.onloadend = () => {
            const base64String = reader.result.split(',')[1];
            resolve(base64String);
        };
        reader.onerror = reject;
        reader.readAsDataURL(blob);
    });
}