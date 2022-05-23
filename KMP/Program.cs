using System;
using System.IO;
using System.Runtime.InteropServices;

namespace KMP
{
    internal class Program
    {
        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.Winapi)]
        private static extern void GetSystemTimePreciseAsFileTime(out long filetime);
        
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

            var kmpTime = Time(() => KMPSolver.FindAllMatches(text, pattern));
            var brutalTime = Time(() => BrutePatternSearching.FindAllMatches(text, pattern));
            Console.Write($"KMP: {kmpTime} ms, Brutal: {brutalTime} ms");
            // try
            // {
            //     using (var sw = File.CreateText(outputFilename))
            //     {
            //         foreach (var match in matches)
            //         {
            //             sw.WriteLine(match);
            //         }
            //     }
            // }
            // catch
            // {
            //     Console.WriteLine($"Nie można utworzyć pliku {outputFilename}");
            //     return;
            // }
        }

        public static double Time(Action action)
        {
            GetSystemTimePreciseAsFileTime(out var start);
            action.Invoke();
            GetSystemTimePreciseAsFileTime(out var end);
            return (DateTime.FromFileTimeUtc(end) - DateTime.FromFileTimeUtc(start)).TotalMilliseconds;
        }
    }
}