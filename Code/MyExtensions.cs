using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IJPReporting
{
    public static class StringExtensions
    {
        public static string TruncateText(this string text, int nbCaractere)
        {
            string truncText = RemoveHTMLTag(text);
            if (truncText.Length > nbCaractere)
            {
                truncText = truncText.Substring(0, nbCaractere) + "(...)";

            }

            return truncText;
        }

        public static string RemoveHTMLTag(this string html)
        {

            if (html == null || html == string.Empty)
                return string.Empty;

            return System.Text.RegularExpressions.Regex.Replace(html, "<[^>]*>", string.Empty);
        }

        public static int ToInt32(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return 0;

            int parsed;
            Int32.TryParse(text, out parsed);

            return parsed;
        }

        public static int? ToInt32Nullable(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            int parsed;
            Int32.TryParse(text, out parsed);

            if (parsed <= 0)
                return null;

            return parsed;
        }

        public static IOrderedEnumerable<T> NullableOrderBy<T>(this IEnumerable<T> list, Func<T, object> parentKeySelector, Func<T, object> childKeySelector)
        {
            return list.OrderBy(v => parentKeySelector(v) != null ? 0 : 1).ThenBy(childKeySelector);
        }

        public static IOrderedEnumerable<T> NullableOrderByDescending<T>(this IEnumerable<T> list, Func<T, object> parentKeySelector, Func<T, object> childKeySelector)
        {
            return list.OrderByDescending(v => parentKeySelector(v) != null ? 0 : 1).ThenBy(childKeySelector);
        }

    }
}