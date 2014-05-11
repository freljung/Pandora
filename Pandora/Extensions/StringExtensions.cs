using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandora
{
    public static class StringExtensions
    {
        public static bool EndsWith(this string source, string end)
        {
            if (source == null || end == null || source.Length < end.Length)
                return false;

            return source.Substring(source.Length - end.Length).Equals(end, StringComparison.OrdinalIgnoreCase);
        }
    }
}
