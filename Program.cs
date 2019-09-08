using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Brive.Middleware.PdfGenerator;

namespace candidatereport
{
    class MainClass
    {
        public static void Main(string[] args)
        {

            PdfFactory pdf;
            pdf = new PdfFactory(@"/Users/joel/prueba.pdf");
            pdf.GenerateReport(1);

            /*
            // Creamos el documento con el tamaño de página tradicional
            Document doc = new Document(PageSize.LETTER);
            // Indicamos donde vamos a guardar el documento
            PdfWriter writer = PdfWriter.GetInstance(doc,
                                        new FileStream(@"/Users/joel/prueba.pdf", FileMode.Create));

            // Le colocamos el título y el autor
            // Nota: Esto no será visible en el documento
            doc.AddTitle("Mi primer PDF");
            doc.AddCreator("Roberto Torres");

            // Abrimos el archivo
            doc.Open();
            doc.Add(new Paragraph("Mi primer documento PDF"));

            doc.Close();
            writer.Close();
            */
        }
    }
}
