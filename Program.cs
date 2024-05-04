using System;
using System.IO;
namespace Programming
{
    public class Program : CommandsImplementation
    { 
        static string currentDirectory = Directory.GetCurrentDirectory();
        public static string input;
		public static _Directory current_Directory = new _Directory();
		static void Main(string[] args)
        {   //initialize both virtual disk and FAT System
            Virtual_Disk.Initialize();
            FAT_Table.initialize();
            while (true)
            {
                Console.Write($"{currentDirectory}> ");
                 input = Console.ReadLine().Trim().ToLower();

                if (string.IsNullOrWhiteSpace(input))
                     continue;

                string[] inputParts = input.Split(' ', 2);
                string command = inputParts[0];
                string parameter = inputParts.Length > 1 ? inputParts[1] : "";

                switch (command)
                {
                    case "help":
                        Help(parameter);
                        break;
                    case "cls":
                        Console.Clear();
                        break;
                    case "quit":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Command not recognized. Type 'help' for a list of available commands.");
                        break;
                }
            }
        }
        

    }
}
