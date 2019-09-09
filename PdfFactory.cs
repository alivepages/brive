using Brive.Middleware.PdfGenerator.Yooin;
using Brive.Yooin.Contracts;
using Brive.YooinEnterprise.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brive.Middleware.PdfGenerator
{
    public class PdfFactory
    {
        private string pathReportPdf;
        private Candidate candidate;        
        private VacantCandidateResult vacantCandidateResult;
        private ComparativeReportFactory comparativeReportFactory;
        private VacantCandidateReportComparative vacantCandidateReportComparative;

        public PdfFactory(string pathReportPdf, VacantCandidateReportComparative vacantCandidateReportComparative)
        {
            this.pathReportPdf = pathReportPdf;
            //this.candidate = candidate;
            //this.vacantCandidateResult = vacantCandidateResult;
            this.vacantCandidateReportComparative = vacantCandidateReportComparative;

            comparativeReportFactory = new ComparativeReportFactory();
        }

        public byte[] GenerateReport(int reportType)
        {

            switch (reportType)
            {
                case (int)ReportType.YooinCandidate:
                    comparativeReportFactory.SetCandidate(candidate);
                    comparativeReportFactory.SetCandidateResult(vacantCandidateResult);
                    comparativeReportFactory.SetVacantCandidateReportComparative(vacantCandidateReportComparative);
                    return comparativeReportFactory.BuildPdf(pathReportPdf);               
            }
            return new byte[] { };
        }
    }

    public enum ReportType
    {
        YooinCandidate = 1,
    }
}
