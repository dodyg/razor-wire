using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SK.RazorWire
{
    [HtmlTargetElement("column")]
    public class ColumnTagHelper : BaseTagHelper
    {
        public override void Init(TagHelperContext context)
        {
            Class = "col";
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.InitProcess(output);
        }
    }
}