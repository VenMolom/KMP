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

            var inputDir = args[0];
            var outputFilename = args[1];

            var files = Directory.EnumerateFiles(inputDir, "10*", SearchOption.TopDirectoryOnly);
            foreach (var fileName in files)
            {
                var shortName = Path.GetFileNameWithoutExtension(fileName);
                try
                {
                    using (var fr = File.OpenText(fileName))
                    {
                        var pattern = fr.ReadLine();
                        var text = fr.ReadLine();

                        KMPSolver.FindAllMatches(text, pattern);
                        BrutePatternSearching.FindAllMatches(text, pattern);
                        
                        var kmpTime = Time(() => KMPSolver.FindAllMatches(text, pattern));
                        var brutalTime = Time(() => BrutePatternSearching.FindAllMatches(text, pattern));
                        Console.WriteLine($"{shortName}: KMP: {kmpTime} ms, Brutal: {brutalTime} ms");
                    }
                }
                catch
                {
                    Console.WriteLine($"Nie można otworzyć pliku {shortName}");
                    return;
                }
            }
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