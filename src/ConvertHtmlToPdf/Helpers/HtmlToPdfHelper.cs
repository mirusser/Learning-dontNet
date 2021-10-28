using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertHtmlToPdf.Models.Enums;
using SelectPdf;

namespace ConvertHtmlToPdf.Helpers
{
    public static class HtmlToPdfHelper
    {
        public static HtmlToPdf CreateHtmlToPdf(string baseMailTemplateLocationPath)
        {
            HtmlToPdf converter = new();

            converter.Options.PdfPageSize = PdfPageSize.A4;

            var pdfHeaderHtml = CreatePdfHtmlSection(baseMailTemplateLocationPath, BaseMailTemplateFileNameEnum.Header);
            converter.Options.DisplayHeader = true;
            converter.Header.Height = 65;
            converter.Header.Add(pdfHeaderHtml);

            var pdfFooterHtml = CreatePdfHtmlSection(baseMailTemplateLocationPath, BaseMailTemplateFileNameEnum.Footer);
            converter.Options.DisplayFooter = true;
            converter.Footer.Height = 65;
            converter.Footer.Add(pdfFooterHtml);

            return converter;
        }

        public static PdfHtmlSection CreatePdfHtmlSection(string mailTemplateLocationPath, BaseMailTemplateFileNameEnum baseMailTemplateFileNameEnum)
        {
            var html = FileHelper.GetFileAsString(mailTemplateLocationPath, baseMailTemplateFileNameEnum.GetDescription());

            if (!string.IsNullOrEmpty(html))
            {
                PdfHtmlSection pdfHeaderHtml = new(html, "");
                pdfHeaderHtml.AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment;

                return pdfHeaderHtml;
            }

            return new("", "");
        }
    }
}
