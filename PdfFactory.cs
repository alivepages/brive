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
        private CandidateReportFactory candidateReportFactory;

        public PdfFactory(string pathReportPdf /*, Candidate candidate, VacantCandidateResult vacantCandidateResult*/)
        {
            this.pathReportPdf = pathReportPdf;
            //this.candidate = candidate;
            //this.vacantCandidateResult = vacantCandidateResult;

            candidateReportFactory = new CandidateReportFactory();
        }

        public byte[] GenerateReport(int reportType)
        {

            switch (reportType)
            {
                case (int)ReportType.YooinCandidate:
                    candidateReportFactory.SetCandidate(candidate);
                    candidateReportFactory.SetCandidateResult(vacantCandidateResult);
                    return candidateReportFactory.BuildPdf(pathReportPdf);               
            }
            return new byte[] { };
        }
    }

    public enum ReportType
    {
        YooinCandidate = 1,
    }
}
