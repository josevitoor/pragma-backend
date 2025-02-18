using System;
using System.Text;
using System.Text.RegularExpressions;

namespace TceAdmin.Util.Validation
{
    public static class CpfAssertionConcern
    {
        public static string RemoveNaoNumericos(String pText)
        {
            Regex reg = new Regex(@"[^0-9]");
            String ret = reg.Replace(pText, String.Empty);
            return ret;
        }

        public static bool ValidaCPF(String pCpf)
        {
            String cpf;

            // Remove formatação do número, ex: "123.456.789-01" => "12345678901"
            cpf = RemoveNaoNumericos(pCpf);

            if (cpf.Length > 11)
                return false;

            while (cpf.Length != 11)
                cpf = '0' + cpf;

            // Verifica se todos os caracteres são iguais. Ex: "111.111.111-11"            
            if (ValidaSequenciaNumerica(cpf))
            {
                int[] numeros = new int[11];
                for (int i = 0; i < 11; i++)
                {
                    numeros[i] = Int32.Parse(cpf[i].ToString());
                }

                if (!Valida9Digitos(numeros))
                {
                    return false;
                }

                if (!Valida2Digitos(numeros))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Valida9Digitos(int[] numeros)
        {
            int soma = 0;
            for (int i = 0; i < 9; i++)
            {
                soma += (10 - i) * numeros[i];
            }

            int resultado = soma % 11;
            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                {
                    return false;
                }
            }
            else if (numeros[9] != 11 - resultado)
            {
                return false;
            }
            return true;
        }

        public static bool Valida2Digitos(int[] numeros)
        {
            int soma = 0;
            for (int i = 0; i < 10; i++)
            {
                soma += (11 - i) * numeros[i];
            }

            int resultado = soma % 11;
            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                {
                    return false;
                }
            }
            else if (numeros[10] != 11 - resultado)
            {
                return false;
            }
            return true;
        }

        public static bool ValidaSequenciaNumerica(string cpf)
        {
            bool igual = true;
            for (int i = 0; i < 11 && igual; i++)
            {
                if (cpf[i] != cpf[0])
                {
                    igual = false;
                }
            }

            if (igual || cpf.Equals("12345678909"))
            {
                return false;
            }
            return true;

        }

        public static string formatCpf(string cpf)
        {
            if (cpf == null || cpf.Length != 11)
                return cpf;

            StringBuilder bld = new StringBuilder();
            for (int i = 0; i < cpf.Length; i++)
            {
                if (!Char.IsDigit(cpf[i]))
                    return cpf;
                bld.Append(cpf[i]);

                if (i == 8)
                    bld.Append('-');
                else if (i % 3 == 2)
                    bld.Append('.');
            }
            string result = bld.ToString();
            return result;
        }

    }
}
