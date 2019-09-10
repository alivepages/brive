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
                            AvatarUri = "https://s3-us-west-2.amazonaws.com/brive-yooin-users/files/8391",
                            Experience = "0 años 10 meses"
                        },
                        CandidateSalary = new CandidateSalary{
                             Minimum = 23000,
                             Maximum = 25000
                        },
                        AffinityPercentage = (decimal?)100.14
                    },
                    new CandidateReportComparative
                    {
                        candidate = new CandidateLight
                        {
                            Name = "Obi-Wan Kenobi",
                            Address = "Tepito, Mexico City, CDMX, Mexico",
                            AvatarUri = "https://s3-us-west-2.amazonaws.com/brive-yooin-users/files/8408",
                            Experience = "0 años 10 meses"
                        },
                        CandidateSalary = new CandidateSalary{
                             Minimum = 23000,
                             Maximum = 25000
                        },
                        AffinityPercentage = (decimal?)96.58
                    },
                    new CandidateReportComparative
                    {
                        candidate = new CandidateLight
                        {
                            Name = "Obi-Wan Kenobi",
                            Address = "Tepito, Mexico City, CDMX, Mexico",
                            AvatarUri = "",
                            Experience = "0 años 10 meses"
                        },
                        CandidateSalary = new CandidateSalary{
                             Minimum = 23000,
                             Maximum = 25000
                        },
                        AffinityPercentage = (decimal?)80.36
                    },
                    new CandidateReportComparative
                    {
                        candidate = new CandidateLight
                        {
                            Name = "Palpatine",
                            Address = "Chalco, Mexico City, CDMX, Mexico",
                            Experience = "5 años 1 mes"
                        },
                        CandidateSalary = new CandidateSalary{
                             Minimum = 22000,
                             Maximum = 20000
                        },
                        AffinityPercentage = (decimal?)78.54
                    },
                                                                                                    new CandidateReportComparative
                    {
                        candidate = new CandidateLight
                        {
                            Name = "Darth Maul",
                            Address = "Tepito, Mexico City, CDMX, Mexico",
                            AvatarUri = "",
                            Experience = "0 años 10 meses"
                        },
                        CandidateSalary = new CandidateSalary{
                             Minimum = 23000,
                             Maximum = 25000
                        },
                        AffinityPercentage = (decimal?)60.36
                    }
                }
            };


            PdfFactory pdf;
            pdf = new PdfFactory(@"/Users/joel/prueba.pdf", data);
            pdf.GenerateReport(1);

        }
    }
}
