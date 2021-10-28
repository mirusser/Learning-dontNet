using System.ComponentModel;

namespace ConvertHtmlToPdf.Models.Enums
{
    public enum BaseMailTemplateFileNameEnum
    {
        [Description("HtmlTemplate.html")]
        HtmlTemplate,

        [Description("Head.html")]
        Head,

        [Description("Header.html")]
        Header,

        [Description("Footer.html")]
        Footer,

        [Description("Style.html")]
        Style
    }
}