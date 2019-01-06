using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace SK.RazorWire
{
    public enum FormButtonType
    {
        Submit,
        Button
    }

    public enum FormButtonSize
    {
        Normal,
        Small,
        Large
    }

    [HtmlTargetElement("formbutton")]
    public class FormButtonTagHelper : BaseTagHelper
    {
        public StyleContext ButtonStyle { get; set; }

        public FormButtonType Type { get; set; }

        public FormButtonSize Size { get; set; }

        public string Text { get; set; } = "submit";

        public string ToggleModalId { get; set; }

        public override void Init(TagHelperContext context)
        {
            TagName = "button";
            Class = $"btn {GetStyle()} {GetButtonSize()}";
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            InitProcess(output);

            output.Attributes.Add("type", GetButtonType());

            if (!string.IsNullOrWhiteSpace(ToggleModalId))
            {
                output.Attributes.Add("data-toggle", "modal");
                output.Attributes.Add("data-target", "#" + ToggleModalId);
            }

            output.Content.SetHtmlContent(Text);
        }

        string GetButtonType()
        {
            switch (Type)
            {
                case FormButtonType.Button: return "button";
                case FormButtonType.Submit: return "submit";
                default: throw new ArgumentOutOfRangeException();
            }
        }

        string GetButtonSize()
        {
            switch (Size)
            {
                case FormButtonSize.Large: return "btn-lg";
                case FormButtonSize.Small: return "btn-sm";
                case FormButtonSize.Normal: return "";
                default: throw new ArgumentOutOfRangeException();
            }
        }

        string GetStyle()
        {
            switch (ButtonStyle)
            {
                case StyleContext.Danger: return "btn-danger";
                case StyleContext.Dark: return "btn-dark";
                case StyleContext.Info: return "btn-info";
                case StyleContext.Light: return "btn-light";
                case StyleContext.Primary: return "btn-primary";
                case StyleContext.Secondary: return "btn-secondary";
                case StyleContext.Success: return "btn-success";
                case StyleContext.Warning: return "btn-warning";
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}