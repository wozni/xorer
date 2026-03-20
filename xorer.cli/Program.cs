using System;
using System.IO;

namespace xorer
{
    internal class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("----- XOR Tool for Base64 keys -----");

            try
            {
                Console.Write("Key A (Base64, file path, or -random): ");
                string? keyAArg = Console.ReadLine();

                Console.Write("Key B (Base64, file path, or -random): ");
                string? keyBArg = Console.ReadLine();

                Console.Write("Output file path (ENTER to show in console): ");
                string? outputPath = Console.ReadLine();
                FileInfo? outputFile = string.IsNullOrWhiteSpace(outputPath) ? null : new FileInfo(outputPath);

                BuildKey(keyAArg, keyBArg, outputFile);
                Console.ReadKey();
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return 1;
            }
        }

        static void BuildKey(string? keyAArg, string? keyBArg, FileInfo? outputFile)
        {
            int? randomLenA = ParseRandomLength(keyAArg);
            int? randomLenB = ParseRandomLength(keyBArg);

            byte[] keyA = LoadKey(keyAArg, randomLenA ?? 64);
            byte[] keyB = LoadKey(keyBArg, randomLenB ?? keyA.Length);

            if (keyA.Length != keyB.Length)
                throw new Exception("Input lengths are not equal!");

            byte[] xor = XOR(keyA, keyB);
            string result = Convert.ToBase64String(xor);

            if (outputFile != null)
            {
                File.WriteAllText(outputFile.FullName, result);
                Console.WriteLine($"\nSaved to {outputFile.FullName}");
            }
            else
            {
                Console.WriteLine("\nResult: ");
                Console.WriteLine(result);
            }
        }

        static byte[] LoadKey(string? arg, int len)
        {
            if (string.IsNullOrWhiteSpace(arg))
                throw new Exception("Missing key input!");

            if (arg.StartsWith("-random"))
                return GenerateRandomKeyBytes(len);

            if (File.Exists(arg))
            {
                string base64 = File.ReadAllText(arg);
                return Convert.FromBase64String(base64);
            }

            try
            {
                return Convert.FromBase64String(arg);
            }
            catch
            {
                throw new Exception("Input is neither Base64 or a file path!");
            }
        }

        static byte[] GenerateRandomKeyBytes(int len)
        {
            var bytes = new byte[len];
            new Random().NextBytes(bytes);
            Console.WriteLine($"\nGenerated random key: {Convert.ToBase64String(bytes)}");
            return bytes;
        }

        static int? ParseRandomLength(string? arg)
        {
            if (arg == null) return null;
            if (!arg.StartsWith("-random")) return null;

            var parts = arg.Split(new[] { ':', '=' }, 2);
            if (parts.Length == 2 && int.TryParse(parts[1], out int len))
                return len;

            return null;
        }

        static byte[] XOR(byte[] a, byte[] b)
        {
            var result = new byte[a.Length];
            for (int i = 0; i < a.Length; i++)
                result[i] = (byte)(a[i] ^ b[i]);
            return result;
        }
    }
}