using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SK.RazorWire
{
    [HtmlTargetElement("formpassword")]
    public class FormPasswordTagHelper : BaseTagHelper
    {
        public string Label { get; set; }

        public override void Init(TagHelperContext context)
        {
            Class = "form-group";
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            InitProcess(output);

            var content = $@"
<label for="""">{Label}</label>
<input type=""password"" class=""form-control"" />
";

            output.Content.SetHtmlContent(content);
        }
    }
}