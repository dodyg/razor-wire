using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SK.RazorWire
{
    public class FormSelectListContent
    {
        public string Label { get; set; }

        public List<(string value, string text)> List { get; set; }

        public bool IsMultiple { get; set; }

        public FormSelectListContent()
        {
        }

        public FormSelectListContent(string label, params string[] items)
        {
            Label = label;
            List = items.Select(x => (x, x)).ToList();
        }

        public FormSelectListContent(string label, params (string value, string text)[] items)
        {
            Label = label;
            List = items.ToList();
        }
    }

    [HtmlTargetElement("formselectlist")]
    public class FormSelectListTagHelper : BaseTagHelper
    {
        public FormSelectListContent Content { get; set; }

        public override void Init(TagHelperContext context)
        {
            Class = "form-group";
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            InitProcess(output);

            var builder = new StringBuilder(200);
            builder.AppendLine($"<label>{Content.Label}</label>");
            if (Content.IsMultiple)
                builder.AppendLine(@"<select class=""form-control"" multiple>");
            else
                builder.AppendLine(@"<select class=""form-control"">");

            foreach (var i in Content.List)
                builder.AppendLine($@"<option value=""{i.value}"">{i.text}</option>");

            builder.AppendLine("</select>");
            output.Content.SetHtmlContent(builder.ToString());
        }
    }
}