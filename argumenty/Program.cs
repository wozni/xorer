using System.CommandLine;
 
namespace xorer
{
    internal class Program
    {
        static int Main(string[] args)
        {
            var keyAOption = new Option<string?>("-inputKeyA");
            var keyAFileOption = new Option<FileInfo?>("-inputFileA");
            var keyBOption = new Option<string?>("-inputKeyB");
            var keyBFileOption = new Option<FileInfo?>("-inputFileB");
            var outputOption = new Option<FileInfo?>("-outputFile");
 
            var root = new RootCommand("XOR Tool for Base64 keys") {keyAOption, keyAFileOption,keyBOption, keyBFileOption,outputOption};
 
            root.SetAction(parseResult =>
            {
                var keyAArg = parseResult.GetValue(keyAOption);
                var keyAFile = parseResult.GetValue(keyAFileOption);
                var keyBArg = parseResult.GetValue(keyBOption);
                var keyBFile = parseResult.GetValue(keyBFileOption);
                var output = parseResult.GetValue(outputOption);
 
                try
                {
                    BuildKey(keyAArg, keyAFile, keyBArg, keyBFile, output);
                    return 0;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: {ex.Message}");
                    return 1;
                }
            });
 
            return root.Parse(args).Invoke();
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
                Console.WriteLine(result);
            }
        }
 
        static byte[] LoadKey(string? arg, FileInfo? file, int len)
        {
            if (arg == "-random")
                return GenerateRandomKeyBytes(len);
 
            if (arg != null)
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
 