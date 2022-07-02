
using System.Security.Cryptography;
using Hash.Md5;

public class Program
{
    public static void Main(string[] args)
    {

        for (var i = 0u; i < 100; i++)
        {
            PrintHex(i);
        }

        uint a = 0x78_56_34_12;

        foreach (byte b in BitConverter.GetBytes(a))
        {
            Console.Write($"{b:X02}");
        }



        var hasher = new Md5Hasher();

        var fileName = "C:\\talha\\inbox\\sample.jpg";

        var builtinHash = BuiltinHasher(fileName);

        var input = File.ReadAllBytes(fileName);
        
        var myHash = hasher.GenerateHash(input);

        Console.WriteLine();
        Console.WriteLine();
        Console.Write("Built-in hash: ");
        DisplayHash(builtinHash);

        Console.WriteLine();
        Console.WriteLine();
        Console.Write("      My hash: ");
        DisplayHash(myHash);
        Console.WriteLine();
        Console.WriteLine();
    }

    public static void PrintHex(uint n)
    {
        foreach (byte b in BitConverter.GetBytes(n))
        {
            Console.Write($"{b:X02}");
        }

        Console.Write($"  {n:X08}");
        Console.WriteLine();
    }

    public static byte[] BuiltinHasher(string fileName)
    {
        using var md5 = MD5.Create();
        using var stream = File.OpenRead(fileName);
        return md5.ComputeHash(stream);
    }

    public static void DisplayHash(byte[] bytes)
    {
        foreach (byte b in bytes)
        {
            Console.Write($"{b:X2} ");
        }
    }

    public static void ByteArrBinary(byte[] bytes)
    {
        int lineBr = 1;

        for (int i = 0; i < bytes.Length; i++, lineBr++)
        {
            Console.Write($"{Convert.ToString(bytes[i], 2).PadLeft(8, '0')} ");

            if (lineBr == 4)
            {
                Console.WriteLine();
                lineBr = 0;
            }
        }


    }


}

