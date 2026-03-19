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

            bool hasKeyA = dict.ContainsKey("-inputKeyA");
            bool hasKeyB = dict.ContainsKey("-inputKeyB");
            bool hasFileA = dict.ContainsKey("-inputFileA");
            bool hasFileB = dict.ContainsKey("-inputFileB");

            if (!((hasKeyA && hasKeyB) || (hasFileA && hasFileB)))
            {
                Console.WriteLine("Invalid usage!");
                Console.WriteLine("Variant 1: dotnet run -inputKeyA A -inputKeyB B");
                Console.WriteLine("Variant 2: dotnet run -inputKeyA A -inputKeyB B -outputFile F.txt");
                Console.WriteLine("Variant 3: dotnet run -inputFileA A.txt -inputFileB B.txt -outputFile F.txt");
                return;
            }
            
            byte[] keyA = Array.Empty<byte>();
            byte[] keyB = Array.Empty<byte>();
            
            if (hasKeyA && hasKeyB)
            {
                string argA = dict["-inputKeyA"];
                string argB = dict["-inputKeyB"];

                bool aRandom = argA == "-random";
                bool bRandom = argB == "-random";

                if (!aRandom)
                    keyA = Convert.FromBase64String(argA);

                if (!bRandom)
                    keyB = Convert.FromBase64String(argB);

                if (aRandom && bRandom)
                {
                    keyA = GenerateRandomKeyBytes(64);
                    keyB = GenerateRandomKeyBytes(64);
                }
                else if (aRandom)
                {
                    keyA = GenerateRandomKeyBytes(keyB.Length);
                }
                else if (bRandom)
                {
                    keyB = GenerateRandomKeyBytes(keyA.Length);
                }
            }
            else
            {
                try
                {
                    keyA = Convert.FromBase64String(File.ReadAllText(dict["-inputFileA"]));
                    keyB = Convert.FromBase64String(File.ReadAllText(dict["-inputFileB"]));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"File read error: {ex.Message}");
                    return;
                }
            }

            if (keyA.Length != keyB.Length)
            {
                Console.WriteLine("Error: Input lengths are not equal!");
                return;
            }

            byte[] xor = XOR(keyA, keyB);
            string resultString = Convert.ToBase64String(xor);

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

        static Dictionary<string, string> ParseArgs(string[] args)
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

        static byte[] GenerateRandomKeyBytes(int byteLength)
        {
            byte[] randomBytes = new byte[byteLength];
            Random random = new Random();
            random.NextBytes(randomBytes);
            Console.WriteLine($"Generated key used for XOR: {Convert.ToBase64String(randomBytes)}");
            return randomBytes;
        }

        static byte[] XOR(byte[] a, byte[] b)
        {
            int len = a.Length;
            byte[] result = new byte[len];

            for (int i = 0; i < len; i++)
                result[i] = (byte)(a[i] ^ b[i]);

            return result;
        }
    }
}
