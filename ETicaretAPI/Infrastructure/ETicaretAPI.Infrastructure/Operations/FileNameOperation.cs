using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ETicaretAPI.Infrastructure.Operations
{
    public static class FileNameOperation
    {
      
        public static string CharacterRegulatory(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            var replacements = new (string, string)[]
            {
                ("ö", "o"), ("Ö", "o"),
                ("ü", "u"), ("Ü", "u"),
                ("ı", "i"), ("I", "i"),
                ("ğ", "g"), ("Ğ", "g"),
                ("ş", "s"), ("Ş", "s"),
                ("ç", "c"), ("Ç", "c"),
                ("æ", "ae"), ("ß", "ss"),
                ("â", "a"), ("Â", "a"),
                ("î", "i"), ("Î", "i")
            };
            foreach (var (key, val) in replacements)
            {
                name = name.Replace(key, val);
            }

            name = name.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in name)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            name = sb.ToString();
            name = Regex.Replace(name, @"[^a-zA-Z0-9]+", "-");
            name = name.Trim('-');
            name = name.ToLowerInvariant();

            return name;
        }
    }
}
