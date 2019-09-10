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
                vacantCompetences = new VacantCompetence[]
                {
                    new VacantCompetence
                    {
                        Name = "Guapo",
                        IsRequired = false,
                        MinScore = 3,
                        MaxScore = 4,
                        CompetenceId = 5
                    },
                    new VacantCompetence
                    {
                        Name = "Creatividad e Innovación",
                        IsRequired = true,
                        MinScore = 2,
                        MaxScore = 4,
                        CompetenceId = 3
                    }
                },
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
                        AffinityPercentage = (decimal?)100.14,
                        candidateCompetences = new Brive.YooinEnterprise.DTO.Models.CandidateCompetence[]
                        {
                            new Brive.YooinEnterprise.DTO.Models.CandidateCompetence
                            {
                                Competence = new Competence
                                {
                                    Id = 1
                                },
                                Score = 1
                            },
                            new Brive.YooinEnterprise.DTO.Models.CandidateCompetence
                            {
                                Competence = new Competence
                                {
                                    Id = 3
                                },
                                Score = 1
                            },
                            new Brive.YooinEnterprise.DTO.Models.CandidateCompetence
                            {
                                Competence = new Competence
                                {
                                    Id = 5
                                },
                                Score = 1
                            }
                        }
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
                        AffinityPercentage = (decimal?)96.58,
                        candidateCompetences = new Brive.YooinEnterprise.DTO.Models.CandidateCompetence[]
                        {
                            new Brive.YooinEnterprise.DTO.Models.CandidateCompetence
                            {
                                Competence = new Competence
                                {
                                    Id = 1
                                },
                                Score = 1
                            },
                            new Brive.YooinEnterprise.DTO.Models.CandidateCompetence
                            {
                                Competence = new Competence
                                {
                                    Id = 3
                                },
                                Score = 1
                            },
                            new Brive.YooinEnterprise.DTO.Models.CandidateCompetence
                            {
                                Competence = new Competence
                                {
                                    Id = 5
                                },
                                Score = 2
                            }
                        }
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
                        AffinityPercentage = (decimal?)80.36,
                        candidateCompetences = new Brive.YooinEnterprise.DTO.Models.CandidateCompetence[]
                        {
                            new Brive.YooinEnterprise.DTO.Models.CandidateCompetence
                            {
                                Competence = new Competence
                                {
                                    Id = 1
                                },
                                Score = 1
                            },
                            new Brive.YooinEnterprise.DTO.Models.CandidateCompetence
                            {
                                Competence = new Competence
                                {
                                    Id = 3
                                },
                                Score = 1
                            },
                            new Brive.YooinEnterprise.DTO.Models.CandidateCompetence
                            {
                                Competence = new Competence
                                {
                                    Id = 5
                                },
                                Score = 3
                            }
                        }
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
                        AffinityPercentage = (decimal?)78.54,
                        candidateCompetences = new Brive.YooinEnterprise.DTO.Models.CandidateCompetence[]
                        {
                            new Brive.YooinEnterprise.DTO.Models.CandidateCompetence
                            {
                                Competence = new Competence
                                {
                                    Id = 1
                                },
                                Score = 1
                            },
                            new Brive.YooinEnterprise.DTO.Models.CandidateCompetence
                            {
                                Competence = new Competence
                                {
                                    Id = 3
                                },
                                Score = 1
                            },
                            new Brive.YooinEnterprise.DTO.Models.CandidateCompetence
                            {
                                Competence = new Competence
                                {
                                    Id = 5
                                },
                                Score = 4
                            }
                        }
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
                        AffinityPercentage = (decimal?)60.36,
                        candidateCompetences = new Brive.YooinEnterprise.DTO.Models.CandidateCompetence[]
                        {
                            new Brive.YooinEnterprise.DTO.Models.CandidateCompetence
                            {
                                Competence = new Competence
                                {
                                    Id = 1
                                },
                                Score = 1
                            },
                            new Brive.YooinEnterprise.DTO.Models.CandidateCompetence
                            {
                                Competence = new Competence
                                {
                                    Id = 3
                                },
                                Score = 1
                            },
                            new Brive.YooinEnterprise.DTO.Models.CandidateCompetence
                            {
                                Competence = new Competence
                                {
                                    Id = 5
                                },
                                Score = 5
                            }
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
