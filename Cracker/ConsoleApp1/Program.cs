using Cracker;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using static Cracker.Encrypt;
using static Cracker.Tool;

namespace ConsoleApp1
{
    internal class Program
    {
        private static void Main(string[] args)
        {   // adds the usable characters to the list
            //chars = new List<char>();
            //Addchars(ref chars);
            //Console.WriteLine(chars.Count);
            //Passwords();
            // Cracker();
            //RainbowTable();
            //CheckCitites();
            // Decrypt();
            CheckCitites();

            Console.ReadKey();
        }


        public static void Cracker()
        {
            Console.Write("\nPlease enter the hash: "); Console.ForegroundColor = ConsoleColor.Cyan;
            string hash = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Hash format ( Hexa, HEXA, h:e:x:a, 64bitstring): ");
            string answer = Console.ReadLine();
            if (answer != "Hexa")
            {
                switch (answer)
                {
                    case "h:e:x:a":
                        answer = answer.Replace(":", "");
                        break;

                    case "HEXA":
                        answer = answer.ToLower();
                        break;

                    default:
                        Console.WriteLine("Error invalid hash format ");
                        Console.Beep(100, 10000);
                        break;
                }
            }
            string hashkind = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("Please enter the hash algorithm type (Md5, riperMD160, SHA1, SHA256, SHA384 or SHA512: ");
            string hashtype = Console.ReadLine();
            switch (hashtype.ToLower().Replace(" ", "").Replace("\t", ""))
            {
                case "md5":
                    MD5Decrypt(hash);
                    break;

                case "ripermd160":
                    RiperdMD160ecrypt(hash);
                    break;

                case "sha1":
                    SHA1Decrypt(hash);
                    break;

                case "sha256":
                    SHA256Decrypt(hash);
                    break;

                case "sha384":
                    SHA384Decrypt(hash);
                    break;

                case "sha512":
                    SHA512Decrypt(hash);
                    break;

                default:
                    Console.WriteLine("Not in the list");
                    break;
            }
            Console.ReadKey();
        }

