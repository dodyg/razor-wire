using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SK.RazorWire
{
    [HtmlTargetElement("formtextarea")]
    public class FormTextAreaTagHelper : BaseTagHelper
    {
        public string Label { get; set; }

        public int Cols { get; set; } = 80;

        public int Rows { get; set; } = 6;

        public bool Required { get; set; }

        public string DefaultValue { get; set; }

        public override void Init(TagHelperContext context)
        {
            Class = "form-group";
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            InitProcess(output);

            if (Required)
                Label += " *";

            var content = $@"
<label>{Label}</label>
<textarea class=""form-control"" rows=""{Rows}"" cols=""{Cols}"">{DefaultValue}</textarea>";

            output.Content.AppendHtml(content);
        }
    }
}