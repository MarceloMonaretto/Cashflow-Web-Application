$(document).ready(createPDF);

async function createPDF() { $('#GetPdf').click(loadPDF); }

async function loadPDF() {
    const { PDFDocument } = PDFLib; 
    var existingPdfBytes;

    existingPdfBytes = await fetch("http://localhost:5001/cashflowapi/Transaction/getPDF").then((res) => res.blob());

    fetch("http://localhost:5001/cashflowapi/Transaction/getPDF")
    .then(r => r.blob())
    .then(showFile)

    console.log(existingPdfBytes);
    //await saveAndRender(PDFDocument.load(existingPdfBytes));
}

async function saveAndRender(doc) {
    // Serialize the `PDFDocument` to bytes (a `Uint8Array`).
    const pdfBytes = await doc.save();

    const pdfDataUri = await doc.saveAsBase64({ dataUri: true });
    document.getElementById('pdf').src = pdfDataUri;
}

function showFile(blob){
    // It is necessary to create a new blob object with mime-type explicitly set
    // otherwise only Chrome works like it should
    var newBlob = new Blob([blob], { type: "application/pdf" })

    // Create a link pointing to the ObjectURL containing the blob.
    const data = window.URL.createObjectURL(newBlob);
    var link = document.createElement('a');
    link.href = data;
    link.download = "file.pdf";
    link.click();
    window.URL.revokeObjectURL(data);
}