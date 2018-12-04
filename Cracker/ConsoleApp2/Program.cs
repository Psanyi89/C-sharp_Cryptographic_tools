using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cracker.Tool;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            string pass = "";
            do
            {
                Console.Write("Enter Password: ");
                pass = Console.ReadLine();

            } while (!IsValid(pass));
            Console.WriteLine("Good");
        }
    }
}
