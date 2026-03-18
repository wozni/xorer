using System;
using System.Text;
using System.IO;
 
namespace xorer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 4 && args.Length != 6)
            {
                Console.WriteLine("Niepoprawne użycie!");
                Console.WriteLine("Wariant 1: xorer -inputKeyA A -inputKeyB B");
                Console.WriteLine("Wariant 2: xorer -inputKeyA A -inputKeyB B -outputFile F.txt");
                Console.WriteLine("Wariant 3: xorer -inputFileA A.txt -inputFileB B.txt -outputFile F.txt");
                return;
            }
 
            string kluczA = "";
            string kluczB = "";
 
            if (args[0] == "-inputKeyA" && args[2] == "-inputKeyB")
            {
                kluczA = args[1];
                kluczB = args[3];
            }
            else if (args[0] == "-inputFileA" && args[2] == "-inputFileB")
            {
                try
                {
                    kluczA = File.ReadAllText(args[1]);
                    kluczB = File.ReadAllText(args[3]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd odczytu pliku: {ex.Message}");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Niepoprawne argumenty! Użyj -inputKeyA/-inputKeyB lub -inputFileA/-inputFileB!");
                return;
            }
 
            string binarnaA = BityNaBinarne(Encoding.ASCII.GetBytes(kluczA));
            string binarnaB = BityNaBinarne(Encoding.ASCII.GetBytes(kluczB));
 
            int max = Math.Max(binarnaA.Length, binarnaB.Length);
            string[] wynik = new string[max];
 
            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine(args[i]);
            }
 
            XOR(max, wynik, binarnaA, binarnaB);
 
            string wynikString = string.Join("", wynik);
 
            if (args.Length == 4)
            {
                Console.WriteLine(wynikString);
            }
            else if (args.Length == 6)
            {
                string sciezka = args[5];
 
                try
                {
                    File.WriteAllText(sciezka, wynikString);
                    Console.WriteLine($"Wynik zapisano do pliku: {sciezka}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd zapisu do pliku: {ex.Message}");
                }
            }
        }
 
        static string BityNaBinarne(byte[] bity)
        {
            string wynik = "";
 
            foreach (byte b in bity)
            {
                wynik += Convert.ToString(b, 2).PadLeft(8, '0');
            }
 
            return wynik;
        }
 
        static void XOR(int max, string[] wynik, string binarnaA, string binarnaB)
        {
            binarnaA = binarnaA.PadLeft(max, '0');
            binarnaB = binarnaB.PadLeft(max, '0');
 
            for (int i = 0; i < max; i++)
            {
                wynik[i] = (binarnaA[i] == binarnaB[i]) ? "0" : "1";
            }
        }
    }
}
 