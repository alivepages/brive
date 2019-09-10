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
        private Candidate candidate;
        private VacantCandidateResult vacantCandidateResult;
        private VacantCandidateReportComparative vacantCandidateReportComparative;

        public void SetCandidate(Candidate candidate)
        {
            this.candidate = candidate;
        }

        public void SetCandidateResult(VacantCandidateResult vacantCandidateResult)
        {
            this.vacantCandidateResult = vacantCandidateResult;
        }

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
            namePdf = pathReportPdf;

            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(namePdf, FileMode.Create));
            
            //Llamamos a constrruir el header y footer del documento
            writer.PageEvent = new FeaturesReport(@"joel", @"Desarrollador Web", DateTime.Now);

            // Abrimos el archivo
            doc.Open();

            
            doc.Add(this.title("Resumen Ejecutivo", 2f));
            doc.Add(this.BuildResumeTable());
            doc.Add(this.title("Comparativo por competencias", 2f));
            doc.Add(this.BuildComparativeTable());

            doc.Close();
            writer.Close();
            return new byte[] { };

            /*
            try
            {
                PdfContentByte pcb = writer.DirectContent;

                // Creamos el tipo de Font que vamos utilizar
                iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);               

                // Agregamos los datos demograficos del candidato
                doc.Add(BuildCandidatePresentation());
                doc.Add(BuildDemographicInfo());

                //Agregamos la gráfica de competencias contra el puesto
                doc.Add(Chunk.NEWLINE);
                doc.Add(Chunk.NEWLINE);
                doc.Add(Chunk.NEWLINE);
                //doc.Add(BuildGraph());
                doc.Add(Chunk.NEXTPAGE);

                //Agregamos la tabla de competencias
                doc.Add(BuildResultTable());

                // Agregamos las interpretaciones de las competencias
                doc.Add(BuildInterpretation());

                // Agregamos las escalas de dominio de las comptetencias
                doc.Add(BuildHeaderScalesDomainLevels());
                doc.Add(BuildScalesDomainLevels());

                // Cerramos el documento
                doc.Close();
                writer.Close();

                // Pasamos el archivo a memoria y borramos el físico
                if (System.IO.File.Exists(namePdf))
                {
                    byte[] buffer = System.IO.File.ReadAllBytes(namePdf);
                    System.IO.File.Delete(namePdf);
                    return buffer;
                }
                return new byte[] { };
            }
            catch (Exception ex)
            {
                doc.Close();
                return new byte[] { };
            }#region MyRegion

#endregion
            */
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
                table.AddCell(TableContent(vi.Name));
                table.AddCell(TableContent(vi.MinScore.ToString()));

                for (int k = 0; k < Math.Min(5, this.vacantCandidateReportComparative.candidate.Length); k++)
                {
                    ci = this.vacantCandidateReportComparative.candidate[i];
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

        private PdfPCell TableContent(string data)
        {
            PdfPCell cell = new PdfPCell();
            Paragraph p = new Paragraph();
            Font arial = FontFactory.GetFont("Arial", 10f);
            arial.Color = new BaseColor(0, 0, 0);

            //p.PaddingTop = -5f;
            p.Add(new Chunk(data, arial));
            p.Alignment = Element.ALIGN_CENTER;
            
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





        /*
        #region Candidate Presentation

        private PdfPTable BuildCandidatePresentation()
        {
            // Creamos la tabla que guarda la información del candidato
            float[] widths = new float[] { 130f, 470f };
            PdfPTable presentationTable = new PdfPTable(widths);
            presentationTable.SpacingAfter = 30F;
            presentationTable.DefaultCell.Border = Rectangle.NO_BORDER;

            // Guardamos la imagen de perfil del candidato
            PdfPTable imageTable = new PdfPTable(1);
            imageTable.DefaultCell.Border = Rectangle.NO_BORDER;
            string urlImageCandidate = candidate.User.Files.Where(c => c.FileType.Id == 1).Select(c => c.Url).FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(urlImageCandidate))
                imageTable.AddCell(ImageReport.GetCandidateImage(urlImageCandidate));
            else
                imageTable.AddCell(ImageReport.GetCandidateImage());
            presentationTable.AddCell(imageTable);

            // Creamos una nueva tabla donde guardamos el nombre, edad y experiencia del candidato
            PdfPTable pdfPTable = new PdfPTable(2); //new float[] { 420f, 180f }
            pdfPTable.DefaultCell.Border = Rectangle.NO_BORDER;
            PdfPCell cell = new PdfPCell();
            Paragraph p = new Paragraph();

            Font lato = FontFactory.GetFont("Lato", 14f, Font.BOLD);
            Chunk text = new Chunk(candidate.Name, lato);
            p.Add(text);
            p.Add(Chunk.NEWLINE);
            p.Add(Chunk.NEWLINE);

            lato = FontFactory.GetFont("Lato", 10f);
            text = new Chunk(candidate.Experiences.Length > 0 ? candidate.Experiences[candidate.Experiences.Length - 1].JobTitle : "Sin experiencia", lato);
            p.Add(text);
            p.Add(Chunk.NEWLINE);
            p.Add(Chunk.NEWLINE);

            text = new Chunk(OperationsYears.CalculateYearsOld(candidate.BirthDate).ToString() + " años", lato);
            p.Add(text);            
            cell.AddElement(p);
            cell.BorderWidth = 0;
            cell.PaddingLeft = 10f;
            cell.PaddingBottom = 10f;            
            pdfPTable.AddCell(cell);

            // Agregamos la afinidad del candidato
            cell = new PdfPCell();
            p = new Paragraph();
            lato = FontFactory.GetFont("Lato", 28f, Font.BOLD);
            lato.Color = new BaseColor(12, 123, 234);
            text = new Chunk(vacantCandidateResult.TotalScoreCompetencesNeededAfinity.ToString() + "%", lato);
            p.Add(text);
            p.Add(Chunk.NEWLINE);
            p.Add(BuildHeaderAfinity());
            p.Alignment = Element.ALIGN_RIGHT;
            cell.AddElement(p);
            cell.BorderWidth = 0;
            cell.PaddingTop = 10f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfPTable.AddCell(cell);

            cell = new PdfPCell();
            cell.Colspan = 2;
            p = new Paragraph();
            p.Add(candidate.Bio);
            cell.AddElement(p);
            cell.BorderWidth = 0;
            cell.PaddingLeft = 10f;
            pdfPTable.AddCell(cell);

            presentationTable.AddCell(pdfPTable);           
            return presentationTable;
        }

        private PdfPCell BuildCellDemograficHeader(string data, string iconName)
        {
            PdfPCell cell = new PdfPCell();
            Paragraph p = new Paragraph();
            Font lato = FontFactory.GetFont("Lato", 9f);
            lato.Color = BaseColor.GRAY;

            Chunk text = new Chunk(data, lato);
            p.Add(text);
            
            if (iconName != "")
            {
                Image icon = ImageReport.GetDemographicIcon("DemographicIcons/drawable-hdpi/" + iconName);
                icon.ScaleAbsolute(15f, 15f);

                PdfPTable table = new PdfPTable(2);
                table.AddCell(new PdfPCell(icon));
                //document.Add(table);


                cell.AddElement(table);
                p.Alignment = Element.ALIGN_CENTER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
            }
            cell.AddElement(p);
            cell.BorderWidth = 0;
            return cell;
        }

        private PdfPCell BuildCellDemograficData(string data)
        {
            PdfPCell cell = new PdfPCell();
            Paragraph p = new Paragraph();
            Font lato = FontFactory.GetFont("Lato", 9f, Font.BOLD);

            Chunk text = new Chunk(data, lato);
            p.Add(text);
            p.Alignment = Element.ALIGN_CENTER;
            cell = new PdfPCell();
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.AddElement(p);
            cell.BorderWidth = 0;
            return cell;
        }

        private PdfPTable BuildDemographicInfo()
        {
            PdfPTable table = new PdfPTable(4);
            table.SpacingAfter = 20f;

            Font lato = FontFactory.GetFont("Lato", 9f);
            lato.Color = BaseColor.GRAY;
            
            table.AddCell(BuildCellDemograficHeader("Experiencia", "Experience"));
            table.AddCell(BuildCellDemograficHeader("Nivel", "Level"));            
            table.AddCell(BuildCellDemograficHeader("Salario deseado", "Salary"));            
            table.AddCell(BuildCellDemograficHeader("Ubicación", "Location"));

            string moneyMin = candidate.JobPreferences.Where(m => m.Type.Id == 1).Select(m => m.Value).FirstOrDefault();
            string moneyMax = candidate.JobPreferences.Where(m => m.Type.Id == 2).Select(m => m.Value).FirstOrDefault();
            string location = string.Empty;

            if (candidate.Locations.Count() > 0)
                location = candidate.Locations.Select(l => l.Name).FirstOrDefault();

            string currency = (!string.IsNullOrWhiteSpace(moneyMin) ? Convert.ToDecimal(moneyMin).ToString("C2") : "$ 0.00") + " - " +
                (!string.IsNullOrWhiteSpace(moneyMax) ? Convert.ToDecimal(moneyMax).ToString("C2") : "$ 0.00");
            table.AddCell(BuildCellDemograficData(OperationsYears.CalculateExperience(candidate.Experiences)));
            table.AddCell(BuildCellDemograficData(candidate.JobLevel.Name));            
            table.AddCell(BuildCellDemograficData(currency));
            table.AddCell(BuildCellDemograficData(location));
            return table;
        }

        private Paragraph BuildHeaderAfinity()
        {
            Paragraph p = new Paragraph();
            Font latoHeader = FontFactory.GetFont("Lato", 8f);
            latoHeader.Color = new BaseColor(12, 123, 234);

            Chunk text = new Chunk(vacantCandidateResult.VacantAnalyzed.Name);
            p.Add(text);
            p.Add(Chunk.NEWLINE);
            text = new Chunk("Afinidad con el puesto", latoHeader);
            p.Add(text);
            p.Alignment = Element.ALIGN_RIGHT;

            return p;
        }

        #endregion

        #region Domain Levels

        private PdfPTable BuildHeaderScalesDomainLevels()
        {
            PdfPTable table = new PdfPTable(1);
            table.SpacingAfter = 30f;

            PdfPCell cell = new PdfPCell(new Phrase("¿Cómo se interpreta la afinidad?",
               new Font(Font.FontFamily.HELVETICA, 16f, Font.BOLD, new BaseColor(12, 123, 234))));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BorderWidth = 0;
            table.AddCell(cell);           

            //Paragraph p = new Paragraph();
            string text = @"El objetivo de este reporte es mostrar los resultados obtenidos al comparar el 
                perfil de competencias del evaluado con el perfil requerido del puesto, para lograr un desempeño adecuado
                con las competencias clave necesarias.";
            text = text.Replace(Environment.NewLine, String.Empty).Replace("  ", String.Empty);
            //p.Add(text);
            //p.Alignment = Element.ALIGN_JUSTIFIED;

            Font lato = FontFactory.GetFont("Lato", 10f);
            lato.Color = BaseColor.GRAY;
            Chunk beginning = new Chunk(text, lato);
            Phrase p1 = new Phrase(beginning);
            cell = new PdfPCell(p1);
            cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            cell.BorderWidth = 0;
            table.AddCell(cell);
            return table;
        }

        private PdfPTable BuildScalesDomainLevels()
        {
            PdfPTable scalesTable = new PdfPTable(5);
            scalesTable.SpacingAfter = 30f;

            Font latoHeader = FontFactory.GetFont("Lato", 8f);
            Font latobody = FontFactory.GetFont("Lato", 8f);

            latoHeader.Color = new BaseColor(12, 123, 234);            

            PdfPCell cellHeader = new PdfPCell(new Phrase("Escala de nivel de dominios", 
                new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD, new BaseColor(12, 123, 234))));
            cellHeader.Colspan = 5;
            cellHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeader.BorderWidth = 2f;
            cellHeader.Padding = 5f;
            cellHeader.BorderColor = new BaseColor(12, 123, 234);
            scalesTable.AddCell(cellHeader);

            Paragraph p1 = new Paragraph();
            Chunk text = new Chunk("Nivel 0: Carece de la competencia 0.00 - 0.49", latoHeader);
            p1.Add(text);
            p1.Add(Chunk.NEWLINE);
            p1.Add(Chunk.NEWLINE);
            string t = @"En el momento de la evaluación, la persona no demostró la posesión de esta habilidad. 
                Si es necesaria para la obtención de los resultados esperados, se recomienda analizar la 
                factibilidad de que la Organización invierta en ayudar al candidato a desarrollarla.";
            t = t.Replace(Environment.NewLine, String.Empty).Replace("  ", String.Empty);
            text = new Chunk(t, latobody);
            p1.Add(text);
            PdfPCell cell = new PdfPCell(p1);
            cell.BorderWidth = 2f;
            cell.Padding = 5f;
            cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            cell.BorderColor = new BaseColor(12, 123, 234);
            scalesTable.AddCell(cell);
            //scalesTable.AddCell(p);

            Paragraph p2 = new Paragraph();
            text = new Chunk("Nivel 1: No demostrada 0.50 - 1.69", latoHeader);
            p2.Add(text);
            //p2.Add("Nivel 1: No demostrada 0.50 - 1.69");
            p2.Add(Chunk.NEWLINE);
            p2.Add(Chunk.NEWLINE);
            text = new Chunk("- Posee mínimas habilidades en esta competencia 0.50 - 0.89", latobody);
            p2.Add(text);
            p2.Add(Chunk.NEWLINE);
            p2.Add(Chunk.NEWLINE);
            text = new Chunk("- Está adquiriendo las habilidades que requiere la competencia 0.90 - 1.29", latobody);
            p2.Add(text);
            p2.Add(Chunk.NEWLINE);
            p2.Add(Chunk.NEWLINE);
            text = new Chunk("- Posee algunas habilidades, pero generalmente no las emplea 1.30 - 1.69", latobody);
            p2.Add(text);
            cell = new PdfPCell(p2);
            cell.Padding = 5f;
            cell.BorderWidth = 2f;
            cell.BorderColor = new BaseColor(12, 123, 234);
            cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            scalesTable.AddCell(cell);

            Paragraph p3 = new Paragraph();
            text = new Chunk("Nivel 2: En desarrollo 1.70 - 2.59", latoHeader);
            p3.Add(text);
            p3.Add(Chunk.NEWLINE);
            p3.Add(Chunk.NEWLINE);
            text = new Chunk("- Empezando a aplicar las habilidades en la competencia 1.70 - 1.99", latobody);
            p3.Add(text);
            p3.Add(Chunk.NEWLINE);
            p3.Add(Chunk.NEWLINE);
            text = new Chunk("- Inconsistente al presentar la competencia 2.00 - 2.29", latobody);
            p3.Add(text);
            p3.Add(Chunk.NEWLINE);
            p3.Add(Chunk.NEWLINE);
            text = new Chunk("- Está fortaleciendo su empleo de las habilidades 2.30 - 2.59", latobody);
            p3.Add(text);
            cell = new PdfPCell(p3);
            cell.Padding = 5f;
            cell.BorderWidth = 2f;
            cell.BorderColor = new BaseColor(12, 123, 234);
            cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            scalesTable.AddCell(cell);

            Paragraph p4 = new Paragraph();
            text = new Chunk("Nivel 3: Competente 2.60 - 3.49", latoHeader);
            p4.Add(text);
            p4.Add(Chunk.NEWLINE);
            p4.Add(Chunk.NEWLINE);
            text = new Chunk("- Demuestra las habilidades cuando el entorno es favorable 2.60 - 2.89", latobody);
            p4.Add(text);
            p4.Add(Chunk.NEWLINE);
            p4.Add(Chunk.NEWLINE);
            text = new Chunk("- Aplica la competencia en su trabajo cotidiano 2.90 - 3.19", latobody);
            p4.Add(text);
            p4.Add(Chunk.NEWLINE);
            p4.Add(Chunk.NEWLINE);
            text = new Chunk("- Está perfeccionando su aplicación de la competencia 3.20 - 3.49", latobody);
            p4.Add(text);
            cell = new PdfPCell(p4);
            cell.Padding = 5f;
            cell.BorderWidth = 2f;
            cell.BorderColor = new BaseColor(12, 123, 234);
            cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            scalesTable.AddCell(cell);

            Paragraph p5 = new Paragraph();
            text = new Chunk("Nivel 4: Experto 3.50 - 4.00", latoHeader);
            p5.Add(text);
            p5.Add(Chunk.NEWLINE);
            p5.Add(Chunk.NEWLINE);
            text = new Chunk("- Especialista en la aplicación de la competencia 3.50 - 3.69", latobody);
            p5.Add(text);
            p5.Add(Chunk.NEWLINE);
            p5.Add(Chunk.NEWLINE);
            text = new Chunk("- Aplica la competencia en escenarios diferentes y novedosos 3.70 - 3.84", latobody);
            p5.Add(text);
            p5.Add(Chunk.NEWLINE);
            p5.Add(Chunk.NEWLINE);
            text = new Chunk("- Transmite sus habilidades a otras personas 3.85 -4.00", latobody);
            p5.Add(text);
            cell = new PdfPCell(p5);
            cell.Padding = 5f;
            cell.BorderWidth = 2f;
            cell.BorderColor = new BaseColor(12, 123, 234);
            cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            scalesTable.AddCell(cell);
            return scalesTable;
        }

        #endregion

        #region Graphic

        private void BuildCompetenceHeader(string data, string iconData, PdfPTable competencesTable)
        {
            Font lato = FontFactory.GetFont("Lato", 12f);
            Chunk text = new Chunk(data, lato);
            Paragraph p = new Paragraph(text);

            Image icon = ImageReport.GetStrengthIcon(iconData);
            PdfPCell cell = new PdfPCell(icon);
            cell.Padding = 5f;
            cell.BorderWidth = 0;

            competencesTable.AddCell(cell);

            cell = new PdfPCell(p);
            cell.Padding = 5f;
            cell.BorderWidth = 0;
            competencesTable.AddCell(cell);
            competencesTable.AddCell(" ");
        }

        private void BuildCompetenceItem(CompetenceResultVacantCandidate competence, PdfPTable competencesTable)
        {
            Image icon;

            
             

            if (!string.IsNullOrWhiteSpace(competence.CompetenceIcon))
            {
                icon = ImageReport.GetCompetenceIcon(competence.CompetenceIcon);                
            }
            else
            {
                icon = ImageReport.GetCompetenceIcon("fa fa-trophy");                
            }
            
            PdfPCell cell = new PdfPCell(icon);
            cell.Padding = 5f;
            cell.BorderWidth = 0;
            competencesTable.AddCell(cell);

            Font latoComptetence = FontFactory.GetFont("Lato", 10f);
            Font latoDomain = FontFactory.GetFont("Lato", 8f);
            latoDomain.Color = new BaseColor(12, 123, 234);
            Chunk chunk1 = new Chunk(competence.CompetenceName, latoComptetence);
            Chunk chunk2 = new Chunk(competence.CandidateDomainLevel, latoDomain);

            Paragraph p = new Paragraph();
            p.Add(chunk1);
            p.Add(Chunk.NEWLINE);
            p.Add(chunk2);
            cell = new PdfPCell(p);
            cell.Padding = 5f;
            cell.BorderWidth = 0;
            competencesTable.AddCell(cell);

            cell = new PdfPCell(new Phrase(competence.CandidateScore.ToString()));
            cell.Padding = 5f;
            cell.BorderWidth = 0;
            competencesTable.AddCell(cell);
        }
       
        #endregion

        #region Summary Results

        private PdfPTable BuildResultTable()
        {
            PdfPTable resultTable = new PdfPTable(1);            
            resultTable.SpacingAfter = 30f;

            PdfPCell cell = new PdfPCell(new Phrase("Resumen de resultados",
              new Font(Font.FontFamily.HELVETICA, 16f, Font.BOLD, new BaseColor(12, 123, 234))));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BorderWidth = 0;
            cell.Padding = 10f;
            resultTable.AddCell(cell);

            PdfPTable resultBody = new PdfPTable(6);
            //resultBody.DefaultCell.Border = Rectangle.NO_BORDER;
            ResultComptenecesTable resultComptenecesTable = new ResultComptenecesTable();
            resultBody = resultComptenecesTable.GenerateHeaderTable(resultBody);

            foreach (var item in vacantCandidateResult.CompetencesAnalyzed.OrderByDescending(c => c.CandidateVacantGap))
            {
                resultBody.AddCell(resultComptenecesTable.GenerateBodyCompetence(item.CompetenceName));
                resultBody.AddCell(resultComptenecesTable.GenerateBody("Necesaria"));
                resultBody.AddCell(resultComptenecesTable.GenerateBody(item.CandidatesAverageScoreMinimum.ToString() + " - " + item.CandidatesAverageScoreMaximum.ToString()));
                resultBody.AddCell(resultComptenecesTable.GenerateBody(item.VacantScoreRequired.ToString()));
                resultBody.AddCell(resultComptenecesTable.GenerateBody(item.CandidateScore.ToString()));
                resultBody.AddCell(resultComptenecesTable.GenerateBody(item.CandidateVacantGap.ToString()));                
            }

            PdfPCell bodyCell = new PdfPCell(resultBody);
            bodyCell.Padding = 0f;
            bodyCell.BorderWidth = 0;

            resultTable.AddCell(bodyCell);
            return resultTable;
        }

        #endregion

        #region Interpretations

        private PdfPTable BuildInterpretation()
        {
            PdfPTable interpretationTable = new PdfPTable(new float[] { 30f, 150f, 150f, 70f, 70f, 80f });
            interpretationTable.SpacingAfter = 30f;
            interpretationTable.DefaultCell.Border = Rectangle.NO_BORDER;
            PdfPCell cell = new PdfPCell(new Phrase("Interpretación de competencias clave",
              new Font(Font.FontFamily.HELVETICA, 16f, Font.BOLD, new BaseColor(12, 123, 234))));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Colspan = 6;
            cell.BorderWidth = 0;
            interpretationTable.AddCell(cell);

            var mm = vacantCandidateResult.CompetencesAnalyzed.OrderBy(c => c.CandidateVacantGap);
            foreach (var item in vacantCandidateResult.CompetencesAnalyzed.OrderByDescending(c => c.CandidateVacantGap))
            {
                cell = new PdfPCell(new Phrase(" "));
                cell.Padding = 10f;
                cell.Colspan = 6;
                cell.BorderWidth = 0;
                interpretationTable.AddCell(cell);

                if (item.CompetenceIcon == null)
                {
                    item.CompetenceIcon = "fa fa-trophy";
                }

                interpretationTable.AddCell(ImageReport.GetCompetenceInterpretationIcon(item.CompetenceIcon));

                Paragraph p = new Paragraph();
                Font latoTitle = FontFactory.GetFont("Lato", 10f);
                Font latoSubTitle = FontFactory.GetFont("Lato", 10f);
                latoSubTitle.Color = new BaseColor(12, 123, 234);

                Chunk text = new Chunk(item.CompetenceName, latoTitle);
                p.Add(text);
                p.Add(Chunk.NEWLINE);

                text = new Chunk(item.CandidateDomainLevel, latoSubTitle);
                p.Add(text);
                interpretationTable.AddCell(p);
                
                p = new Paragraph();
                Font lato = FontFactory.GetFont("Lato", 8f);
                lato.Color = new BaseColor(32, 129, 1);
                text = new Chunk("  Rango de los evaluados en el mercado laboral  ", lato);
                p.Add(new Chunk(ImageReport.GetInterpretationIcon("range"), 0, 0));
                p.Add(text);
                lato = FontFactory.GetFont("Lato", 8f, Font.BOLD);
                text = new Chunk(String.Format("{0:0.##}",item.CandidatesAverageScoreMinimum) + " - " + item.CandidatesAverageScoreMaximum.ToString(), lato);
                p.Add(text);
                interpretationTable.AddCell(p);

                p = new Paragraph();
                lato = FontFactory.GetFont("Lato", 8f);
                lato.Color = new BaseColor(12, 123, 234);
                text = new Chunk("  Puesto  ", lato);
                p.Add(new Chunk(ImageReport.GetInterpretationIcon("job"), 0, 0));
                p.Add(text);
                lato = FontFactory.GetFont("Lato", 8f, Font.BOLD);
                lato.Color = BaseColor.BLACK;
                text = new Chunk(item.VacantScoreRequired.ToString(), lato);
                p.Add(text);
                interpretationTable.AddCell(p);

                p = new Paragraph();
                lato = FontFactory.GetFont("Lato", 8f);
                lato.Color = new BaseColor(147, 85, 239);
                text = new Chunk("  Persona  ", lato);
                p.Add(new Chunk(ImageReport.GetInterpretationIcon("avatar"), 0, 0));
                p.Add(text);
                lato = FontFactory.GetFont("Lato", 8f, Font.BOLD);
                lato.Color = BaseColor.BLACK;
                text = new Chunk(item.CandidateScore.ToString(), lato);
                p.Add(text);
                interpretationTable.AddCell(p);

                p = new Paragraph();
                lato = FontFactory.GetFont("Lato", 8f);
                lato.Color = new BaseColor(26, 133, 158);
                text = new Chunk("  Diferencia  ", lato);
                p.Add(new Chunk(ImageReport.GetInterpretationIcon("gap"), 0, 0));
                p.Add(text);
                lato = FontFactory.GetFont("Lato", 8f, Font.BOLD);
                lato.Color = BaseColor.BLACK;
                text = new Chunk(item.CandidateVacantGap.ToString(), lato);
                p.Add(text);
                interpretationTable.AddCell(p);

                

                if (vacantCandidateResult.VacantAnalyzed.EvaluationTypeId == 2)
                {
                    Paragraph p1 = new Paragraph();

                    iTextSharp.text.html.simpleparser.StyleSheet estiloHtml = new iTextSharp.text.html.simpleparser.StyleSheet();
                    string interp = "<font size=\"1\">" + item.InterpretationOfResults + "</font>";
                    var elementosHtml = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(interp), estiloHtml);
                    for (int k = 0; k < elementosHtml.Count; k++)
                    {
                        var ele = (IElement)elementosHtml[k];                        
                        p1.Add(ele);
                    }

                    PdfPCell interpretationDescription = new PdfPCell(p1);
                    interpretationDescription.Colspan = 6;
                    interpretationDescription.BorderWidth = 0;
                    interpretationTable.AddCell(interpretationDescription);
                }
                else if (vacantCandidateResult.VacantAnalyzed.EvaluationTypeId == 1)
                {
                    string[] interpretationsList = item.InterpretationOfResults.Split('|');
                    string suggestion = interpretationsList[interpretationsList.Length - 1];
                    string demostration = interpretationsList[0];


                    Paragraph p2 = new Paragraph();
                    lato = FontFactory.GetFont("Lato", 8f);
                    lato.Color = new BaseColor(147, 85, 239);
                    text = new Chunk(candidate.Name + " " + demostration, lato);
                    p2.Add(text);
                    p2.Add(Chunk.NEWLINE);
                    p2.Add(Chunk.NEWLINE);

                    string[] newInterpretationsList = new string[interpretationsList.Length - 1];
                    Array.Copy(interpretationsList, 1, newInterpretationsList, 0, interpretationsList.Length - 2);

                    lato = FontFactory.GetFont("Lato", 8f);
                    lato.Color = BaseColor.BLACK;
                    foreach (var interpretation in newInterpretationsList)
                    {
                        if (!string.IsNullOrWhiteSpace(interpretation))
                        {
                            text = new Chunk("- " + interpretation, lato);
                            p2.Add(text);
                            p2.Add(Chunk.NEWLINE);
                        }
                    }

                    p2.Add(Chunk.NEWLINE);
                    lato = FontFactory.GetFont("Lato", 8f);
                    lato.Color = new BaseColor(12, 123, 234);
                    text = new Chunk("Sugerencia de desarrollo para " + candidate.Name, lato);
                    p2.Add(text);
                    p2.Add(Chunk.NEWLINE);
                    p2.Add(Chunk.NEWLINE);
                    lato = FontFactory.GetFont("Lato", 8f);
                    lato.Color = BaseColor.BLACK;
                    text = new Chunk("- " + suggestion, lato);
                    p2.Add(text);
                    p2.Alignment = Element.ALIGN_JUSTIFIED;

                    PdfPCell interpretationDescription = new PdfPCell(p2);
                    interpretationDescription.Colspan = 6;
                    interpretationDescription.BorderWidth = 0;
                    interpretationTable.AddCell(interpretationDescription);
                }
            }
            return interpretationTable;
        }
        */
    }



    //#endregion

}




