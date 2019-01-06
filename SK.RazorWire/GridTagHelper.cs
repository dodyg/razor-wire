using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SK.RazorWire
{
    [HtmlTargetElement("grid")]
    public class GridTagHelper : BaseTagHelper
    {
        public string Gap { get; set; }

        public string[] Columns { get; set; }

        public override void Init(TagHelperContext context)
        {
            if (!string.IsNullOrWhiteSpace(Gap))
                Style += $"grip-gap:{Gap};";

            if (Columns != null && Columns.Length > 0)
                Style += $"grid-template-columns: {string.Join(" ", Columns)};";
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.InitProcess(output);
        }
    }
}