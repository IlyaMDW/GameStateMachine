using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Extensions
{
    [Serializable]
    public class StringReplacer
    {
        public string From;
        public string To;

        public static StringReplacer Create(string from, string to)
        {
            return new StringReplacer {From = from, To = to};
        }
    }

    public static class StringExtensions
    {
        public static string Replace(this string stringToModificate, IEnumerable<StringReplacer> replacers)
        {
            if (stringToModificate.IsNullOrWhitespace() || !replacers.Any() || replacers.Any(x => x == null)) return stringToModificate;

            var str = new StringBuilder(stringToModificate);
            replacers.ForEach(x => str.Replace(x.From, x.To));
            return str.ToString();
        }

        public static string JoinStrings<T>(this IEnumerable<T> source, Func<T, string> projection, string separator)
        {
            if (source == null) return null;
            var builder = new StringBuilder();
            var first = true;
            foreach (var element in source)
            {
                if (first)
                    first = false;
                else
                    builder.Append(separator);

                builder.Append(projection(element));
            }

            return builder.ToString();
        }


        public static string GetAdaptedInt(ulong intToAdapt)
        {
            if (intToAdapt > MathUtils.GetPowULong(10, 19))
            {
                return $"{intToAdapt / MathUtils.GetPowULong(10, 18)}Z";
            }

            if (intToAdapt > MathUtils.GetPowULong(10, 16))
            {
                return $"{intToAdapt / MathUtils.GetPowULong(10, 15)}A";
            }

            if (intToAdapt > MathUtils.GetPowULong(10, 13))
            {
                return $"{intToAdapt / MathUtils.GetPowULong(10, 12)}Q";
            }

            if (intToAdapt > MathUtils.GetPowULong(10, 10))
            {
                return $"{intToAdapt / MathUtils.GetPowULong(10, 9)}B";
            }

            if (intToAdapt > MathUtils.GetPowULong(10, 7))
            {
                return $"{intToAdapt / MathUtils.GetPowULong(10, 6)}M";
            }

            if (intToAdapt > MathUtils.GetPowULong(10, 4))
            {
                return $"{intToAdapt / MathUtils.GetPowULong(10, 3)}K";
            }

            return intToAdapt.ToString();
        }

        public static string JoinStrings<T>(this IEnumerable<T> source, string separator)
        {
            return JoinStrings(source, t => t.ToString(), separator);
        }

        /// <summary>
        ///     Returns true if this string is null, empty, or contains only whitespace.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <returns><c>true</c> if this string is null, empty, or contains only whitespace; otherwise, <c>false</c>.</returns>
        public static bool IsNullOrWhitespace(this string str)
        {
            if (!string.IsNullOrEmpty(str))
                for (var index = 0; index < str.Length; ++index)
                    if (!char.IsWhiteSpace(str[index]))
                        return false;
            return true;
        }
    }
}