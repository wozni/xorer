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
 
            byte[] kluczA;
            byte[] kluczB;
 
            if (args[0] == "-inputKeyA" && args[2] == "-inputKeyB")
            {
                try
                {
                    kluczA = Convert.FromBase64String(args[1]);
                    kluczB = Convert.FromBase64String(args[3]);
                }
                catch
                {
                    Console.WriteLine("Długość kluczy nie jest podzielna przez 4!");
                    return;
                }
            }
            else if (args[0] == "-inputFileA" && args[2] == "-inputFileB")
            {
                try
                {
                    try
                    {
                        kluczA = Convert.FromBase64String(File.ReadAllText(args[1]));
                        kluczB = Convert.FromBase64String(File.ReadAllText(args[3]));
                    }
                    catch
                    {
                        Console.WriteLine("Długość kluczy nie jest podzielna przez 4!");
                        return;
                    }
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
 
            if (kluczA.Length != kluczB.Length)
            {
                Console.WriteLine("Błąd: długości wejść nie są równe!");
                return;
            }
 
            byte[] xor = XOR(kluczA, kluczB);
 
            string wynikString = Convert.ToBase64String(xor);
 
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
 
        static byte[] XOR(byte[] a, byte[] b)
        {
            int len = a.Length;
            byte[] wynik = new byte[len];
 
            for (int i = 0; i < len; i++)
            {
                wynik[i] = (byte)(a[i] ^ b[i]);
            }
 
            return wynik;
        }
    }
}