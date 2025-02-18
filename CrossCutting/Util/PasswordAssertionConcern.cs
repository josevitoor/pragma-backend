using System;
using System.Security.Cryptography;
using System.Text;

namespace CrossCutting.Util
{
    public static class PasswordAssertionConcern
    {
        static readonly char[] chars = "abcdefghijklmnopqrstuvw0987654321".ToCharArray();
#pragma warning disable SYSLIB0023 // O tipo ou membro é obsoleto
        static readonly RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
#pragma warning restore SYSLIB0023 // O tipo ou membro é obsoleto

        public static void AssertIsValid(string password)
        {
            AssertionConcern.AssertArgumentNotNull(password, "Senha de usuário inválida");
        }

        public static string Encrypt(string password)
        {
            SHA1 hasher = SHA1.Create();
            var encoding = new ASCIIEncoding();

            byte[] array = encoding.GetBytes(password);
            array = hasher.ComputeHash(array);

            var strHexa = new StringBuilder();

            foreach (byte item in array)
            {
                // Convertendo  para Hexadecimal
                strHexa.Append(item.ToString("x2"));
            }

            return strHexa.ToString();
        }

        public static string RandomPassowrd(int quantidadeLetras)
        {
            var seed = GetSeed(512);

            var random = GetRandomString(new Random(seed), quantidadeLetras);

            return random;

        }

        static string GetRandomString(Random random, int length)
        {
            string str = "";

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(0, chars.Length - 1);

                str += chars[index];
            }

            return str;
        }

        static int GetSeed(int length)
        {
            var bytes = new byte[length];

            provider.GetBytes(bytes);

            var seed = 0;

            foreach (byte b in bytes)
            {
                seed += b;
            }

            return seed;
        }
    }
}