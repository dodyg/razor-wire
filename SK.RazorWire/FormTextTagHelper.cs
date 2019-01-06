using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SK.RazorWire
{
    [HtmlTargetElement("formtext")]
    public class FormTextTagHelper : BaseTagHelper
    {
        public string Label { get; set; }

        public string DefaultValue { get; set; }

        public bool Required { get; set; }

        public override void Init(TagHelperContext context)
        {
            Class = "form-group";
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            InitProcess(output);

            output.Attributes.Add("value", DefaultValue);

            if (Required)
                Label += " *";

            var content = $@"<label for="""">{Label}</label><input type=""text"" value=""{ DefaultValue}"" class=""form-control"" />";
            output.Content.SetHtmlContent(content);
        }
    }
}