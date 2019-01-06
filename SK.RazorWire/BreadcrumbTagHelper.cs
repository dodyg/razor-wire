using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Text;

namespace SK.RazorWire
{
    [HtmlTargetElement("breadcrumb")]
    public class BreadcrumbTagHelper : BaseTagHelper
    {
        public IReadOnlyCollection<(string text, string uri)> Paths { get; set; } = new List<(string uri, string text)>();

        public override void Init(TagHelperContext context)
        {
            TagName = "nav";
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.InitProcess(output);

            var content = new StringBuilder();
            content.AppendLine(@"<ol class=""breadcrumb"">");
            var idx = 0;
            foreach (var p in Paths)
            {
                if (idx != Paths.Count - 1)
                    content.AppendLine($@"<li class=""breadcrumb-item""><a href=""{p.uri}"">{p.text}</a></li>");
                else
                    content.AppendLine($@"<li class=""breadcrumb-item active"">{p.text}</li>");

                idx++;
            }
            content.AppendLine("</ol>");

            output.Content.SetHtmlContent(content.ToString());
        }
    }
}