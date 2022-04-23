using System;
using System.IO;

namespace KMP
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine($"Wywołanie musi być postaci: " +
                                  $"{Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0])} " +
                                  $"<plik wejściowy> <plik wyjściowy>");
                return;
            }

            var inputFilename = args[0];
            var outputFilename = args[1];

            string text, pattern;
            try
            {
                using (var fr = File.OpenText(inputFilename))
                {
                    pattern = fr.ReadLine();
                    text = fr.ReadLine();
                }
            }
            catch
            {
                Console.WriteLine($"Nie można otworzyć pliku {inputFilename}");
                return;
            }

            var matches = KMPSolver.FindAllMatches(text, pattern);
            
            try
            {
                using (var sw = File.CreateText(outputFilename))
                {
                    foreach (var match in matches)
                    {
                        sw.WriteLine(match);
                    }
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