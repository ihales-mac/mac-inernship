using System;

namespace RainbowTables
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
            Console.WriteLine("2 -  Get password for a user");
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
                    case "3":
                        makeBackup();
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

        private void makeBackup()
        {
            this.repo.copyPassToAnotherTable();

        }

        private void hackPassword()
        {
            string username;
            string password;

            Console.WriteLine("Enter user for which you want to find password");
            username = Console.ReadLine();
            password = this.repo.getPassword(username);
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
            Console.WriteLine("Enter hash algorith{md2 | md4 | md5 | sha | sha1}");
            hashAlgorithm = Console.ReadLine();

            repo.add(username, password, hashAlgorithm);
        }
    }
}
