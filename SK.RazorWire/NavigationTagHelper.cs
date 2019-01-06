using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SK.RazorWire
{
    public class Navigation
    {
        public string Url { get; set; }

        public string Label { get; set; }

        public List<(string label, string url)> Sub { get; set; } = new List<(string label, string url)>();

        public Navigation()
        {
        }

        public Navigation(string label, string url)
        {
            Url = url;
            Label = label;
        }

        public Navigation(string label, params (string label, string url)[] subs)
        {
            Label = label;
            Sub = subs.ToList();
        }
    }

    [HtmlTargetElement("navigation")]
    public class NavigationTagHelper : BaseTagHelper
    {
        public List<Navigation> Navigations { get; set; }

        public override void Init(TagHelperContext context)
        {
            TagName = "ul";
            Class = "nav nav-tabs";
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            InitProcess(output);

            var builder = new StringBuilder(400);

            foreach (var n in Navigations)
                builder.AppendLine(Nav(n));

            output.Content.SetHtmlContent(builder.ToString());
        }

        string Nav(Navigation n)
        {
            string Sub((string label, string url) s)
            {
                if (s == ("-", "-"))
                    return @"<div class=""dropdown-divider""></div>";
                else
                    return $@"<a class=""dropdown-item"" href=""{s.url}"">{s.label}</a>";
            }

            if (n.Sub.Count == 0)
                return $@"<li class=""nav-item""><a class=""nav-link"" href=""{n.Url}"">{n.Label}</a></li>";
            else
            {
                var sub = new StringBuilder(200);
                foreach (var s in n.Sub)
                    sub.AppendLine(Sub(s));

                return $@"
<li class=""nav-item-dropdown"">
    <a class=""nav-link dropdown-toggle"" data-toggle=""dropdown"" href=""#"">{n.Label}</a>
    <div class=""dropdown-menu"">
    {sub}
    </div>
</li>
  ";
            }
        }
    }
}