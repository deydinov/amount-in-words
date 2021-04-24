using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amount_in_words.Helpers
{
    public static class StringHelper
    {
        public static string ToString(this StringBuilder sb, bool reverseOrder)
        {
            var arr = sb.ToString().Split(Environment.NewLine);
            if (reverseOrder)
            {
                arr = arr.Reverse().ToArray();
            }

            return string.Join(' ', arr).Trim();
        }

        public static void AddLine(this StringBuilder sb, string line)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                sb.AppendLine(line);
            }
        }
    }
}
