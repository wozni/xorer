namespace argumenty
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 4)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    Console.WriteLine(args[i]);
                }
            }
            XOR(args);
        }


        
        static void XOR(string[] args)
        {
            int keyA = int.Parse(args[1]);
            int keyB = int.Parse(args[3]);

            string binarnaA = Convert.ToString(keyA, 2);
            string binarnaB = Convert.ToString(keyB, 2);

            int max = Math.Max(binarnaA.Length, binarnaB.Length);
            binarnaA = binarnaA.PadLeft(max, '0');
            binarnaB = binarnaB.PadLeft(max, '0');

            string[] binarne1 = new string[max];
            string[] binarne2 = new string[max];

            for (int i = 0; i < max; i++)
            {
                binarne1[i] = binarnaA[i].ToString();
                binarne2[i] = binarnaB[i].ToString();
            }

            string[] wynik = new string[max];

            for (int i = 0; i < max; i++)
            {
                wynik[i] = (binarne1[i] == binarne2[i]) ? "0" : "1";
            }

            foreach (string bit in wynik)
            {
                Console.Write(bit);
            }
        }

    }
}
