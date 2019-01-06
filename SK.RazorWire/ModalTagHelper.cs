using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SK.RazorWire
{
    public enum ModalSize
    {
        Normal,
        Small,
        Large
    }

    public static class Modal
    {
        public static (string text, Dictionary<string, string> attributes) Button(string text, params (string key, string value)[] attributes)
        {
            var dict = new Dictionary<string, string>(5);
            foreach (var a in attributes)
                dict[a.key] = a.value;

            return (text, dict);
        }
    }

    [HtmlTargetElement("modal")]
    public class ModalTagHelper : BaseTagHelper
    {
        public string Title { get; set; }

        public ModalSize Size { get; set; }

        public string TriggerButton { get; set; } = "Modal";

        public string TriggerButtonStyle { get; set; } = "margin:5px;";

        public IReadOnlyCollection<(string text, Dictionary<string, string> attributes)> Buttons { get; set; } = new(string text, Dictionary<string, string> attributes)[] { };

        public override void Init(TagHelperContext context)
        {
            Class = "modal fade";
        }

        string GetModalSize()
        {
            switch (Size)
            {
                case ModalSize.Normal: return string.Empty;
                case ModalSize.Small: return "modal-sm";
                case ModalSize.Large: return "modal-lg";
                default: throw new System.ArgumentOutOfRangeException();
            }
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            InitProcess(output);

            output.Attributes.Add("tabindex", "-1");
            output.Attributes.Add("role", "dialog");

            if (!string.IsNullOrWhiteSpace(TriggerButton))
            {
                output.PreElement.AppendHtml(
    $@"<button type=""button"" class=""btn btn-primary"" style=""{TriggerButtonStyle}"" data-toggle=""modal"" data-target=""#{Id}"">
  { TriggerButton }
</button>");
            }

            var content = await output.GetChildContentAsync();
            var modal = $@"
<div class=""modal-dialog {GetModalSize()}"" role=""document"">
    <div class=""modal-content"">
          <div class=""modal-header"" >
            <h5 class=""modal-title"">{Title}</h5>
          </div>
          <div class=""modal-body"">
            { content.GetContent() }
          </div>
          <div class=""modal-footer"">
            { BuildButtons()}
            <button type=""button"" class=""btn btn-secondary"" data-dismiss=""modal"">Close</button>
          </div>
    </div>
</div>
";

            output.Content.SetHtmlContent(modal);
        }

        public string BuildButtons()
        {
            var str = new StringBuilder(1000);

            foreach (var b in Buttons)
            {
                str.Append(@"<button type=""button""");
                foreach (var k in b.attributes)
                {
                    str.Append(' ');
                    str.Append($@"{k.Key}=""{k.Value}""");
                }
                str.Append($@">{b.text}</button>");
                str.AppendLine();
            }
            return str.ToString();
        }
    }
}