
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local

namespace Hash.Md5;

public class Md5Hasher
{
    private uint F(uint X, uint Y, uint Z) => (X & Y) | (~X & Z);
    
    private uint G(uint X, uint Y, uint Z) => (X & Z) | (Y & ~Z);
    
    private uint H(uint X, uint Y, uint Z) => X ^ Y ^ Z;
    
    private uint I(uint X, uint Y, uint Z) => Y ^ (X | ~Z);

    public Md5Hasher()
    {
    }

    public byte[] GenerateHash(byte[] message)
    {
        // length in bits
        var length = message.Length * 8;

        // Padding the message
        var paddingChars = length % 512 == 0 ? 512 : length % 512;
        var paddingBytes = paddingChars / 8;

        var messageList = message.ToList();

        messageList.Add(0b10000000);
        paddingBytes--;

        while (paddingBytes > 0)
        {
            messageList.Add(0b00000000);
            paddingBytes--;
        }
        
        // Initialize
        uint A = 0x01234567;
        uint B = 0x89abcdef;
        uint C = 0xfedcba98;
        uint D = 0x76543210;

        var numBlocks = (messageList.Count * 8) / 512;
        // main loop
        // process 16 word blocks

        for (var i =0; i < numBlocks; i++)
        {
            var AA = A;
            var BB = B;
            var CC = C;
            var DD = D;


            // Round 1

            //A = Round1();




        }

        return message[..16];
    }


    private uint Round1(uint n1, uint n2, uint n3, byte k , byte s, byte i)
    {
        var x = BitOperations.RotateLeft(n1, s);

        //var result = b + ((a + F(b, c, d) + X[k] + T[i]) <<< s)

        return 0;
    }
}
