using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace SK.RazorWire
{
    public enum LinkType
    {
        Link,
        Button
    }

    [HtmlTargetElement("autolink")]
    public class AutoLinkTagHelper : BaseTagHelper
    {
        public string Url { get; set; }

        public string Label { get; set; }

        public LinkType Type { get; set; }

        public StyleContext ButtonStyle { get; set; }

        public override void Init(TagHelperContext context)
        {
            TagName = "a";

            if (Type == LinkType.Button)
                Class = $"btn {GetButtonStyle()}";
        }

        string GetButtonStyle()
        {
            switch (ButtonStyle)
            {
                case StyleContext.Primary: return "btn-primary";
                case StyleContext.Secondary: return "btn-secondary";
                case StyleContext.Success: return "btn-success";
                case StyleContext.Info: return "btn-info";
                case StyleContext.Warning: return "btn-warning";
                case StyleContext.Danger: return "btn-danger";
                case StyleContext.Dark: return "btn-dark";
                case StyleContext.Light: return "btn-light";
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            InitProcess(output);
            output.Attributes.Add("href", Url);
            output.Content.SetContent(Label);
        }
    }
}