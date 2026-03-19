using System;
using System.IO;

namespace xorer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 4 && args.Length != 6)
            {
                Console.WriteLine("Invalid usage!");
                Console.WriteLine("Variant 1: dotnet run -inputKeyA A -inputKeyB B");
                Console.WriteLine("Variant 2: dotnet run -inputKeyA A -inputKeyB B -outputFile F.txt");
                Console.WriteLine("Variant 3: dotnet run -inputFileA A.txt -inputFileB B.txt -outputFile F.txt");
                return;
            }

            byte[] keyA = new byte[args[1].Length];
            byte[] keyB = new byte[args[3].Length];

            if (args[0] == "-inputKeyA" && args[2] == "-inputKeyB")
            {
                string argA = args[1];
                string argB = args[3];

                bool aRandom = argA == "-random";
                bool bRandom = argB == "-random";

                if (!aRandom)
                {
                    keyA = Convert.FromBase64String(argA);
                }

                if (!bRandom)
                {
                    keyB = Convert.FromBase64String(argB);
                }

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
            else if (args[0] == "-inputFileA" && args[2] == "-inputFileB")
            {
                try
                {
                    keyA = Convert.FromBase64String(File.ReadAllText(args[1]));
                    keyB = Convert.FromBase64String(File.ReadAllText(args[3]));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"File read error: {ex.Message}");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Invalid arguments! Use -inputKeyA/-inputKeyB or -inputFileA/-inputFileB!");
                return;
            }

            if (keyA.Length != keyB.Length)
            {
                Console.WriteLine("Error: Input lengths are not equal!");
                return;
            }

            byte[] xor = XOR(keyA, keyB);
            string resultString = Convert.ToBase64String(xor);

            if (args.Length == 4)
            {
                Console.WriteLine(resultString);
            }
            else if (args.Length == 6)
            {
                string path = args[5];

                try
                {
                    File.WriteAllText(path, resultString);
                    Console.WriteLine($"Result saved to file: {path}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"File write error: {ex.Message}");
                }
            }
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
