using AspNetCore.LegacyAuthCookieCompat;
using Newtonsoft.Json;
using System;

namespace CrossCutting.Util
{
    public static class FormAuthenticationExtend
    {

        static string validationKey = "B467658DBDA81FBDBE2186DD472574FDECB8941C3B657AA26A04D435FA465FE1267BC6285CE320A06189CF2F73483FCC13B80815A46264A1EE7AE3DEF7B0FFD3";
        static string decryptionKey = "A21C0936A41C1E0FF1F021030E820E8147622D99C0664F3822FC395DC83E3674";

        public static string Encrypt(object obj, string nome)
        {
            var operadorData = JsonConvert.SerializeObject(obj);
            var formsAuthenticationTicket = new FormsAuthenticationTicket(1, nome, DateTime.Now, DateTime.Now.AddSeconds(40),
                false, operadorData, "/");

            var legacyFormsAuthenticationTicketEncryptor = new LegacyFormsAuthenticationTicketEncryptor(decryptionKey, validationKey, ShaVersion.Sha256);

            // Act
            // We encrypt the forms auth cookie.
            var encryptedText = legacyFormsAuthenticationTicketEncryptor.Encrypt(formsAuthenticationTicket);

            return encryptedText;
        }

    }
}
