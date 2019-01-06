using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SK.RazorWire
{
    [HtmlTargetElement("formcheckbox")]
    public class FormCheckboxTagHelper : BaseTagHelper
    {
        public string Label { get; set; }

        public override void Init(TagHelperContext context)
        {
            Class = "form-check";
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            InitProcess(output);

            var content = $@"
  <input class=""form-check-input"" type=""checkbox"">
  <label class=""form-check-label"">{Label}</label>
";

            output.Content.SetHtmlContent(content);
        }
    }
}