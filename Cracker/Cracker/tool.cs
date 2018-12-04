using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Cracker
{
    public class Rainbow
    {
        public string Password { get; set; }
        public string MD5 { get; set; }
        public string RipeMD160 { get; set; }
        public string SHA1 { get; set; }
        public string SHA256 { get; set; }
        public string SHA384 { get; set; }
        public string SHA512 { get; set; }
    }

    public class Tool
    {
        public static List<char> chars = new List<char>();
        public static HashSet<string> passwd = new HashSet<string>();

        public static void Addchars(ref List<char> chars)
        {
            for (char x = 'a'; x <= 'z'; x++)
            {
                chars.Add(x);
            }
            for (char x = 'A'; x <= 'Z'; x++)
            {
                chars.Add(x);
            }
            for (char x = '0'; x <= '9'; x++)
            {
                chars.Add(x);
            }
            //for (char x = '!'; x <= '_'; x++)
            //{
            //    chars.Add(x);
            //}

            chars.Add('$');
            chars.Add('@');
            chars.Add('!');
            chars.Add('?');
            chars.Add('%');
            chars.Add('*');
            chars.Add('&');
            //chars.Add('á');
            //chars.Add('Á');
            //chars.Add('é');
            //chars.Add('É');
            //chars.Add('ű');
            //chars.Add('Ű');
            //chars.Add('í');
            //chars.Add('Í');
            //chars.Add('ú');
            //chars.Add('Ú');
            //chars.Add('ő');
            //chars.Add('Ő');
            //chars.Add('ó');
            //chars.Add('Ó');
            //chars.Add('ü');
            //chars.Add('Ü');
            //chars.Add('ö');
            //chars.Add('Ö');
        }

        public static string PasswdGenerator()
        {
            Random rnd = new Random();
            StringBuilder build = new StringBuilder();
            int x = 8/*rnd.Next(8, 51)*/;
            int y = 0;
            while (y < x)
            {
                build.Append(chars[rnd.Next(0, chars.Count)]);
                y++;
            }

            return build.ToString();
        }

        public static List<string> Passwords(int y)
        {
            HashSet<string> passwd = new HashSet<string>(File.ReadLines("PassWord_Respository.txt"));
            passwd = new HashSet<string>(passwd.Distinct());
            passwd = new HashSet<string>(passwd.Select(item =>
            {
                item = Regex.Replace(item, @"\s+", "");
                item = Regex.Replace(item, @"\t", "");
                return item;
            }));
            passwd.OrderBy(x => x);
            File.WriteAllLines("PassWord_Respository.txt", passwd);

            string word = "";
            ulong result = Convert.ToUInt64(Math.Pow(chars.Count, y));

            Console.WriteLine($"{ulong.Parse(passwd.Count.ToString())} < {result}");
            while (ulong.Parse(passwd.Count.ToString()) < result)
            {
                //Stopwatch watch = Stopwatch.StartNew();
                do
                {
                    do
                    {
                        word = PasswdGenerator();
                    } while (!IsValid(word));
                } while (passwd.Contains(word));

                Console.WriteLine($"{passwd.Count} {word}");
                passwd.Add(word);
                using (TextWriter write = new StreamWriter("PassWord_Respository.txt", true))
                {
                    write.WriteLine(passwd.Last());
                }
                //watch.Stop();
                //Console.WriteLine("List/Contains: {0}ms", watch.ElapsedMilliseconds);
            }
            List<string> passwd2 = passwd.OrderBy(x => x).ToList();
            return passwd2;
        }

        public static bool IsValid(string pass)
        {
            Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,}");
            Match match = regex.Match(pass);
            return match.Success;
        }

        public static string GetPassword(int x, int y)
        {
            Random length = new Random();
            int strength = length.Next(x, y);
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] buff = new byte[strength];
                rng.GetBytes(buff);
                return Convert.ToBase64String(buff);
            }
        }

        public static ulong Rounds(int y)
        {
            return Convert.ToUInt64(Math.Pow(94, y));
        }

        public static HashSet<string> ReplaceandSort(HashSet<string> vs)
        {
            HashSet<string> passwd2 = new HashSet<string>(vs.Distinct());
            passwd2 = new HashSet<string>(passwd2.Select(item =>
            {
                item = Regex.Replace(item, @"\s+", "");
                item = Regex.Replace(item, @"\t", "");
                item = Regex.Replace(item, @"'", "");
                return item;
            }));
            passwd2 = new HashSet<string>(passwd2.OrderBy(x => x));
            File.WriteAllLines("PassWord_Respository.txt", passwd2);
            return passwd2;
        }
    }

    public class Encrypt
    {
        public static void SHA512Decrypt(string hash)
        {
            string bhash = "";
            try
            {
                List<string> list = File.ReadLines("PassWord_Respository.txt").ToList();
                foreach (string lines in list)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write($"\t {lines} - ");
                    using (SHA512CryptoServiceProvider sHA512 = new SHA512CryptoServiceProvider())
                    {
                        sHA512.ComputeHash(ASCIIEncoding.ASCII.GetBytes(lines));
                        byte[] data = sHA512.Hash;
                        StringBuilder stringBuilder = new StringBuilder();
                        for (int i = 0; i < data.Length; i++)
                        {
                            stringBuilder.Append(data[i].ToString("x2"));
                        }
                        bhash = stringBuilder.ToString();
                    }
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($" {bhash} -");
                    if (bhash == hash)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write(" Cracked!");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(" Not Match\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }

        public static void SHA384Decrypt(string hash)
        {
            string bhash = "";
            try
            {
                List<string> list = File.ReadLines("PassWord_Respository.txt").ToList();
                foreach (string lines in list)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write($"\t {lines} - ");
                    using (SHA384CryptoServiceProvider sHA384 = new SHA384CryptoServiceProvider())
                    {
                        sHA384.ComputeHash(ASCIIEncoding.ASCII.GetBytes(lines));
                        byte[] data = sHA384.Hash;
                        StringBuilder stringBuilder = new StringBuilder();
                        for (int i = 0; i < data.Length; i++)
                        {
                            stringBuilder.Append(data[i].ToString("x2"));
                        }
                        bhash = stringBuilder.ToString();
                    }
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($" {bhash} -");
                    if (bhash == hash)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write(" Cracked!");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(" Not Match\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }

        public static void SHA256Decrypt(string hash)
        {
            string bhash = "";
            try
            {
                List<string> list = File.ReadLines("PassWord_Respository.txt").ToList();
                foreach (string lines in list)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write($"\t {lines} - ");
                    using (SHA256CryptoServiceProvider sHA256 = new SHA256CryptoServiceProvider())
                    {
                        sHA256.ComputeHash(ASCIIEncoding.ASCII.GetBytes(lines));
                        byte[] data = sHA256.Hash;
                        StringBuilder stringBuilder = new StringBuilder();
                        for (int i = 0; i < data.Length; i++)
                        {
                            stringBuilder.Append(data[i].ToString("x2"));
                        }
                        bhash = stringBuilder.ToString();
                    }
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($" {bhash} -");
                    if (bhash == hash)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write(" Cracked!");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(" Not Match\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }

        public static void SHA1Decrypt(string hash)
        {
            string bhash = "";
            try
            {
                List<string> list = File.ReadLines("PassWord_Respository.txt").ToList();
                foreach (string lines in list)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write($"\t {lines} - ");
                    using (SHA1CryptoServiceProvider SHA1 = new SHA1CryptoServiceProvider())
                    {
                        SHA1.ComputeHash(ASCIIEncoding.ASCII.GetBytes(lines));
                        byte[] data = SHA1.Hash;
                        StringBuilder stringBuilder = new StringBuilder();
                        for (int i = 0; i < data.Length; i++)
                        {
                            stringBuilder.Append(data[i].ToString("x2"));
                        }
                        bhash = stringBuilder.ToString();
                    }
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($" {bhash} -");
                    if (bhash == hash)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write(" Cracked!");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(" Not Match\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }

        public static void MD5Decrypt(string hash)
        {
            string bhash = "";
            try
            {
                List<string> list = File.ReadLines("PassWord_Respository.txt").ToList();
                foreach (string lines in list)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write($"\t {lines} - ");
                    using (MD5CryptoServiceProvider mD5 = new MD5CryptoServiceProvider())
                    {
                        mD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(lines));
                        byte[] data = mD5.Hash;
                        StringBuilder stringBuilder = new StringBuilder();
                        for (int i = 0; i < data.Length; i++)
                        {
                            stringBuilder.Append(data[i].ToString("x2"));
                        }
                        bhash = stringBuilder.ToString();
                    }
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($" {bhash} -");
                    if (bhash == hash)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write(" Cracked!");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(" Not Match\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }
    }
}