
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




}

