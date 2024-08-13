async function extractText(pdfDocument, pdfViewer) {
    console.log("Extracting", pdfDocument.numPages);
    const currentPage = pdfViewer.currentPageNumber;
    const buf = [];
    for (let i = 0; i < 2; i++) {
        if (currentPage + 1 < pdfDocument.numPages)
            buf.push(await extractPage(pdfDocument, currentPage + i));
    }
    writeText(buf.join("\n"));
}

async function extractPage(pdfDocument, pageNumber) {
    const page = await pdfDocument.getPage(pageNumber);
    const text = await page.getTextContent();
    return text.items.map(function (s) {
        if (s.hasEOL) {
            return "\n" + s.str;
        }
        return s.str;
    }).join(' ');
}