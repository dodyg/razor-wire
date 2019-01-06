using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace SK.RazorWire
{
    [HtmlTargetElement("alert")]
    public class AlertTagHelper : BaseTagHelper
    {
        public StyleContext AlertStyle { get; set; }

        public override void Init(TagHelperContext context)
        {
            Class = $"alert {GetAlertStyle()}";
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.InitProcess(output);
        }

        string GetAlertStyle()
        {
            switch (AlertStyle)
            {
                case StyleContext.Primary: return "alert-primary";
                case StyleContext.Secondary: return "alert-secondary";
                case StyleContext.Success: return "alert-success";
                case StyleContext.Info: return "alert-info";
                case StyleContext.Warning: return "alert-warning";
                case StyleContext.Danger: return "alert-danger";
                case StyleContext.Dark: return "alert-dark";
                case StyleContext.Light: return "alert-light";
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}