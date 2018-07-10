using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainbowEF
{
    class UI
    {
        private Repository repo;

        public UI()
        {

        }

        public UI(Repository repo)
        {
            this.repo = repo;
        }

        private static void printMenu()
        {
            Console.WriteLine("Menu");
            Console.WriteLine("1 - Add user");
            Console.WriteLine("2 -  Recover password for user");
            Console.WriteLine("x - Exit");
        }

        public void readInput()
        {
            printMenu();
            while (true)
            {
                Console.WriteLine("Choose option");
                string cmd = Console.ReadLine();
                switch (cmd)
                {
                    case "1":
                        insertUser();
                        break;
                    case "2":
                        hackPassword();
                        break;
                    case "x":
                        goto Outer;
                    default:
                        Console.WriteLine("Bad Input");
                        break;
                }
            }
            Outer:
            return;
        }

        private void hackPassword()
        {
            string username;
            string password;

            Console.WriteLine("Enter user for which you want to find password");
            username = Console.ReadLine();
            password = this.repo.GetPassword(username);
            Console.WriteLine("Password for user {0} is {1}", username, password);

        }

        private void insertUser()
        {
            string username;
            string password;
            string hashAlgorithm;
            Console.WriteLine("Enter username");
            username = Console.ReadLine();
            Console.WriteLine("Enter password");
            password = Console.ReadLine();
            Console.WriteLine("Enter hash algorith{ MD5 | SHA1 | SHA256 }");
            hashAlgorithm = Console.ReadLine();

            repo.Add(username, password, hashAlgorithm);
        }
    }
}
