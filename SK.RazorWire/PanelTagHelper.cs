using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SK.RazorWire
{
    [HtmlTargetElement("panel")]
    public class PanelTagHelper : BaseTagHelper
    {
        public string Title { get; set; }

        public bool Collapse { get; set; }

        public string AnchorName { get; set; }

        public IReadOnlyCollection<(string text, string action)> Headers { get; set; } = new List<(string text, string jsAction)>();

        public string HeaderStyle { get; set; }

        public override void Init(TagHelperContext context)
        {
            Class = $"card {(Collapse ? "collapse" : "")}";
            Style = "margin-bottom:10px;margin-top:10px;";
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            InitProcess(output);

            if (!string.IsNullOrWhiteSpace(AnchorName))
                output.PreElement.AppendHtml($@"<a name=""{AnchorName}""/>");

            if (Collapse)
            {
                var collapseButton = $@"
<button class=""btn btn-primary btn-sm"" style=""margin:5px"" type=""button"" data-toggle=""collapse"" data-target=""#{Id}"">
    {Title}
</button>
";
                output.PreElement.AppendHtml(collapseButton);
            }

            var content = new StringBuilder(1000);

            //process header
            if (Headers.Count > 0)
            {
                var style = !string.IsNullOrWhiteSpace(HeaderStyle) ? $@"style=""{HeaderStyle}""" : "";

                var headers = new StringBuilder();
                foreach (var h in Headers)
                {
                    if (string.IsNullOrWhiteSpace(h.action))
                        headers.Append(h.text + " ");
                    else
                    {
                        if (System.Uri.TryCreate(h.action, System.UriKind.Absolute, out var res) && (res.Scheme == System.Uri.UriSchemeHttp || res.Scheme == System.Uri.UriSchemeHttps))
                            headers.Append($@"<a href=""{h.action}"">{h.text}</a>" + " ");
                        else if (h.action.IndexOf('/') == 0)
                            headers.Append($@"<a href=""{h.action}"">{h.text}</a>" + " ");
                        else
                            headers.Append($@"<span onclick=""{h.action}"" style=""cursor:pointer;"">{h.text}</span>");
                    }
                }
                content.AppendLine($@"<div class=""card-header"" {style}>{headers}</div>");
            }

            //process body
            var title = string.Empty;

            if (!string.IsNullOrWhiteSpace(Title))
            {
                title = $@"<h5 class=""card-title"">{Title}</h5>";
            }

            var child = await output.GetChildContentAsync();
            var childContent = child.GetContent();

            var body = $@"<div class=""card-body"">
{title}
{childContent}
</div>
";
            content.AppendLine(body);
            output.Content.AppendHtml(content.ToString());
        }
    }
}