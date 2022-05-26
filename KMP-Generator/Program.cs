using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace KMP_Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 6)
            {
                Console.WriteLine($"Wywołanie musi być postaci: " +
                                  $"{Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0])}\n" +
                                  $"[długość wzorca] > 0\n" +
                                  $"[liczba różnych liter wzorca] > 0 && < 36\n" +
                                  $"[max. liczba powtórzeń liter we wzorcu] >= 0\n" +
                                  $"[długość tekstu] > 0\n" +
                                  $"[liczba losowych wstawień wzorca] >= 0\n" +
                                  $"[plik wyjściowy]\n");
                return;
            }

            var patternLength = int.Parse(args[0]);
            var patternCharsetCount = Math.Max(1, Math.Min('Z' - 'A' + 1, int.Parse(args[1])));
            var patternMaxCharRepeatCount = Math.Max(0, int.Parse(args[2]));
            var textLength = int.Parse(args[3]);
            var patternInsertionCount = int.Parse(args[4]);
            var outputFilename = args[5];

            char[] pattern = new char[patternLength];
            var textBuilder = new StringBuilder(textLength);

            Random rng = new Random();
            for (int i = 0; i < patternLength;)
            {
                char c = (char)rng.Next('A', 'A' + patternCharsetCount);
                int repeatCount = 1 + rng.Next(0, patternMaxCharRepeatCount);
                for (int j = 0; j < repeatCount && i < patternLength; j++)
                {
                    pattern[i] = c;
                    i++;
                }
            }

            var patternString = string.Join("", pattern);
            var patternStringWithoutLast = patternString.Substring(0, patternString.Length - 1);
            var patternLast = patternString[patternString.Length - 1];
            
            for (int i = 0; i < textLength / patternLength; i++)
            {
                textBuilder.Append(patternStringWithoutLast);
                textBuilder.Append((char)(patternLast + (rng.Next(0, 2) * 2 - 1)));
            }

            var text = textBuilder.ToString().ToCharArray();
            
            for (int i = 0; i < patternInsertionCount; i++)
            {
                var insertionIdx = rng.Next(0, text.Length);
                for (int j = 0; j < patternLength && j + insertionIdx < textLength; j++)
                {
                    text[j + insertionIdx] = pattern[j];
                }
            }

            try
            {
                using (var sw = File.CreateText(outputFilename))
                {
                    sw.WriteLine(pattern);
                    sw.WriteLine(text);
                }
            }
            catch
            {
                Console.WriteLine($"Nie można utworzyć pliku {outputFilename}");
                return;
            }
        }
    }
}
