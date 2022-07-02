using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Hash.Md5;
using Xunit;

namespace Hash.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var hasher = new Md5Hasher();

            var fileBytes = File.ReadAllBytes("C:\\talha\\inbox\\hash.txt");
            var hash = hasher.GenerateHash(fileBytes);

            using System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();

            var hashBytes = md5.ComputeHash(fileBytes);


            Debug.WriteLine($"{hash[0]:X02}{hash[1]:X02}{hash[2]:X02}{hash[3]:X02}");

            foreach (var item in hashBytes)
            {
                Debug.Write($"{item:X02}");
            }

        }
    }
}