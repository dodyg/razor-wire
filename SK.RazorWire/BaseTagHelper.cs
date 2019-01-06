using Microsoft.AspNetCore.Razor.TagHelpers;
using SK.TextGenerator;

namespace SK.RazorWire
{
    public abstract class BaseTagHelper : TagHelper
    {
        public string Style { get; set; }

        public string Class { get; set; }

        public TagMode TagMode { get; set; }

        public string TagName { get; set; } = "div";

        public string Id { get; set; } = Lorem.StringId();

        public void InitProcess(TagHelperOutput output)
        {
            output.TagName = TagName;
            output.TagMode = TagMode;

            if (!string.IsNullOrWhiteSpace(Id))
                output.Attributes.Add("id", Id);

            if (!string.IsNullOrWhiteSpace(Style))
                output.Attributes.Add("style", Style);

            if (!string.IsNullOrWhiteSpace(Class))
                output.Attributes.Add("class", Class);
        }
    }
}