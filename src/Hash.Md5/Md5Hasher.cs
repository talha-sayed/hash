
using System;
using System.Buffers.Binary;
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

    private uint[] T;
    private uint[] _x;

    public Md5Hasher()
    {
        var arr = new uint[64];

        for (int ix = 0; ix < arr.Length; ix++)
        {
            arr[ix] = (uint)Math.Abs(4294967296 * Math.Sin(ix + 1));
        }

        T = arr;
    }

    public byte[] GenerateHash(byte[] message)
    {
        // length in bits
        var length = (uint)message.Length * 8;

        // Padding the message
        var paddingChars = 512 - (length % 512);
        var paddingBytes = paddingChars / 8;

        var messageList = message.ToList();

        messageList.Add(0b10000000);
        paddingBytes--;

        while (paddingBytes > 0)
        {
            messageList.Add(0b00000000);
            paddingBytes--;
        }

        var lengthArrayBits = messageList.Count*8;
        var lengthArrayBytes = lengthArrayBits/8;

        messageList[lengthArrayBytes - 4] = (byte)(length >> 0);
        messageList[lengthArrayBytes - 3] = (byte)(length >> 8);
        messageList[lengthArrayBytes - 2] = (byte)(length >> 16);
        messageList[lengthArrayBytes - 1] = (byte)(length >> 24);

        // Initialize
        uint A = 0x67452301;
        uint B = 0xefcdab89;
        uint C = 0x98badcfe;
        uint D = 0x10325476;

        var numBlocks = (messageList.Count * 8) / 512;
        // main loop
        // process 16 word blocks

        for (var i = 0; i < numBlocks; i++)
        {
            var AA = A;
            var BB = B;
            var CC = C;
            var DD = D;

            _x = new uint[16];

            for (var j = 0; j < 64; j=j+4)
            {
                _x[j/4] = BitConverter.ToUInt32(messageList.Skip(i * 16 + j).Take(4).ToArray());
            }

#pragma warning disable IDE0055
            // @formatter:off

            // Round 1
            //    [ABCD         0  7   1]        [DABC         1  12   2]        [CDAB         2  17   3]        [BCDA         3  22   4]
            //    [ABCD         4  7   5]        [DABC         5  12   6]        [CDAB         6  17   7]        [BCDA         7  22   8]
            //    [ABCD         8  7   9]        [DABC         9  12  10]        [CDAB        10  17  11]        [BCDA        11  22  12]
            //    [ABCD        12  7  13]        [DABC        13  12  14]        [CDAB        14  17  15]        [BCDA        15  22  16]
            R1(ref A, B, C, D,  0, 7,  1); R1(ref D, A, B, C,  1, 12,  2); R1(ref C, D, A, B,  2, 17,  3); R1(ref B, C, D, A,  3, 22,  4);
            R1(ref A, B, C, D,  4, 7,  5); R1(ref D, A, B, C,  5, 12,  6); R1(ref C, D, A, B,  6, 17,  7); R1(ref B, C, D, A,  7, 22,  8);
            R1(ref A, B, C, D,  8, 7,  9); R1(ref D, A, B, C,  9, 12, 10); R1(ref C, D, A, B, 10, 17, 11); R1(ref B, C, D, A, 11, 22, 12);
            R1(ref A, B, C, D, 12, 7, 13); R1(ref D, A, B, C, 13, 12, 14); R1(ref C, D, A, B, 14, 17, 15); R1(ref B, C, D, A, 15, 22, 16);

            // Round 2
            //    [ABCD         1  5  17]        [DABC         6  9  18]        [CDAB       11   14  19]        [BCDA         0  20  20]
            //    [ABCD         5  5  21]        [DABC        10  9  22]        [CDAB       15   14  23]        [BCDA         4  20  24]
            //    [ABCD         9  5  25]        [DABC        14  9  26]        [CDAB        3   14  27]        [BCDA         8  20  28]
            //    [ABCD        13  5  29]        [DABC         2  9  30]        [CDAB        7   14  31]        [BCDA        12  20  32]
            R2(ref A, B, C, D,  1, 5, 17); R2(ref D, A, B, C,  6, 9, 18); R2(ref C, D, A, B, 11, 14, 19); R2(ref B, C, D, A,  0, 20, 20);
            R2(ref A, B, C, D,  5, 5, 21); R2(ref D, A, B, C, 10, 9, 22); R2(ref C, D, A, B, 15, 14, 23); R2(ref B, C, D, A,  4, 20, 24);
            R2(ref A, B, C, D,  9, 5, 25); R2(ref D, A, B, C, 14, 9, 26); R2(ref C, D, A, B,  3, 14, 27); R2(ref B, C, D, A,  8, 20, 28);
            R2(ref A, B, C, D, 13, 5, 29); R2(ref D, A, B, C,  2, 9, 30); R2(ref C, D, A, B,  7, 14, 31); R2(ref B, C, D, A, 12, 20, 32);

            // Round 3
            //    [ABCD         5  4  33]        [DABC         8  11  34]        [CDAB        11  16  35]        [BCDA        14  23  36]
            //    [ABCD         1  4  37]        [DABC         4  11  38]        [CDAB         7  16  39]        [BCDA        10  23  40]
            //    [ABCD        13  4  41]        [DABC         0  11  42]        [CDAB         3  16  43]        [BCDA         6  23  44]
            //    [ABCD         9  4  45]        [DABC        12  11  46]        [CDAB        15  16  47]        [BCDA         2  23  48]
            R3(ref A, B, C, D,  5, 4, 33); R3(ref D, A, B, C,  8, 11, 34); R3(ref C, D, A, B, 11, 16, 35); R3(ref B, C, D, A, 14, 23, 36);
            R3(ref A, B, C, D,  1, 4, 37); R3(ref D, A, B, C,  4, 11, 38); R3(ref C, D, A, B,  7, 16, 39); R3(ref B, C, D, A, 10, 23, 40);
            R3(ref A, B, C, D, 13, 4, 41); R3(ref D, A, B, C,  0, 11, 42); R3(ref C, D, A, B,  3, 16, 43); R3(ref B, C, D, A,  6, 23, 44);
            R3(ref A, B, C, D,  9, 4, 45); R3(ref D, A, B, C, 12, 11, 46); R3(ref C, D, A, B, 15, 16, 47); R3(ref B, C, D, A,  2, 23, 48);

            // Round 4
            //    [ABCD         0  6  49]        [DABC         7  10  50]        [CDAB        14  15  51]        [BCDA         5  21  52]
            //    [ABCD        12  6  53]        [DABC         3  10  54]        [CDAB        10  15  55]        [BCDA         1  21  56]
            //    [ABCD         8  6  57]        [DABC        15  10  58]        [CDAB         6  15  59]        [BCDA        13  21  60]
            //    [ABCD         4  6  61]        [DABC        11  10  62]        [CDAB         2  15  63]        [BCDA         9  21  64]
            R4(ref A, B, C, D,  0, 6, 49); R4(ref D, A, B, C,  7, 10, 50); R4(ref C, D, A, B, 14, 15, 51); R4(ref B, C, D, A,  5, 21, 52);
            R4(ref A, B, C, D, 12, 6, 53); R4(ref D, A, B, C,  3, 10, 54); R4(ref C, D, A, B, 10, 15, 55); R4(ref B, C, D, A,  1, 21, 56);
            R4(ref A, B, C, D,  8, 6, 57); R4(ref D, A, B, C, 15, 10, 58); R4(ref C, D, A, B,  6, 15, 59); R4(ref B, C, D, A, 13, 21, 60);
            R4(ref A, B, C, D,  4, 6, 61); R4(ref D, A, B, C, 11, 10, 62); R4(ref C, D, A, B,  2, 15, 63); R4(ref B, C, D, A,  9, 21, 64);

            // @formatter:on
#pragma warning restore IDE0055


            A += AA;
            B += BB;
            C += CC;
            D += DD;

        }

        var Abytes = BitConverter.GetBytes(ReverseBytes(A));
        var Bbytes = BitConverter.GetBytes(ReverseBytes(B));
        var Cbytes = BitConverter.GetBytes(ReverseBytes(C));
        var Dbytes = BitConverter.GetBytes(ReverseBytes(D));

        return Abytes.Concat(Bbytes).Concat(Cbytes).Concat(Dbytes).ToArray();
    }


    private void R1(ref uint a, uint b, uint c, uint d, byte k, byte s, byte i)
    {
        // a = b + ((a + F(b,c,d) + X[k] + T[i]) <<< s)
        var ax = a;
        a = b + BitOperations.RotateLeft(ax + F(b, c, d) + _x[k] + T[i-1], s);
    }

    private void R2(ref uint a, uint b, uint c, uint d, byte k, byte s, byte i)
    {
        // a = b + ((a + G(b,c,d) + X[k] + T[i]) <<< s)
        var ax = a;
        a = b + BitOperations.RotateLeft(ax + G(b, c, d) + _x[k] + T[i-1], s);
    }

    private void R3(ref uint a, uint b, uint c, uint d, byte k, byte s, byte i)
    {
        // a = b + ((a + H(b,c,d) + X[k] + T[i]) <<< s)
        var ax = a;
        a = b + BitOperations.RotateLeft(ax + H(b, c, d) + _x[k] + T[i-1], s);
    }

    private void R4(ref uint a, uint b, uint c, uint d, byte k, byte s, byte i)
    {
        // a = b + ((a + I(b,c,d) + X[k] + T[i]) <<< s)
        var ax = a;
        a = b + BitOperations.RotateLeft(ax + I(b, c, d) + _x[k] + T[i-1], s);
    }

    public static UInt32 ReverseBytes(UInt32 value)
    {
        return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
               (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
    }
}
