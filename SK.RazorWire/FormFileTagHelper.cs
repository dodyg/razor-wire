using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SK.RazorWire
{
    [HtmlTargetElement("formfile")]
    public class FormFileTagHelper : BaseTagHelper
    {
        public string Label { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            InitProcess(output);

            var content = $@"
<div class=""custom-file"" >
    <input type=""file"" class=""custom-file-input"">
    <label class=""custom-file-label"">{Label}</label>
</div>
";
            output.Content.SetHtmlContent(content);
        }
    }
}