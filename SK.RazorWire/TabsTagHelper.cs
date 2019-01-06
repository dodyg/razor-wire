using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SK.RazorWire
{
    [HtmlTargetElement("tabs")]
    [RestrictChildren("tabcontent")]
    public class TabsTaghelper : BaseTagHelper
    {
        public const string ContentKey = "tabs-contents";

        public List<(string title, string id, bool isActive)> Contents { get; set; } = new List<(string title, string id, bool isActive)>();

        public override void Init(TagHelperContext context)
        {
            context.Items.Add(ContentKey, Contents);
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            base.InitProcess(output);

            var childContent = await output.GetChildContentAsync();

            var contents = context.Items[ContentKey] as List<(string title, string id, bool isActive)>;

            var content = new StringBuilder(500);
            content.AppendLine($@"<ul class=""nav nav-tabs"">");
            foreach (var c in contents)
            {
                content.Append(@"<li class=""nav-item"">");
                if (c.isActive)
                    content.Append($@"<a class=""nav-link active"" data-toggle=""tab"" href=""#{c.id}"">{c.title}</a>");
                else
                    content.Append($@"<a class=""nav-link"" data-toggle=""tab"" href=""#{c.id}"">{c.title}</a>");

                content.AppendLine("</li>");
            }
            content.AppendLine("</ul>");

            content.AppendLine($@"<div class=""tab-content"">{childContent.GetContent()}</div>");
            output.Content.SetHtmlContent(content.ToString());
        }
    }

    [HtmlTargetElement("tabcontent", ParentTag = "tabs", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class TabContentTagHelper : BaseTagHelper
    {
        public bool Active { get; set; }

        public string Title { get; set; }

        public override void Init(TagHelperContext context)
        {
            Class = "tab-pane container-fluid";

            if (Active)
                Class += " active";
            else
                Class += " fade";
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.InitProcess(output);

            var contents = context.Items[TabsTaghelper.ContentKey] as List<(string title, string id, bool isActive)>;
            contents.Add((Title, Id, Active));
        }
    }
}