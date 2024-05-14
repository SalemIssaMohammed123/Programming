using System;
using System.IO;
namespace Programming
{
    public class Program : CommandsImplementation
    { 
        public static string current_Path;
        public static string input;
		public static _Directory current_Directory ;
		static void Main(string[] args)
        {

            current_Directory = new _Directory("root", 1, 0, 5, null!);
            //initialize both virtual disk and FAT System
            Virtual_Disk.Initialize();
            // Initialize the root directory
            current_Directory.Read_Directory();
            current_Directory.Write_Directory(); // Write the root directory to virtual disk
            //current_Path = new string(current_Directory.name);
            while (true)
            {
                Console.Write($"{string.Join("", current_Directory.name)}/>");
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
                    case "cd":
                        cd(parameter);
                        break;
                    case "md":
                        md(parameter);
                        break;
                    case "rd":
                        rd(parameter);
                        break;
                    case "dir":
                        dir();
                        break;
                    case "rename":
                        string[] renameParams = parameter.Split(' ', 2);
                        string sourceFileName = renameParams[0];
                        string targetFileName = renameParams.Length > 1 ? renameParams[1] : "";
                        rename(sourceFileName, targetFileName);
                        break;
                    case "copy":
                        string[] copyParams = parameter.Split(' ', 2);
                        string SourceFileName = copyParams[0];
                        string TargetFileName = copyParams.Length > 1 ? copyParams[1] : "";
                        copy(SourceFileName, TargetFileName);
                        break;
                    case "type":
                        type(parameter);
                        break;
                    case "del":
                        del(parameter);
                        break;
                    case "import":
                        import(parameter);
                        break;
                    case "export":
                        string[] exportParams = parameter.Split(' ', 2);
                        string srcFileName = exportParams[0];
                        string target_FileName = exportParams.Length > 1 ? exportParams[1] : "";
                        export(srcFileName, target_FileName);
                        break;
                    default:
                        Console.WriteLine("Command not recognized. Type 'help' for a list of available commands.");
                        break;
                }
            }
        }
        

    }
}
