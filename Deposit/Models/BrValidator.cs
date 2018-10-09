using System;

namespace Deposit.Models
{
    public static class BrValidator
    {
        public static Boolean ValidateMail(String mail)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(mail, @"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");
        }

        public static Boolean ValidateCPF(String CPF)
        {
            if (CPF.Length < 11 || CPF.Length > 11)
                return false;

            if (CPF == "11111111111" || CPF == "22222222222" || CPF == "33333333333" || CPF == "44444444444" ||
                CPF == "55555555555" || CPF == "66666666666" || CPF == "77777777777" || CPF == "88888888888" ||
                CPF == "99999999999" || CPF == "00000000000")
                return false;

            int sumChars = 0;

            for (int i = 0, j = 11; i < CPF.Length - 1; i++, j--)
                sumChars += Convert.ToInt32(CPF[i].ToString()) * j;

            sumChars = (sumChars * 10) % 11;

            return sumChars.ToString() == CPF[10].ToString() ? true : false;
        }

        public static Boolean ValidateCNPJ(String CNPJ)
        {
            if (CNPJ.Length != 14)
                return false;

            if (!IsNumber(CNPJ))
                return false;

            if (CNPJ == "11111111111" || CNPJ == "22222222222" || CNPJ == "33333333333" || CNPJ == "44444444444" ||
                CNPJ == "55555555555" || CNPJ == "66666666666" || CNPJ == "77777777777" || CNPJ == "88888888888" ||
                CNPJ == "99999999999" || CNPJ == "00000000000")
                return false;

            String cnpjNums = String.Empty;

            Int32[] firstMultipliers = new Int32[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            Int32 sum = 0;

            for (Int32 i = 0; i < CNPJ.Length-2; i++)
                sum += Convert.ToInt32(firstMultipliers[i] * Convert.ToUInt32(CNPJ[i].ToString()));

            Int32 firstVerifyingDigit = (sum % 11) < 2 ? 0 : 11 - sum % 11;
            if (CNPJ[12].ToString() != firstVerifyingDigit.ToString())
                return false;

            Int32[] secondMultipliers = new Int32[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            sum = 0;

            for (Int32 i = 0; i < CNPJ.Length - 1; i++)
                sum += Convert.ToInt32(secondMultipliers[i] * Convert.ToUInt32(CNPJ[i].ToString()));

            Int32 secondVerifyingDigit = (sum % 11) < 2 ? 0 : 11 - sum % 11;
            if (CNPJ[13].ToString() != secondVerifyingDigit.ToString())
                return false;

            return true;
        }

        private static Boolean IsNumber(String s)
        {
            for (Int32 i = 0; i < s.Length - 1; i++)
            {
                if (!Char.IsDigit(s[i]))
                    return false;
            }

            return true;
        }
    }
}