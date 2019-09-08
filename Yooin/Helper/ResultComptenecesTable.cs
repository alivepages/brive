using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brive.Middleware.PdfGenerator.Yooin.Helper
{
    public class ResultComptenecesTable
    {
        public PdfPTable GenerateHeaderTable(PdfPTable resultBody)
        {
            foreach(var item in Enum.GetNames(typeof(HeaderText)))
            {
                string value = item.Replace("_", " ");
                resultBody.AddCell(CreateCellHeader(value, item));
            }

            return resultBody;
        }

        public PdfPCell GenerateBodyCompetence(string textBody)
        {
            PdfPCell cell = new PdfPCell();
            Font lato = FontFactory.GetFont("Lato", 8f);
            lato.Color = BaseColor.GRAY;
            Chunk text = new Chunk(textBody, lato);
            Paragraph p = new Paragraph();
            p.Add(text);

            cell.AddElement(p);
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0;
            cell.BorderColorLeft = BaseColor.WHITE;
            cell.BorderColorRight = BaseColor.WHITE;
            return cell;
        }

        public PdfPCell GenerateBody(string textBody)
        {
            PdfPCell cell = new PdfPCell();
            Font lato = FontFactory.GetFont("Lato", 8f, Font.BOLD);
            Chunk text = new Chunk(textBody, lato);           
            Paragraph p = new Paragraph();
            p.Add(text);            
            cell.AddElement(p);
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0;
            cell.BorderColorLeft = BaseColor.WHITE;
            cell.BorderColorRight = BaseColor.WHITE;
            return cell;
        }

        private PdfPCell CreateCellHeader(string headerText, string iconText)
        {
            PdfPCell cell = new PdfPCell();
            Font lato = FontFactory.GetFont("Lato", 8f);
            lato.Color = BaseColor.WHITE;
            Chunk text = new Chunk("  " + headerText, lato);
            Paragraph p = new Paragraph();
            p.Add(new Chunk(ImageReport.GetResultTableIcon(iconText), 0, 0));
            p.Add(text);

            cell.AddElement(p);
            cell.BorderColor = new BaseColor(12, 123, 234);
            cell.BackgroundColor = new BaseColor(12, 123, 234);
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;

            return cell;
        }

        private enum HeaderText
        {
            Competencias,
            Tipo_de_competencia,
            Rango_de_los_evaluadores_en_el_mercado_laboral,
            Puesto,
            Persona,
            GAP_diferencia
        }
    }    
}
