using System;
using System.Collections.Generic;
using System.IO;

namespace xorer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var dict = ParseArgs(args);

            byte[] keyA;
            byte[] keyB;

            if (dict.ContainsKey("-inputKeyA")) // Sprawdza czy inputy istnieją i generuje lub pobiera w zależności od tego czy jest -random
            {
                string argA = dict["-inputKeyA"];
                if (argA == "-random")
                {
                    keyA = GenerateRandomKeyBytes(64);
                }
                else
                {
                    keyA = Convert.FromBase64String(argA);
                }
            }
            else if (dict.ContainsKey("-inputFileA"))
            {
                try
                {
                    keyA = Convert.FromBase64String(File.ReadAllText(dict["-inputFileA"]));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"File A read error: {ex.Message}");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Missing A: provide -inputKeyA or -inputFileA");
                return;
            }

            if (dict.ContainsKey("-inputKeyB"))
            {
                string argB = dict["-inputKeyB"];
                if (argB == "-random")
                {
                    keyB = GenerateRandomKeyBytes(keyA.Length);
                }
                else
                {
                    keyB = Convert.FromBase64String(argB);
                }
            }
            else if (dict.ContainsKey("-inputFileB"))
            {
                try
                {
                    keyB = Convert.FromBase64String(File.ReadAllText(dict["-inputFileB"]));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"File B read error: {ex.Message}");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Missing B: provide -inputKeyB or -inputFileB");
                return;
            }

            if (keyA.Length != keyB.Length)
            {
                Console.WriteLine("Error: Input lengths are not equal!");
                return;
            }

            byte[] xor = XOR(keyA, keyB); // Wykonuje funkcję XOR z wcześniej podanymi/wygenerowanymi kluczami
            string resultString = Convert.ToBase64String(xor);

            // Sprawdza czy jest -outputFile i zapisuje rezultat w podanym pliku
            if (dict.TryGetValue("-outputFile", out string? outputPath) && outputPath is not null)
            {
                try
                {
                    File.WriteAllText(outputPath, resultString);
                    Console.WriteLine($"Result saved to file: {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"File write error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine(resultString);
            }
        }

        static Dictionary<string, string> ParseArgs(string[] args) // Tworzy słownik z argumentami 
        {
            var dict = new Dictionary<string, string>();

            for (int i = 0; i < args.Length - 1; i++)
            {
                if (args[i].StartsWith("-"))
                {
                    dict[args[i]] = args[i + 1];
                }
            }

            return dict;
        }

        static byte[] GenerateRandomKeyBytes(int byteLength) // Generuje losowy pasujący klucz i zwraca rezultat
        {
            byte[] randomBytes = new byte[byteLength];
            Random random = new Random();
            random.NextBytes(randomBytes);
            Console.WriteLine($"Generated key used for XOR: {Convert.ToBase64String(randomBytes)}");
            return randomBytes;
        }

        static byte[] XOR(byte[] a, byte[] b) // XOR-uje podane klucze i zwraca rezultat
        {
            int len = a.Length;
            byte[] result = new byte[len];

            for (int i = 0; i < len; i++)
                result[i] = (byte)(a[i] ^ b[i]);

            return result;
        }
    }
}
