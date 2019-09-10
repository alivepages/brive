using Brive.Middleware.PdfGenerator.Yooin.Helper;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brive.Middleware.PdfGenerator.Yooin
{
    public class ImageReport
    {
        public static Image GetCandidateImage()
        {
            Image png = Image.GetInstance(ConfigurationManager.AppSettings["ReportPdfIcons"]);
            //png.ScalePercent(10f);
            png.ScaleAbsolute(2f, 2f);
            return png;
        }

        public static Image GetLogo(string logo)
        {
            string src = ConfigurationManager.AppSettings["ReportPdfIcons"] + logo + ".png";
            Image image = Image.GetInstance(new Uri(src));
            //image.ScaleAbsolute(50f, 50f);
            image.ScalePercent(80f);
            return image;
        }

        public static Image GetCandidateImage(string url)
        {
            Image image = Image.GetInstance(new Uri(url));
            //image.ScaleAbsolute(2f, 2f);
            return image;
        }

        public static Image GetDemographicIcon(string iconName)
        {
            String url = ConfigurationManager.AppSettings["ReportPdfIcons"] + iconName + ".png";
            Image image = Image.GetInstance(url);
            //image.ScaleAbsolute(15f, 15f);
            //image.Alignment = Element.ALIGN_CENTER;
            return image;
        }

        public static Image GetStrengthIcon(string iconName)
        {
            Image image = Image.GetInstance(ConfigurationManager.AppSettings["ReportPdfIcons"] + iconName + ".png");
            //image.ScaleAbsolute(15f, 15f);
            image.Alignment = Element.ALIGN_CENTER;
            return image;
        }

        public static Image GetCompetenceIcon(string iconName)
        {
            Image image = Image.GetInstance(ConfigurationManager.AppSettings["ReportPdfIcons"] + iconName + ".png");
            image.Alignment = Element.ALIGN_CENTER;
            return image;
        }

        public static Image GetCompetenceInterpretationIcon(string iconName)
        {
            Image image = Image.GetInstance(ConfigurationManager.AppSettings["ReportPdfIcons"] + iconName + ".png");
            image.ScaleAbsolute(30f, 30f);
            image.Alignment = Element.ALIGN_CENTER;
            return image;
        }

        public static Image GetInterpretationIcon(string iconName)
        {
            Image image = Image.GetInstance(ConfigurationManager.AppSettings["ReportPdfIcons"] + iconName + ".png");
            image.ScaleAbsolute(10f, 10f);
            image.Alignment = Image.ALIGN_MIDDLE | Image.ALIGN_LEFT;
            return image;
        }

        public static Image GetResultTableIcon(string iconName)
        {
            Image image = Image.GetInstance(ConfigurationManager.AppSettings["ReportPdfIcons"] + iconName + ".png");
            image.ScaleAbsolute(10f, 10f);
            image.Alignment = Image.ALIGN_MIDDLE | Image.ALIGN_LEFT;
            return image;
        }

        public static Image GetFooter(string iconName)
        {
            Image image = Image.GetInstance(new Uri(ConfigurationManager.AppSettings["ReportPdfIcons"] + iconName + ".png"));
            image.ScalePercent(50f);
            return image;
        }

        public static Image GetHeader(string iconName)
        {
            Image image = Image.GetInstance(new Uri(ConfigurationManager.AppSettings["ReportPdfIcons"] + iconName + ".png"));
            //image.ScalePercent(100f);
            return image;
        }
    }
}
