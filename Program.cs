using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Brive.Middleware.PdfGenerator;
using Brive.YooinEnterprise.DTO.Models;
using Brive.Yooin.Contracts;

namespace candidatereport
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            // Mock
            VacantCandidateReportComparative data;
            data = new VacantCandidateReportComparative
            {
                candidate = new CandidateReportComparative[]
                {
                    new CandidateReportComparative
                    {
                        candidate = new CandidateLight
                        {
                            Name = "Anakin Skywalker"
                        } 
                    },
                    new CandidateReportComparative
                    {
                        candidate = new CandidateLight
                        {
                            Name = "Obi-Wan Kenobi"
                        }
                    },
                    new CandidateReportComparative
                    {
                        candidate = new CandidateLight
                        {
                            Name = "Gui-Gon Jinn"
                        }
                    },
                    new CandidateReportComparative
                    {
                        candidate = new CandidateLight
                        {
                            Name = "Palpatine"
                        }
                    },
                                                                                                    new CandidateReportComparative
                    {
                        candidate = new CandidateLight
                        {
                            Name = "Darth Maul"
                        }
                    }
                }
            };


            PdfFactory pdf;
            pdf = new PdfFactory(@"/Users/joel/prueba.pdf", data);
            pdf.GenerateReport(1);

        }
    }
}
