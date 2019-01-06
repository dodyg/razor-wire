using Microsoft.AspNetCore.Razor.TagHelpers;
using Scriban;
using SK.TextGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SK.RazorWire
{
    public enum AutoColumnType
    {
        Number,
        ShortText,
        LongText,
        Date,
        Time,
        DateTime,
        Email,
        FirstName,
        LastName,
        Name,
        YesNo,
        Link,
        ButtonLink,
        Enumeration,
        MultiEnumeration,
        MultiEnumerationAll,
        Address,
        Templated,
        MultiRows,
        MultiColumns,
        Conditional
    }

    public enum NestedColumn
    {
        Rows,
        Columns,
        Either
    }

    public enum ColumnEnumerationType
    {
        Single,
        MultiAll,
        Multi
    }

    public class AutoColumn
    {
        public string Name { get; set; }

        public AutoColumnType Type { get; set; }

        public string Url { get; set; }

        public IReadOnlyCollection<(string attribute, string value)> UrlAttributes = new List<(string attribute, string value)>();

        public string Text { get; set; }

        public IReadOnlyCollection<string> Options { get; set; } = new List<string>();

        public IReadOnlyCollection<(string icon, string iconStyle, string text)> IconOptions { get; set; } = new List<(string icon, string iconStyle, string text)>();

        public Func<object> Formatter { get; set; }

        public List<AutoColumn> NestedItems = new List<AutoColumn>();

        public Func<Dictionary<string, object>, AutoColumn, int> OptionsSelector { get; set; }

        public Func<Dictionary<string, object>, AutoColumn, int> IconOptionsSelector { get; set; }

        public AutoColumn()
        {
        }

        public AutoColumn(string name, AutoColumnType type = AutoColumnType.ShortText)
        {
            Name = name;
            Type = type;
        }

        public AutoColumn(string name, AutoColumnType type, string uri) : this(name, type)
        {
            Url = uri;
        }

        public AutoColumn(string name, AutoColumnType type, (string uri, IReadOnlyCollection<(string attribute, string value)> attributes) url) : this(name, type)
        {
            Url = url.uri;
            UrlAttributes = url.attributes;
        }

        public AutoColumn(string name, AutoColumnType type, string uri, string text) : this(name, type)
        {
            Url = uri;
            Text = text;
        }

        public AutoColumn(string name, AutoColumnType type, (string uri, IReadOnlyCollection<(string attribute, string value)> attributes) url, string text) : this(name, type)
        {
            Url = url.uri;
            UrlAttributes = url.attributes;
            Text = text;
        }

        public AutoColumn((string name, string uri) col, params string[] options) : this(col.name, AutoColumnType.Enumeration)
        {
            Url = col.uri;
            Options = options.ToList();
        }

        public AutoColumn((string name, string uri) col, (ColumnEnumerationType type, Func<Dictionary<string, object>, AutoColumn, int> selector, string[] list) options)
        {
            Name = col.name;
            switch (options.type)
            {
                case ColumnEnumerationType.Single:
                    Type = AutoColumnType.Enumeration;
                    break;

                case ColumnEnumerationType.Multi:
                    Type = AutoColumnType.MultiEnumeration;
                    break;

                case ColumnEnumerationType.MultiAll:
                    Type = AutoColumnType.MultiEnumerationAll;
                    break;
            }

            Url = col.uri;
            Options = options.list.ToList();
            OptionsSelector = options.selector;
        }

        public AutoColumn((string name, string uri) col, (ColumnEnumerationType type, Func<Dictionary<string, object>, AutoColumn, int> selector, (string icon, string iconStyle, string text)[] list) options)
        {
            Name = col.name;
            switch (options.type)
            {
                case ColumnEnumerationType.Single:
                    Type = AutoColumnType.Enumeration;
                    break;

                case ColumnEnumerationType.Multi:
                    Type = AutoColumnType.MultiEnumeration;
                    break;

                case ColumnEnumerationType.MultiAll:
                    Type = AutoColumnType.MultiEnumerationAll;
                    break;
            }

            Url = col.uri;
            IconOptions = options.list.ToList();
            IconOptionsSelector = options.selector;
        }

        public AutoColumn(string name, string text, Func<object> formatter) : this(name, AutoColumnType.Templated)
        {
            Text = text;
            Formatter = formatter;
        }

        public AutoColumn(string name, NestedColumn nested, params AutoColumn[] items)
        {
            Name = name;
            if (nested == NestedColumn.Rows)
            {
                Type = AutoColumnType.MultiRows;
                NestedItems = items.ToList();
            }
            else
                if (nested == NestedColumn.Columns)
            {
                Type = AutoColumnType.MultiColumns;
                NestedItems = items.ToList();
            }
            else if (nested == NestedColumn.Either)
            {
                Type = AutoColumnType.Conditional;
                NestedItems = items.ToList();
            }
        }
    }

    public class AutoTable
    {
        public bool ShowHeader { get; set; } = true;

        public bool ShowRecordCount { get; set; }

        public string RecordLabel { get; set; } = "record";

        public string RecordPluralLabel { get; set; } = "records";

        public List<AutoColumn> Columns { get; set; } = new List<AutoColumn>();

        public Dictionary<string, object> Items { get; set; } = new Dictionary<string, object>();

        public int RowCount { get; set; } = 10;

        public int PaginationCount { get; set; } = 10;

        public AutoTable()
        {
        }

        public AutoTable(params AutoColumn[] cols)
        {
            Columns = cols.ToList();
        }

        public AutoTable((bool? showHeader, bool? showRecordCount, int? rowCount, int? paginationCount) options, params AutoColumn[] cols) : this(cols)
        {
            if (options.showHeader.HasValue)
                ShowHeader = options.showHeader.Value;

            if (options.showRecordCount.HasValue)
                ShowRecordCount = options.showRecordCount.Value;

            if (options.rowCount.HasValue)
                RowCount = options.rowCount.Value;

            if (options.paginationCount.HasValue)
                PaginationCount = options.paginationCount.Value;
        }

        public AutoTable((bool? showHeader, (string recordLabel, string recordPluralLabel)? recordLabels, int? rowCount, int? paginationCount) options, params AutoColumn[] cols) : this(cols)
        {
            if (options.showHeader.HasValue)
                ShowHeader = options.showHeader.Value;

            if (options.recordLabels.HasValue)
            {
                ShowRecordCount = true;
                RecordLabel = options.recordLabels.Value.recordLabel;
                RecordPluralLabel = options.recordLabels.Value.recordPluralLabel;
            }

            if (options.rowCount.HasValue)
                RowCount = options.rowCount.Value;

            if (options.paginationCount.HasValue)
                PaginationCount = options.paginationCount.Value;
        }
    }

    [HtmlTargetElement("autotable")]
    public class AutoTableTagHelper : BaseTagHelper
    {
        public AutoTable Content { get; set; }

        public override void Init(TagHelperContext context)
        {
            TagName = "table";
            Class = "table";
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            InitProcess(output);

            var content = new StringBuilder(10000);

            if (Content.ShowRecordCount)
                content.AppendLine($"<div>{GenerateRecordCount()}</div>");

            if (Content.ShowHeader)
                content.AppendLine(GenerateHeader());

            content.AppendLine(GenerateBody());

            if (Content.PaginationCount > 0)
                output.PostElement.AppendHtml(GeneratePagination());

            output.Content.SetHtmlContent(content.ToString());
        }

        string GenerateRecordCount()
        {
            if (Content.PaginationCount == 0)
            {
                if (Content.RowCount > 1)
                    return $"{Content.RowCount} {Content.RecordPluralLabel}";
                else if (Content.RowCount == 1)
                    return $"1 {Content.RecordLabel}";
                else
                    return $"No {Content.RecordLabel} found";
            }
            else
            {
                if (Content.RowCount > 1)
                    return $"{Content.RowCount} {Content.RecordPluralLabel} shown";
                else if (Content.RowCount == 1)
                    return $"1 {Content.RecordLabel} shown";
                else
                    return $"No {Content.RecordLabel} shown";
            }
        }

        string GenerateHeader()
        {
            string GenerateColumn(AutoColumn col)
            {
                return $@"<th scope=""col"">{col.Name}</th>";
            }

            var builder = new StringBuilder(200);
            builder.AppendLine("<thead>");
            foreach (var col in Content.Columns)
                builder.AppendLine(GenerateColumn(col));

            builder.AppendLine("</thead>");

            return builder.ToString();
        }

        string GenerateBody()
        {
            var builder = new StringBuilder(1000);
            builder.AppendLine("<tbody>");

            foreach (var r in Enumerable.Range(0, Content.RowCount))
            {
                builder.AppendLine("<tr>");

                foreach (var col in Content.Columns)
                    builder.AppendLine($"<td>{GenerateColumnContent(col)}</td>");

                builder.AppendLine("</tr>");
            }

            builder.AppendLine("</tbody>");
            return builder.ToString();
        }

        string GenerateColumnContent(AutoColumn col)
        {
            string Url(string uri, IReadOnlyCollection<(string attribute, string value)> attributes, string text)
            {
                string BuildAttributes()
                {
                    var b = new StringBuilder(100);
                    foreach (var (att, val) in attributes)
                    {
                        if (att.Equals("modal", StringComparison.InvariantCulture))
                            b.Append($@"data-toggle=""modal"" data-target=""{val}""");
                        else
                            b.Append($@"{att}=""{val}""");

                        b.Append(' ');
                    }

                    return b.ToString();
                }

                if (!string.IsNullOrWhiteSpace(uri))
                    return $@"<a href=""{uri}"" {BuildAttributes()}>{text}</a>";

                return text;
            }

            string FormatUrl(string txt)
            {
                return Url(col.Url, col.UrlAttributes, txt);
            }

            switch (col.Type)
            {
                case AutoColumnType.Number:
                    return FormatUrl(Lorem.Number(1, 100).ToString());

                case AutoColumnType.DateTime:
                    return FormatUrl(Lorem.DateTime().ToString());

                case AutoColumnType.Date:
                    return FormatUrl(Lorem.DateTime().Date.ToString("dd-MM-yyyy"));

                case AutoColumnType.Time:
                    return FormatUrl(Lorem.DateTime().ToShortTimeString());

                case AutoColumnType.Email:
                    return FormatUrl(Lorem.Email());

                case AutoColumnType.FirstName:
                    return FormatUrl(Lorem.Names(1));

                case AutoColumnType.LastName:
                    return FormatUrl(Lorem.Names(1, 2));

                case AutoColumnType.Name:
                    return FormatUrl(Lorem.Names(2, 4));

                case AutoColumnType.LongText:
                    return FormatUrl(Lorem.Paragraph(100, 30));

                case AutoColumnType.Address:
                    return FormatUrl(Lorem.Paragraph(2, 3));

                case AutoColumnType.ShortText:
                    return FormatUrl(Lorem.Words(wordCountMin: 2, wordCountMax: 10));

                case AutoColumnType.YesNo:
                    return FormatUrl(Lorem.YesNo());

                case AutoColumnType.Link:
                    return FormatUrl(col.Text);

                case AutoColumnType.Enumeration:
                    return FormatUrl(BuildOptions(col));

                case AutoColumnType.MultiEnumeration:
                    return FormatUrl(BuildMultiOptions(col));

                case AutoColumnType.MultiEnumerationAll:
                    return FormatUrl(BuildMultiOptionsAll(col));

                case AutoColumnType.ButtonLink:
                    return $@"<button type=""button"" class=""btn btn-sm"">{FormatUrl(col.Text)}</button>";

                case AutoColumnType.Templated:
                    return BuildTemplatedText(col);

                case AutoColumnType.MultiRows:
                    return BuildMultiRows(col);

                case AutoColumnType.MultiColumns:
                    return BuildMultiColumns(col);

                case AutoColumnType.Conditional:
                    return BuildConditional(col);

                default: throw new ArgumentOutOfRangeException();
            }
        }

        string BuildConditional(AutoColumn col)
        {
            var options = col.NestedItems.ToArray();
            var selected = Lorem.Random(options);

            if (!string.IsNullOrWhiteSpace(col.Name))
                Content.Items[col.Name] = selected.Name;

            if (string.IsNullOrWhiteSpace(selected.Name))
                return GenerateColumnContent(selected);
            else
                return GenerateColumnContent(selected);
        }

        string BuildMultiColumns(AutoColumn col)
        {
            var content = new StringBuilder(400);
            content.AppendLine(@"<div class=""row"">");

            foreach (var c in col.NestedItems)
            {
                content.AppendLine(@"<div class=""col"">");
                content.AppendLine(GenerateColumnContent(c));
                content.Append("</div>");
            }
            content.AppendLine("</div>");

            return content.ToString();
        }

        string BuildMultiRows(AutoColumn col)
        {
            var content = new StringBuilder(400);
            foreach (var row in col.NestedItems)
            {
                content.AppendLine(GenerateColumnContent(row));
                content.Append("<br/>");
            }

            return content.ToString();
        }

        string BuildOptions(AutoColumn col)
        {
            if (col.Options.Count > 0)
            {
                if (col.OptionsSelector != null)
                {
                    var selectedIdx = col.OptionsSelector(Content.Items, col);
                    return col.Options.ElementAt(selectedIdx);
                }

                return Lorem.Random(col.Options.ToArray());
            }
            else if (col.IconOptions.Count > 0)
            {
                if (col.IconOptionsSelector != null)
                {
                    var selectedIdx = col.IconOptionsSelector(Content.Items, col);
                    var selected2 = col.IconOptions.ElementAt(selectedIdx);
                    return $@"<i class=""{selected2.icon}"" title=""{selected2.text}"" style=""{selected2.iconStyle}"" data-toggle=""tooltip"" data-placement=""bottom""></i>";
                }

                var selected = Lorem.Random(col.IconOptions.ToArray());
                return $@"<i class=""{selected.icon}"" title=""{selected.text}"" style=""{selected.iconStyle}"" data-toggle=""tooltip"" data-placement=""bottom""></i>";
            }

            return string.Empty;
        }

        string BuildMultiOptions(AutoColumn col)
        {
            if (col.Options.Count > 0)
            {
                var options = col.Options.ToArray();
                var upperBound = options.Length;
                var items = new List<string>(upperBound);

                foreach (var x in Enumerable.Range(1, upperBound))
                {
                    var selected = Lorem.Random(options);
                    if (!items.Contains(selected))
                        items.Add(selected);
                }

                return string.Join(", ", items);
            }
            else if (col.IconOptions.Count > 0)
            {
                var options = col.IconOptions.ToArray();
                var upperBound = options.Length;
                var items = new List<string>(upperBound);

                foreach (var x in Enumerable.Range(1, upperBound))
                {
                    var (icon, iconStyle, text) = Lorem.Random(options);
                    var selectedIcon = $@"<i class=""{icon}"" style=""{iconStyle}"" title=""{text}"" data-toggle=""tooltip"" data-placement=""bottom""></i>";
                    if (!items.Contains(selectedIcon))
                        items.Add(selectedIcon);
                }

                return string.Join(" ", items);
            }

            return string.Empty;
        }

        string BuildMultiOptionsAll(AutoColumn col)
        {
            if (col.Options.Count > 0)
                return string.Join(",", col.Options);
            else if (col.IconOptions.Count > 0)
            {
                var options = col.IconOptions.ToArray();
                var items = new List<string>(options.Length);

                foreach (var (icon, iconStyle, text) in col.IconOptions)
                {
                    var selectedIcon = $@"<i class=""{icon}"" style=""{iconStyle}"" title=""{text}"" data-toggle=""tooltip"" data-placement=""bottom""></i>";
                    items.Add(selectedIcon);
                }

                return string.Join(" ", items);
            }

            return string.Empty;
        }

        string BuildTemplatedText(AutoColumn col)
        {
            if (col.Formatter == null)
                return col.Text;

            var parsed = Template.Parse(col.Text);
            return parsed.Render(col.Formatter());
        }

        string GeneratePagination()
        {
            var builder = new StringBuilder(300);
            builder.AppendLine("<nav>");
            builder.AppendLine(@"<ul class=""pagination"">");
            builder.AppendLine(@"<li class=""page-item""><a class=""page-link"" href=""#"">Previous</a></li>");

            foreach (var x in Enumerable.Range(0, Content.PaginationCount))
                builder.AppendLine($@"<li class=""page-item""><a class=""page-link"" href=""#"">{x}</a></li>");

            builder.AppendLine(@"<li class=""page-item""><a class=""page-link"" href=""#"">Next</a></li>");
            builder.AppendLine("</ul>");
            builder.AppendLine("</nav>");
            return builder.ToString();
        }
    }
}