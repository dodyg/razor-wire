using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace SK.RazorWire
{
    [HtmlTargetElement("comment")]
    public class CommentTagHelper : BaseTagHelper
    {
        public override void Init(TagHelperContext context)
        {
            Class = "alert alert-secondary";
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            InitProcess(output);

            var child = await output.GetChildContentAsync();
            var childContent = child.GetContent();

            output.Content.AppendHtml(childContent);
        }
    }
}