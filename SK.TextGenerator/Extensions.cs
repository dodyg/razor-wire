using System;

namespace SK.TextGenerator
{
    //https://github.com/dochoffiday/Lorem.NET/blob/master/Lorem.NET/Lorem.cs
    internal static class Extensions
    {
        internal static String Remove(this string s, string pattern)
            => s.Replace(pattern, "");

        internal static String[] Split(this string s, string separator)
            => s.Split(new[] { separator }, StringSplitOptions.None);

        internal static string UppercaseFirst(this string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}