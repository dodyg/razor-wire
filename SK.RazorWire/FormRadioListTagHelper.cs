using Microsoft.AspNetCore.Razor.TagHelpers;
using SK.TextGenerator;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SK.RazorWire
{
    public class FormRadioListContent
    {
        public string Label { get; set; }

        public List<(string value, string text)> List { get; set; }

        public FormRadioListContent()
        {
        }

        public FormRadioListContent(string label, params string[] items)
        {
            Label = label;
            List = items.Select(x => (x, x)).ToList();
        }

        public FormRadioListContent(string label, params (string value, string text)[] items)
        {
            Label = label;
            List = items.ToList();
        }
    }

    [HtmlTargetElement("formradiolist")]
    public class FormRadioListTagHelper : BaseTagHelper
    {
        public FormRadioListContent Content { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            InitProcess(output);

            var builder = new StringBuilder(200);

            var name = Lorem.Words(1).ToLower();
            foreach (var c in Content.List)
            {
                builder.AppendLine(@"<div class=""form-check"">");
                builder.AppendLine($@"<input class=""form-check-input"" type=""radio"" name=""{name}"" value=""{c.value}"">");
                builder.AppendLine($@"<label class=""form-check-label"">{c.text}</label>");
                builder.AppendLine("</div>");
            }
            output.Content.SetHtmlContent(builder.ToString());
        }
    }
}