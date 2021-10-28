using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertHtmlToPdf.Helpers;
using ConvertHtmlToPdf.Models.Enums;
using Microsoft.AspNetCore.Http;
using SelectPdf;

namespace ConvertHtmlToPdf.Extensions
{
    public static class StringExtensionMethods
    {
        public static byte[] FromHtmlToPdfByteArray(this string value)
        {
            byte[]? result = null;
            using (MemoryStream ms = new())
            {
                HtmlToPdf converter = new();
                PdfDocument pdf = converter.ConvertHtmlString(value);

                pdf.Save(ms);
                result = ms.ToArray();
                pdf.Close();
            }
            return result;
        }

        public static IFormFile FromHtmlToPdf(this string value, string name = "", string fileName = "")
        {
            byte[] pdfByteArray = value.FromHtmlToPdfByteArray();
            var file = pdfByteArray.ToIFormFile(name, fileName);

            return file;
        }

        public static byte[] ToPdfByteArrayUsingTemplate(this string html, string baseMailTemplateLocationPath)
        {
            byte[]? result = null;
            using (MemoryStream ms = new())
            {
                var styles = FileHelper.GetFileAsString(baseMailTemplateLocationPath, BaseMailTemplateFileNameEnum.Style.GetDescription());
                var htmlHead = FileHelper.GetFileAsString(baseMailTemplateLocationPath, BaseMailTemplateFileNameEnum.Head.GetDescription());
                var htmlTemplate = FileHelper.GetFileAsString(baseMailTemplateLocationPath, BaseMailTemplateFileNameEnum.HtmlTemplate.GetDescription());

                htmlTemplate = htmlTemplate.Replace("[HeadContent]", htmlHead).Replace("[BodyContent]", $"{styles}{html}");

                HtmlToPdf converter = HtmlToPdfHelper.CreateHtmlToPdf(baseMailTemplateLocationPath);
                PdfDocument pdf = converter.ConvertHtmlString(htmlTemplate);

                pdf.Save(ms);
                result = ms.ToArray();
                pdf.Close();
            }
            return result;
        }


    }
}
