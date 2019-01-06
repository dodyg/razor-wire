using Microsoft.AspNetCore.Razor.TagHelpers;
using SK.TextGenerator;
using System.Text;
using System.Threading.Tasks;

namespace SK.RazorWire
{
    [HtmlTargetElement("accordions")]
    public class AccordionTagHelper : BaseTagHelper
    {
        public const string ParentIdKey = "accordions-id";

        public override void Init(TagHelperContext context)
        {
            Class = "accordion";
            context.Items.Add(ParentIdKey, Id);
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            base.InitProcess(output);

            var childContent = await output.GetChildContentAsync();
            output.Content.SetHtmlContent(childContent.GetContent());
        }
    }

    [HtmlTargetElement("accordioncontent", ParentTag = "accordions", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AccordionContentTagHelper : BaseTagHelper
    {
        public bool Active { get; set; }

        public string Title { get; set; }

        public string HeadingTag { get; set; } = "h5";

        public override void Init(TagHelperContext context)
        {
            Class = "card";
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            base.InitProcess(output);

            var childContent = await output.GetChildContentAsync();

            var parentId = context.Items[AccordionTagHelper.ParentIdKey] as string;

            var localId = Lorem.StringId();
            var content = new StringBuilder();
            content.AppendLine($@"
<div class=""card-header"">
    <{HeadingTag} class=""mb-0"">
        <button class=""btn btn-link"" type=""button"" data-toggle=""collapse"" data-target=""#{localId}"">
            { Title}
        </button>
    </{HeadingTag}>
</div>
<div id=""{localId}"" class=""collapse { (Active ? "show" : "")}"" data-parent=""#{parentId}"">
    <div class=""card-body"">
        {childContent.GetContent()}
    </div>
</div>
");
            output.Content.SetHtmlContent(content.ToString());
        }
    }
}