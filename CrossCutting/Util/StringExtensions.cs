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
    }
}
