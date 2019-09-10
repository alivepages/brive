using Brive.Middleware.PdfGenerator.Yooin.Helper;
using Brive.Yooin.Contracts;
using Brive.YooinEnterprise.DTO.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brive.Middleware.PdfGenerator.Yooin
{
    class ComparativeReportFactory
    {
        private VacantCandidateReportComparative vacantCandidateReportComparative;

        public void SetVacantCandidateReportComparative(VacantCandidateReportComparative vacantCandidateReportComparative)
        {
            this.vacantCandidateReportComparative = vacantCandidateReportComparative;
        }
        
        public byte[] BuildPdf(string pathReportPdf)
        {
            // Creamos el documento con el tamaño de página tradicional
            Document doc = new Document(PageSize.LETTER, -50f, -50f, 90f, 50f);

            // Indicamos donde vamos a guardar el documento

            string namePdf = Guid.NewGuid().ToString();

            //namePdf = pathReportPdf + "\\" + namePdf + ".pdf";
            try
            {
                namePdf = pathReportPdf;

                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(namePdf, FileMode.Create));

                //Llamamos a constrruir el header y footer del documento
                writer.PageEvent = new FeaturesReport("", vacantCandidateReportComparative.vacant.Name, DateTime.Now);

                // Abrimos el archivo
                doc.Open();

                doc.Add(this.title("Resumen Ejecutivo", 2f));
                doc.Add(this.BuildResumeTable());
                doc.Add(this.title("Comparativo por competencias", 2f));
                doc.Add(this.BuildComparativeTable());
                //doc.Add(Chunk.NEXTPAGE);
                doc.Add(this.AddImage("page3"));
                doc.Add(this.AddImage("page4"));

                doc.Close();
                writer.Close();
                return new byte[] { };
            }
       
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                doc.Close();
                return new byte[] { };
            }

        }

        private PdfPTable AddImage(string imageName)
        {
            PdfPTable resultTable = new PdfPTable(1);
            Image image = ImageReport.GetDemographicIcon("Sections/" + imageName);
            //image.ScaleToFit(37f, 37f); 
            PdfPCell cell = new PdfPCell(image);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            resultTable.AddCell(cell);
            return resultTable;
        }

        private PdfPTable title(string text, float spacing)
        {
            PdfPTable resultTable = new PdfPTable(1);
            resultTable.SpacingAfter = spacing;
            PdfPCell cell = new PdfPCell(new Phrase(text,
              new Font(Font.FontFamily.HELVETICA, 20f, Font.NORMAL, new BaseColor(10, 155, 255))));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BorderWidth = 0;
            resultTable.AddCell(cell);
            return resultTable;
        }

        private PdfPTable BuildResumeTable()
        {
            float[] widths = new float[] { 40f, 30f, 30f, 30f, 30f };
            PdfPTable table = new PdfPTable(widths);
            table.SpacingAfter = 20f;

            table.AddCell(TableHeader("Nombre"));
            table.AddCell(TableHeaderWithIcon("Experiencia", "DemographicIcons/drawable-hdpi/Experiencia", 12f, 12f, 7f));
            table.AddCell(TableHeaderWithIcon("Salario deseado", "DemographicIcons/drawable-hdpi/Salario", 10f, 15f, 5f));
            table.AddCell(TableHeaderWithIcon("Ubicación", "DemographicIcons/drawable-hdpi/Ubicacion", 10f, 15f, 5f));
            table.AddCell(TableHeaderWithIcon("Afinidad con el puesto", "DemographicIcons/drawable-hdpi/Level", 15f, 15f, 5f)).PaddingRight = 20f;
            //cell.PaddingRight = 20f;

            CandidateReportComparative ci;
            for (int i = 0; i<Math.Min(5, this.vacantCandidateReportComparative.candidate.Length); i++)
            {
                ci = this.vacantCandidateReportComparative.candidate[i];
                table.AddCell(TableContentName(ci.candidate.Name,i));
                table.AddCell(TableContent(ci.candidate.Experience)).PaddingLeft = 10f;
                table.AddCell(TableContent("$" + string.Format("{0:n}", ci.CandidateSalary.Maximum) + " - $" + string.Format("{0:n}", ci.CandidateSalary.Minimum))).PaddingLeft = 10f;
                table.AddCell(TableContent(ci.candidate.Address)).PaddingLeft = 10f;
                table.AddCell(TableContentLevel(ci.AffinityPercentage));
            }
            return table;
        }

        private PdfPCell HeaderBlue(string data, string iconName, float w, float h, float t)
        {
            float[] widths = new float[] { 10f, 30f };
            PdfPTable table = new PdfPTable(widths);
            Image icon = ImageReport.GetDemographicIcon(iconName);
            icon.ScaleToFit(w, h);
            PdfPCell cell1 = new PdfPCell(icon);

            cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell1.PaddingTop = t;
            cell1.PaddingRight = 2f;
            cell1.BorderWidth = 0;
            table.AddCell(cell1);

            PdfPCell cell = new PdfPCell();
            Paragraph p = new Paragraph();
            //p.Alignment = Element.ALIGN_CENTER;
           


            Font lato = new Font(Font.FontFamily.HELVETICA, 8f, Font.NORMAL, new BaseColor(255, 255, 255));

            lato.Color = new BaseColor(255, 255, 255);

            p.Add(new Chunk(data, lato));
            cell.AddElement(p);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);
            
            PdfPCell cellAll = new PdfPCell(table);

            
            cellAll.Padding = 10f;
            cellAll.BorderWidth = 0;
            cellAll.BackgroundColor = new BaseColor(10, 155, 255);
            
            cellAll.BorderColor = new BaseColor(10, 155, 255);
            return cellAll;
        }

        private PdfPTable BuildComparativeTable()
        {
            float[] widths = new float[] { 10f, 40f, 20f, 20f, 20f, 20f, 20f, 20f };
            PdfPTable table = new PdfPTable(widths);
            table.SpacingAfter = 20f;

            table.AddCell(TableHeaderBlue("")).BorderWidth = 0;
            table.AddCell(HeaderBlue("Competencias", "ResultTableIcons/drawable-hdpi/Competencias", 12f, 12f, 9f)).BorderWidth = 0;
            table.AddCell(HeaderBlue("Puesto", "ResultTableIcons/drawable-hdpi/Puesto", 10f, 15f, 11f)).BorderWidth = 0;
            table.AddCell(HeaderBlue("A", "ResultTableIcons/drawable-hdpi/Persona", 13f, 13f, 5f)).BorderWidth = 0;
            table.AddCell(HeaderBlue("B", "ResultTableIcons/drawable-hdpi/Persona", 13f, 13f, 5f)).BorderWidth = 0;
            table.AddCell(HeaderBlue("C", "ResultTableIcons/drawable-hdpi/Persona", 13f, 13f, 5f)).BorderWidth = 0;
            table.AddCell(HeaderBlue("D", "ResultTableIcons/drawable-hdpi/Persona", 13f, 13f, 5f)).BorderWidth = 0;
            table.AddCell(HeaderBlue("E", "ResultTableIcons/drawable-hdpi/Persona", 13f, 13f, 5f)).BorderWidth = 0;


            YooinEnterprise.DTO.VacantCompetence vi;


            List <YooinEnterprise.DTO.Models.CandidateCompetence> vacants;
            CandidateReportComparative ci;


            for (int i = 0; i < this.vacantCandidateReportComparative.vacantCompetences.Length; i++)
            {
                vi = this.vacantCandidateReportComparative.vacantCompetences[i];
                table.AddCell(TableContent(vi.IsRequired.ToString())); // TODO: poner icono
                table.AddCell(TableContent(vi.Name, Element.ALIGN_LEFT));
                table.AddCell(TableContent(vi.MinScore.ToString()));

                for (int k = 0; k < Math.Min(5, this.vacantCandidateReportComparative.candidate.Length); k++)
                {
                    ci = this.vacantCandidateReportComparative.candidate[k];
                    vacants = ci.candidateCompetences.ToList();
                    YooinEnterprise.DTO.Models.CandidateCompetence competence = vacants.First(s => s.Competence.Id == vi.CompetenceId);
                    table.AddCell(TableContent(competence.Score.ToString())); // TODO: Poner pleca
                }
            }
            
            return table;
        }

        private PdfPCell TableHeader(string data)
        {
            PdfPCell cell = new PdfPCell();
            Paragraph p = new Paragraph(13);
            Font lato = FontFactory.GetFont("Lato", 10f);
            lato.Color = new BaseColor(84, 84, 84);

            p.Add(new Chunk(data, lato));
            cell.AddElement(p);
            cell.BorderWidth = 0;
            return cell;
        }

        private PdfPCell TableHeaderBlue(string data)
        {
            PdfPCell cell = new PdfPCell();
            Paragraph p = new Paragraph(13);
            Font lato = FontFactory.GetFont("Lato", 10f);
            lato.Color = new BaseColor(84, 84, 84);

            p.Add(new Chunk(data, lato));
            cell.AddElement(p);
            cell.BorderWidth = 0;
            cell.BackgroundColor = new BaseColor(10, 155, 255);
            cell.BorderColor = new BaseColor(10, 155, 255);
            return cell;
        }


        private PdfPCell TableHeaderWithIcon(string data, string iconName, float w, float h, float t)
        {
            float[] widths = new float[] { 10f, 30f };
            PdfPTable table = new PdfPTable(widths);
            Image icon = ImageReport.GetDemographicIcon(iconName);
            icon.ScaleToFit(w, h);
            PdfPCell cell1 = new PdfPCell(icon);

            cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell1.PaddingTop = t;
            cell1.PaddingRight = 2f;
            cell1.BorderWidth = 0;
            table.AddCell(cell1);

            PdfPCell cell = new PdfPCell();
            Paragraph p = new Paragraph(13);
            Font lato = FontFactory.GetFont("Lato", 10f);
            lato.Color = new BaseColor(84, 84, 84);
           
            p.Add(new Chunk(data, lato));
            cell.AddElement(p);
            cell.BorderWidth = 0;
            if (iconName == "Level") cell.PaddingTop = -5f; 
            table.AddCell(cell);
            PdfPCell cellAll = new PdfPCell(table);
            cellAll.BorderWidth = 0;
            return cellAll;
        }

        private PdfPCell TableContent(string data, int align = Element.ALIGN_CENTER)
        {
            PdfPCell cell = new PdfPCell();
            Paragraph p = new Paragraph();
            Font arial = FontFactory.GetFont("Arial", 10f);
            arial.Color = new BaseColor(0, 0, 0);

            //p.PaddingTop = -5f;
            p.Add(new Chunk(data, arial));
            p.Alignment = align;
            
            cell.AddElement(p);

            //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.BorderWidth = 0;
            return cell;
        }

        private PdfPCell TableContentName(string data, int number)
        {
            float[] widths = new float[] { 10f, 30f };
            PdfPTable table = new PdfPTable(widths);

            // Imagen de perfil del candidato
            string urlImageCandidate = this.vacantCandidateReportComparative.candidate[number].candidate.AvatarUri;
            Image avatar;

            if (!string.IsNullOrWhiteSpace(urlImageCandidate)) { 
                try
                {
                    avatar = ImageReport.GetCandidateImage(urlImageCandidate);
                }
                catch
                {
                    avatar = ImageReport.GetDemographicIcon("Avatar/User");
                }
            } else {
                avatar = ImageReport.GetDemographicIcon("Avatar/User");
            }

            avatar.ScaleToFit(37f, 37f);
            PdfPCell cell1 = new PdfPCell(avatar);
            cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell1.BorderWidth = 0;
            cell1.PaddingTop = 4F;
            cell1.PaddingBottom = 4F;
            table.AddCell(cell1);

            PdfPCell cell = new PdfPCell();
            cell.PaddingLeft = 10f;
            cell.PaddingTop = 0.5f;
            Paragraph p = new Paragraph();
            Font arial = FontFactory.GetFont("Arial", 12f);
            arial.Color = new BaseColor(0, 0, 0);
            

            p.Add(new Chunk(data, arial));
            p.Alignment = Element.ALIGN_LEFT;
            cell.AddElement(p);

            Paragraph p2 = new Paragraph(13f);
            Font arialG = FontFactory.GetFont("Arial", 11f);
            arialG.Color = new BaseColor(10, 155, 255);

            int unicode = number + 65;
            char character = (char)unicode;

            p2.Add(new Chunk("Candidato " + character.ToString(), arialG));
            cell.AddElement(p2);
            cell.BorderWidth = 0;

            table.AddCell(cell);
            PdfPCell cellAll = new PdfPCell(table);
            cellAll.BorderWidth = 0;

            return cellAll;
        }

        private PdfPCell TableContentLevel(decimal? data)
        {
            PdfPCell cell = new PdfPCell();

            Paragraph p = new Paragraph();
            Font arial = FontFactory.GetFont("Arial", 16f);
            arial.Color = new BaseColor(10, 155, 255);
            p.Add(new Chunk(data.ToString() + "%", arial));
            p.Alignment = Element.ALIGN_RIGHT;
            
            cell.PaddingRight = 20f;
            //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.AddElement(p);

            Paragraph p2 = new Paragraph(10f);
            Font arialCh = FontFactory.GetFont("Arial", 8f, Font.BOLD);
            arialCh.Color = this.AffinityColor(data); 
            
            p2.Add(new Chunk(this.AffinityText(data), arialCh));
            p2.Alignment = Element.ALIGN_RIGHT;
            
            cell.AddElement(p2);
            cell.PaddingTop = 5f;
            cell.BorderWidth = 0;
            return cell;
        }

        
        private BaseColor AffinityColor(decimal? afinity)
        {
            switch ((int)afinity / 10)
            {
                case 10:
                    return new BaseColor(139, 82, 255);
                case 9:
                    return new BaseColor(0, 166, 255);
                case 8:
                    return new BaseColor(125, 217, 86);
                case 7:
                    return new BaseColor(235, 145, 10);
                default:
                    return new BaseColor(255, 86, 86);
            }
        }

        private string AffinityText(decimal? afinity)
        {
            switch ((int) afinity/10)
            {
                case 10:
                    return "Sobrecalificado";
                case 9:
                    return "Altamente competente";
                case 8:
                    return "Calificado";
                case 7:
                    return "Con reservas";
                default:
                    return "No compatible";
            }
        }
    }


}




