using System.Linq;

namespace CrossCutting.Util
{
    public static class StringExtensions
    {
        private static readonly string[] _namePrepositions =
        {
            // Este array contém apenas algumas preposições de nome. Adicionar mais caso seja necessário

            // Português
            "a", "à", "ao", "aos", "as", "às", "da", "das", "de", "do", "dos", "e", "em", "na", "nas", "no", "nos",
            // Espanhol
            "del", "la", "las", "lo", "los", "y",
            // Alemão
            "an", "am", "aus", "bei", "dem", "der", "von", "zu", "zum", "zur", 
            // Holandês
            "den", "ten", "van",
            // Francês
            "du", "des", "la", "las", "le", "les",
            // Italiano
            "dal", "dalla", "dei", "di", "della", "delle",
            // Sueco
            "af", "av", "den", "det", "en", "ett"
        };

        public static string FormatPersonName(this string name)
        {
            var formatedNames = name.ToLower()
                .Replace(".", ". ")
                .Split(' ', System.StringSplitOptions.RemoveEmptyEntries)
                .Select(n => _namePrepositions.Contains(n)
                    ? n : $"{n[0].ToString().ToUpper()}{(n.Count() > 1 ? n.Substring(1) : "")}");

            return string.Join(' ', formatedNames);
        }

        public static string ToLowerFirst(this string value)
        {
            if (string.IsNullOrEmpty(value) || char.IsLower(value[0]))
                return value;

            return char.ToLowerInvariant(value[0]) + value.Substring(1);
        }

        public static string Capitalize(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;
            return char.ToUpperInvariant(value[0]) + value.Substring(1);
        }

        public static string ToKebabCase(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;

            var kebabCase = System.Text.RegularExpressions.Regex.Replace(
                str,
                "(?<!^)([A-Z])",
                "-$1"
            );

            return kebabCase.ToLowerInvariant();
        }

        public static string ToLabel(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;

            var result = System.Text.RegularExpressions.Regex.Replace(
                str,
                "([A-Z])",
                " $1"
            ).Trim();

            return char.ToUpper(result[0]) + result.Substring(1);
        }
    }
}
