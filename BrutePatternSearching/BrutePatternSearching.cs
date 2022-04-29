using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrutePatternSearching
{
    static class BrutePatternSearching
    {
        public static ISet<int> FindAllMatches(string text, string pattern)
        {
            var result = new HashSet<int>();

            if (pattern.Length > text.Length)
            {
                return result;
            }

            for (int i = 0; i < text.Length - pattern.Length; i++)
            {
                bool patternFound = true;

                for (int j = 0; j < pattern.Length; j++)
                {
                    if (text[i + j] != pattern[j])
                    {
                        patternFound = false;
                        break;
                    }
                }

                if (patternFound)
                {
                    result.Add(i);
                }
            }

            return result;
        }
    }
}
