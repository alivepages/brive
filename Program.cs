using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Brive.Middleware.PdfGenerator;
using Brive.YooinEnterprise.DTO.Models;
using Brive.Yooin.Contracts;
using Brive.YooinEnterprise.DTO;

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
                            Name = "Anakin Skywalker",
                            Address = "Tepito, Mexico City, CDMX, Mexico",
                            Experience = "0 años 10 meses"
                        },
                        CandidateSalary = new CandidateSalary{
                             Minimum = 23000,
                             Maximum = 25000
                        },
                        AffinityPercentage = 90
                    },
                    new CandidateReportComparative
                    {
                        candidate = new CandidateLight
                        {
                            Name = "Obi-Wan Kenobi",
                            Address = "Tepito, Mexico City, CDMX, Mexico",
                            Experience = "0 años 10 meses"
                        },
                        CandidateSalary = new CandidateSalary{
                             Minimum = 23000,
                             Maximum = 25000
                        },
                        AffinityPercentage = 100
                    },
                    new CandidateReportComparative
                    {
                        candidate = new CandidateLight
                        {
                            Name = "Obi-Wan Kenobi",
                            Address = "Tepito, Mexico City, CDMX, Mexico",
                            Experience = "0 años 10 meses"
                        },
                        CandidateSalary = new CandidateSalary{
                             Minimum = 23000,
                             Maximum = 25000
                        },
                        AffinityPercentage = 100
                    },
                    new CandidateReportComparative
                    {
                        candidate = new CandidateLight
                        {
                            Name = "Palpatine",
                            Address = "Chalco, Mexico City, CDMX, Mexico",
                            Experience = "5 años 1 mes"
                        },
                        CandidateSalary = new CandidateSalary{
                             Minimum = 22000,
                             Maximum = 20000
                        },
                        AffinityPercentage = 100
                    },
                                                                                                    new CandidateReportComparative
                    {
                        candidate = new CandidateLight
                        {
                            Name = "Darth Maul",
                            Address = "Tepito, Mexico City, CDMX, Mexico",
                            Experience = "0 años 10 meses"
                        },
                        CandidateSalary = new CandidateSalary{
                             Minimum = 23000,
                             Maximum = 25000
                        },
                        AffinityPercentage = 100
                    }
                }
            };


            PdfFactory pdf;
            pdf = new PdfFactory(@"/Users/joel/prueba.pdf", data);
            pdf.GenerateReport(1);

        }
    }
}
