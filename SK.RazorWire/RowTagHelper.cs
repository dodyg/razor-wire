using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SK.RazorWire
{
    [HtmlTargetElement("row")]
    public class RowTagHelper : BaseTagHelper
    {
        public override void Init(TagHelperContext context)
        {
            Class = "row";
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.InitProcess(output);
        }
    }
}