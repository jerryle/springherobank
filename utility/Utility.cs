using System;
using System.Security.Cryptography;
using System.Text;

namespace SpringHeroBank
{
    public class Utility
    {
        public int GetIntNum()
        {
            var value = 0;
            while (true)
            {
                try
                {
                    value = int.Parse(Console.ReadLine());
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Vui lòng nhập số nguyên.");
                }
            }
            return value;
        }

        public decimal GetDecimalNum()
        {
            decimal value = 0;
            while (true)
            {
                try
                {
                    value = Decimal.Parse(Console.ReadLine());
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Vui lòng nhập số nguyên.");
                }
            }
            return value;
        }

        public string EnterPassword()
        {
            var pass = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);


                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter && pass.Length < 30)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    pass = pass.Substring(0, (pass.Length - 1));
                    Console.Write("\b \b");
                }
            }
            while (key.Key != ConsoleKey.Enter);
            return pass;
        }

        public DateTime GetDateTime()
        {
            // Console.WriteLine("Enter a date: ");
            DateTime inputtedDate;
            // while(true)
            // {
            //     try {
            //         userDateTime = DateTime.Parse(Console.ReadLine());
            //         break;
            //     } catch(Exception e) {
            //         Console.WriteLine("Vui lòng nhập số nguyên.");
            //     }
            // }
            while (true)
            {
                try
                {
                    Console.Write("Nhập ngày: ");
                    int day = int.Parse(Console.ReadLine());
                    Console.Write("Nhập tháng: ");
                    int month = int.Parse(Console.ReadLine());
                    Console.Write("Nhập năm: ");
                    int year = int.Parse(Console.ReadLine());

                    inputtedDate = new DateTime(year, month, day);
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Vui lòng nhập đúng ngày tháng.");
                }
            }
            // Console.ReadLine();

            return inputtedDate;
        }

        public decimal GetUnsignedDecimalNum()
        {
            decimal value = 0;
            while (true)
            {
                try
                {
                    value = Decimal.Parse(Console.ReadLine());
                    if (value > 0) break;
                    else Console.WriteLine("Vui lòng nhập giá trị lớn hơn 0");
                }
                catch (Exception)
                {
                    Console.WriteLine("Vui lòng nhập số nguyên.");
                }
            }
            return value;
        }

        private static MD5CryptoServiceProvider algorith = new MD5CryptoServiceProvider();
        public string EncryptedString(string content, string salt)
        {
            byte[] array = Encoding.ASCII.GetBytes(content + salt);
            byte[] encryptedByte = algorith.ComputeHash(array);
            return Convert.ToBase64String(encryptedByte);
        }
    }
}