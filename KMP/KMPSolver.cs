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

        private static IList<int> FindPatternMatches(string text, string pattern, List<int> prefixSuffix)
        {
            var matches = new List<int>();

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

        public static IList<int> FindAllMatches(string text, string pattern)
        {
            var prefixSuffix = FindPatternPrefixSuffix(pattern);
            return FindPatternMatches(text, pattern, prefixSuffix);
        }

        public static IList<int> FastAllMatches(string text, string pattern)
        {
            var prefixSuffix = new List<int>(pattern.Length + 1);
            prefixSuffix.Add(0);
            prefixSuffix.Add(0);

            {
                int j = 0;
                for (int i = 1; i < pattern.Length; ++i)
                {
                    if (j < pattern.Length && pattern[i] == pattern[j])
                    {
                        j++;
                    }
                    else
                    {
                        j = prefixSuffix[j];

                        while (j > 0 && pattern[i] != pattern[j])
                        {
                            j = prefixSuffix[j];
                        }

                        if (pattern[i] == pattern[j])
                        {
                            j++;
                        }
                    }

                    prefixSuffix.Add(j);
                }
            }

            var matches = new List<int>();

            {
                int j = 0;
                for (int i = 0; i < text.Length; ++i)
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

                    if (j == pattern.Length)
                    {
                        matches.Add(i - pattern.Length + 1);
                    }
                }
            }

            return matches;
        }
    }
}