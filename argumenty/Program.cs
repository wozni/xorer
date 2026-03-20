namespace xorer
{
    internal class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("----- XOR Tool for Base64 keys -----");

            try
            {
                Console.Write("Key A (Base64 or -random): ");
                string? keyAArg = Console.ReadLine();

                Console.Write("Key A file path (ENTER to skip): ");
                string? keyAFilePath = Console.ReadLine();
                FileInfo? keyAFile = string.IsNullOrWhiteSpace(keyAFilePath) ? null : new FileInfo(keyAFilePath);

                Console.Write("Key B (Base64 or -random): ");
                string? keyBArg = Console.ReadLine();

                Console.Write("Key B file path (ENTER to skip): ");
                string? keyBFilePath = Console.ReadLine();
                FileInfo? keyBFile = string.IsNullOrWhiteSpace(keyBFilePath) ? null : new FileInfo(keyBFilePath);

                Console.Write("Output file path (ENTER to print to console): ");
                string? outputPath = Console.ReadLine();
                FileInfo? outputFile = string.IsNullOrWhiteSpace(outputPath) ? null : new FileInfo(outputPath);

                BuildKey(keyAArg, keyAFile, keyBArg, keyBFile, outputFile);
                Console.ReadKey();
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return 1;
            }
        }

        static void BuildKey(string? keyAArg, FileInfo? keyAFile, string? keyBArg, FileInfo? keyBFile, FileInfo? outputFile)
        {
            byte[] keyA = LoadKey(keyAArg, keyAFile, 64);
            byte[] keyB = LoadKey(keyBArg, keyBFile, keyA.Length);

            if (keyA.Length != keyB.Length)
                throw new Exception("Input lengths are not equal!");

            byte[] xor = XOR(keyA, keyB);
            string result = Convert.ToBase64String(xor);

            if (outputFile != null)
            {
                File.WriteAllText(outputFile.FullName, result);
                Console.WriteLine($"Saved to {outputFile.FullName}");
            }
            else
            {
                Console.WriteLine("Result:");
                Console.WriteLine(result);
            }
        }

        static byte[] LoadKey(string? arg, FileInfo? file, int len)
        {
            if (arg == "-random")
                return GenerateRandomKeyBytes(len);

            if (!string.IsNullOrWhiteSpace(arg))
                return Convert.FromBase64String(arg);

            if (file != null)
                return Convert.FromBase64String(File.ReadAllText(file.FullName));

            throw new Exception("Missing key input!");
        }

        static byte[] GenerateRandomKeyBytes(int len)
        {
            var bytes = new byte[len];
            new Random().NextBytes(bytes);
            Console.WriteLine($"Generated random key: {Convert.ToBase64String(bytes)}");
            return bytes;
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