        public static void RiperdMD160ecrypt(string hash)
        {
            string bhash = "";
            try
            {
                List<string> list = File.ReadLines("PassWord_Respository.txt").ToList();
                foreach (string lines in list)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write($"\t {lines} - ");
                    using (RIPEMD160 rIPEMD160 = RIPEMD160Managed.Create())
                    {
                        rIPEMD160.ComputeHash(ASCIIEncoding.ASCII.GetBytes(lines));
                        byte[] data = rIPEMD160.Hash;
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
                        Cracker();
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

        public static void RainbowTable()
        {
            HashSet<string> passwords = new HashSet<string>();
            //foreach (var item1 in Directory.EnumerateFiles("source/","*.txt"))
            //{
            passwords = new HashSet<string>(File.ReadLines("source/000webhost.txt", Encoding.GetEncoding("iso-8859-2")).Skip(252475));

            passwords = ReplaceandSort(passwords);
            ulong counter = 1;
            Stopwatch estimated = Stopwatch.StartNew();

            foreach (string item in passwords)
            {
                Stopwatch watch = Stopwatch.StartNew();
                int count = 0;

                if (count == 0)
                {
                    Rainbow data = new Rainbow
                    {
                        Password = item,
                        MD5 = MD5(item),
                        RipeMD160 = RipeMD160(item),
                        SHA1 = SHA1(item),
                        SHA256 = SHA256(item),
                        SHA384 = SHA384(item),
                        SHA512 = SHA512(item)
                    };
                    Console.WriteLine($"{counter} {data.Password} ");
                    using (IDbConnection connection = new SqlConnection(ConnectionString("Rainbow")))
                    {
                        string sql = @"exec dbo.uspInsert @Password, @MD5, @RipeMD160, @SHA1, @SHA256, @SHA384, @SHA512";
                        connection.Execute(sql, data);
                    }
                    counter++;
                }
                else
                {
                    Console.WriteLine("Already in the Database..");
                }

                watch.Stop();
                Console.WriteLine("List/Contains: {0}ms", watch.ElapsedMilliseconds);
            }

            estimated.Stop();
            TimeSpan time = estimated.Elapsed;
            Console.WriteLine($"Finished! Rows Read:{passwords.Count} Elapsead Time: {time.Hours}:{time.Minutes}:{time.Seconds}:{time.Milliseconds}");

            //}
        }

        public static string MD5(string passwd)
        {
            using (MD5CryptoServiceProvider mD5 = new MD5CryptoServiceProvider())
            {
                mD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(passwd));
                byte[] data = mD5.Hash;
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    stringBuilder.Append(data[i].ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }

        public static string RipeMD160(string passwd)
        {
            using (RIPEMD160 rIPEMD160 = RIPEMD160Managed.Create())
            {
                rIPEMD160.ComputeHash(ASCIIEncoding.ASCII.GetBytes(passwd));
                byte[] data = rIPEMD160.Hash;
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    stringBuilder.Append(data[i].ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }

        public static string SHA1(string passwd)
        {
            using (SHA1CryptoServiceProvider SHA1 = new SHA1CryptoServiceProvider())
            {
                SHA1.ComputeHash(ASCIIEncoding.ASCII.GetBytes(passwd));
                byte[] data = SHA1.Hash;
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    stringBuilder.Append(data[i].ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }

        public static string SHA256(string passwd)
        {
            using (SHA256CryptoServiceProvider sHA256 = new SHA256CryptoServiceProvider())
            {
                sHA256.ComputeHash(ASCIIEncoding.ASCII.GetBytes(passwd));
                byte[] data = sHA256.Hash;
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    stringBuilder.Append(data[i].ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }

        public static string SHA384(string passwd)
        {
            using (SHA384CryptoServiceProvider sHA384 = new SHA384CryptoServiceProvider())
            {
                sHA384.ComputeHash(ASCIIEncoding.ASCII.GetBytes(passwd));
                byte[] data = sHA384.Hash;
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    stringBuilder.Append(data[i].ToString("x2"));
                }
                return
 stringBuilder.ToString();
            }
        }

        public static string SHA512(string passwd)
        {
            using (SHA512CryptoServiceProvider sHA512 = new SHA512CryptoServiceProvider())
            {
                sHA512.ComputeHash(ASCIIEncoding.ASCII.GetBytes(passwd));
                byte[] data = sHA512.Hash;
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    stringBuilder.Append(data[i].ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }

        public static string ConnectionString(string connectionName = "Rainbow")
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }

        public static void Decrypt()
        {
            while (true)
            {
                string secret;
                Console.Write("Please insert the hash: ");
                string hash = Console.ReadLine();
                using (IDbConnection conn = new SqlConnection(ConnectionString("Rainbow")))
                {
                    DynamicParameters dynamic = new DynamicParameters();
                    dynamic.Add("@hash", hash);
                    string sql = $@"exec dbo.uspFindPassword @hash";
                    try
                    {
                        secret = conn.QuerySingle<string>(sql, dynamic);
                        Console.WriteLine($"The password is {secret}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                Console.ReadKey();
            }
        }

        public static void CheckCitites()
        {
            HashSet<string> missing = new HashSet<string>();
            HashSet<string> Cities = new HashSet<string>(File.ReadLines("CITIES.TXT"));
            Cities = new HashSet<string>(Cities.Distinct());
            HashSet<string> AllCities = new HashSet<string>();

            using (IDbConnection conn = new SqlConnection(ConnectionString("Shop")))
            {
                AllCities = new HashSet<string>(conn.Query<string>(@"exec dbo.uspGetAllCities"));
            }
            foreach (string item in Cities)
            {
                string[] temp = item.Split('\t');
                if (!AllCities.Contains(temp[1]))
                {
                    missing.Add(item);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Missing Countries:\n\n");
            missing.ToList().ForEach(x => Console.WriteLine(x));
            File.WriteAllLines("Missing.txt", missing);
        }

        public static string SHA512_64bytes(string password)
        {
            using (SHA512CryptoServiceProvider crypt = new SHA512CryptoServiceProvider())
            {
                StringBuilder hash = new StringBuilder();
                byte[] code = crypt.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(code);
            }
        }
    }
}