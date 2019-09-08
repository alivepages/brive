using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brive.Middleware.PdfGenerator.Yooin
{
    public class FeaturesReport : PdfPageEventHelper
    {
        private DateTime expirationDate;
        private string candidateName, vacantName;
        private PdfPTable header, footer;

        public FeaturesReport(string candidateName, string vacantName, DateTime expirationDate)
        {
            this.expirationDate = expirationDate;
            this.candidateName = candidateName;
            this.vacantName = vacantName;
        }

        // Escribimos el header del documento
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);

            float[] widths = new float[] { 400f, 200f };

            header = new PdfPTable(widths);
            header.SpacingAfter = 10F;
            header.TotalWidth = 600F;

            PdfPCell cell;
            cell = new PdfPCell(ImageReport.GetLogo("Logo/drawable-hdpi/Logo"));
            cell.BorderWidth = 0;
            cell.PaddingRight = 20f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;

            PdfPTable nested = new PdfPTable(1);

            Font latoTitle = FontFactory.GetFont("Lato", 18f, Font.NORMAL);
            Font latoSubTitle = FontFactory.GetFont("Lato", 12f);
            latoTitle.Color = BaseColor.WHITE;
            latoSubTitle.Color = BaseColor.WHITE;

            Chunk text = new Chunk("Reporte comparativo de competencias", latoTitle);
            Paragraph p = new Paragraph();
            p.Add(text);
            PdfPCell cell1 = new PdfPCell(p);
            cell1.PaddingLeft = 20f;
            cell1.BorderWidth = 0;
            nested.AddCell(cell1);

            latoSubTitle.SetStyle(Font.BOLD);
            //Chunk chunk = new Chunk("Evaluado: ", latoSubTitle);
            //latoSubTitle.SetStyle(Font.NORMAL);
            //Chunk chunk2 = new Chunk(candidateName, latoSubTitle);
            //latoSubTitle.SetStyle(Font.BOLD);
            Chunk chunk3 = new Chunk("Puesto:", latoSubTitle);
            latoSubTitle.SetStyle(Font.NORMAL);
            Chunk chunk4 = new Chunk(vacantName, latoSubTitle);
            latoSubTitle.SetStyle(Font.BOLD);
            Chunk chunk5 = new Chunk("  |  Fecha de emisión: ", latoSubTitle);
            latoSubTitle.SetStyle(Font.NORMAL);
            expirationDate = expirationDate.AddYears(1);
            Chunk chunk6 = new Chunk(expirationDate.ToString("dd/MM/yyyy"), latoSubTitle);

            p = new Paragraph();
            //p.Add(chunk);
            //p.Add(chunk2);
            p.Add(chunk3);
            p.Add(chunk4);
            p.Add(chunk5);
            p.Add(chunk6);
            cell1 = new PdfPCell(p);
            cell1.BorderWidth = 0;
            cell1.PaddingLeft = 20f;
            cell1.PaddingTop = 5f;
            nested.AddCell(cell1);
            nested.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfPCell nesthousing = new PdfPCell(nested);
            nesthousing.Padding = 0f;
            nesthousing.BorderWidth = 0;
            nesthousing.HorizontalAlignment = Element.ALIGN_CENTER;

            header.AddCell(nesthousing);
            header.AddCell(cell);
        }

        // Escribimos el header al inicio de cada página
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            Image headerImage = ImageReport.GetHeader("Header/drawable-hdpi/Header");
            //headerImage.SetAbsolutePosition(-280, 700);
            headerImage.SetAbsolutePosition(-230, 0);

            PdfContentByte cbhead = writer.DirectContent;
            PdfTemplate tp = cbhead.CreateTemplate(500, 500);
            tp.AddImage(headerImage);
            cbhead.AddTemplate(tp, -20, 700);

            //base.OnStartPage(writer, document);
            header.WriteSelectedRows(0, -1, 5, 780, writer.DirectContent);
        }

        // Escribimos el footer del documento
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            
            float[] widths = new float[] { 180f, 180f, 180f, 60f };
            footer = new PdfPTable(widths);
            PdfPCell cell1, cell2, cell3;
            footer.TotalWidth = 600F;

            Font lato = FontFactory.GetFont("Lato", 9f);
            lato.Color = BaseColor.GRAY;
            Chunk chunk = new Chunk("Una solución Brivé ", lato);
            Paragraph p = new Paragraph();
            p.Add(chunk);
            p.Alignment = Element.ALIGN_CENTER;
            cell1 = new PdfPCell(p);
            cell1.BorderWidth = 0;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;

            lato.Color = BaseColor.BLACK;
            chunk = new Chunk("www.evaluatest.com", lato);
            p = new Paragraph();
            p.Add(chunk);
            p.Alignment = Element.ALIGN_CENTER;
            cell2 = new PdfPCell(p);
            cell2.BorderWidth = 0;
            cell2.HorizontalAlignment = Element.ALIGN_CENTER;

            chunk = new Chunk("Fecha de emisión: " + DateTime.Now.ToString("dd/MM/yyyy"), lato);
            p = new Paragraph();
            p.Add(chunk);
            p.Alignment = Element.ALIGN_CENTER;
            cell3 = new PdfPCell(p);
            cell3.BorderWidth = 0;
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfPCell cell4 = new PdfPCell(new Phrase(writer.PageNumber.ToString(),
                        new Font(Font.FontFamily.HELVETICA, 9f, Font.BOLD, BaseColor.BLACK)));
            cell4.HorizontalAlignment = Element.ALIGN_CENTER;
            cell4.BorderWidth = 0;

            PdfPCell cell0 = new PdfPCell(ImageReport.GetFooter("Logo/drawable-hdpi/footer"));
            cell0.Colspan = 4;
            cell0.Padding = 10f;
            cell0.BorderWidth = 0;

            footer.AddCell(cell0);
            footer.AddCell(cell1);
            footer.AddCell(cell2);
            footer.AddCell(cell3);
            footer.AddCell(cell4);                      

            footer.WriteSelectedRows(0, -1, 5, 50, writer.DirectContent);
        }

        // Escribimos el footer al final de cada página
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            //base.OnCloseDocument(writer, document);
            header.WriteSelectedRows(0, -1, 5, 780, writer.DirectContent);
            footer.WriteSelectedRows(0, -1, 5, 50, writer.DirectContent);
        }
    }
}
