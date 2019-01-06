using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Threading.Tasks;

namespace SK.RazorWire
{
    public enum FormDataStyle
    {
        None,
        Underline,
        Singleline,
        Multiline
    }

    [HtmlTargetElement("formdata")]
    public class FormDataTagHelper : BaseTagHelper
    {
        public string Label { get; set; }

        public FormDataStyle ContentStyle { get; set; }

        public override void Init(TagHelperContext context)
        {
            Class = "form-group";
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            base.InitProcess(output);
            var childContent = await output.GetChildContentAsync();

            var content = $@"<label for="""">{Label}</label><div class=""form-control-plaintext"" style=""{GetStyle()}"">{childContent.GetContent()}</div>";
            output.Content.SetHtmlContent(content);
        }

        string GetStyle()
        {
            switch (ContentStyle)
            {
                case FormDataStyle.None: return string.Empty;
                case FormDataStyle.Underline: return "text-decoration:underline;";
                case FormDataStyle.Singleline: return "border:2px solid black;padding:5px;";
                case FormDataStyle.Multiline: return "border:2px solid black;height:80px;padding:5px;";
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}