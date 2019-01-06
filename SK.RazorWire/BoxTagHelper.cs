using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SK.RazorWire
{
    [HtmlTargetElement("box")]
    public class BoxTagHelper : BaseTagHelper
    {
        public string Height { get; set; } = "100px";

        public string Border { get; set; } = "2px";

        public override void Init(TagHelperContext context)
        {
            Style += $"display:flex;flex-direction:row;align-items:center;justify-content:center;height:{Height};";

            if (!string.IsNullOrWhiteSpace(Border))
                Style += $"border:{Border} solid black";
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.InitProcess(output);
        }
    }
}