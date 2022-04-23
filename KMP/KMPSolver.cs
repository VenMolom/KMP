using System;
using System.Collections.Generic;

namespace KMP
{
    public static class KMPSolver
    {
        private static List<int> FindPatternPrefixSuffix(string pattern)
        {
            var prefixSuffix = new List<int>(pattern.Length + 1);
            prefixSuffix.Add(0);
            prefixSuffix.Add(0);

            TextPatternLoop(pattern, pattern, 1, prefixSuffix, (int i, int j) => { prefixSuffix.Add(j); });

            return prefixSuffix;
        }

        private static ISet<int> FindPatternMatches(string text, string pattern, List<int> prefixSuffix)
        {
            var matches = new HashSet<int>();

            TextPatternLoop(text, pattern, 0, prefixSuffix, (int i, int j) =>
            {
                if (j == pattern.Length)
                {
                    matches.Add(i - pattern.Length + 1);
                }
            });

            return matches;
        }

        private static void TextPatternLoop(string text, string pattern, int start, List<int> prefixSuffix,
                                               Action<int, int> setInCollection)
        {
            int j = 0;
            for (int i = start; i < text.Length; ++i)
            {
                if (j < pattern.Length && text[i] == pattern[j])
                {
                    j++;
                }
                else
                {
                    j = prefixSuffix[j];

                    while (j > 0 && text[i] != pattern[j])
                    {
                        j = prefixSuffix[j];
                    }

                    if (text[i] == pattern[j])
                    {
                        j++;
                    }
                }

                setInCollection(i, j);
            }
        }

        public static ISet<int> FindAllMatches(string text, string pattern)
        {
            var prefixSuffix = FindPatternPrefixSuffix(pattern);
            return FindPatternMatches(text, pattern, prefixSuffix);
        }
    }
}