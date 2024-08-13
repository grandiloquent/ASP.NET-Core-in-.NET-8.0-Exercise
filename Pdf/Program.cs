using iText.Kernel.Pdf;

var dir = @"C:\Users\Administrator\Downloads\.PDF";
foreach (var item in Directory.GetFiles(dir, "2.pdf"))
{
    PdfReader reader = new PdfReader(item);
    var pdfDocument = new PdfDocument(reader);
    PdfDictionary infoDictionary = pdfDocument.GetTrailer().GetAsDictionary(PdfName.Info);
    // if (infoDictionary != null)
    // {
        foreach (PdfName key in infoDictionary.KeySet())
        {
            //if (key.ToString() == "/Title")
                Console.WriteLine($"{key}: {infoDictionary.GetAsString(key)}");
        }
   // }

    //break;
}

