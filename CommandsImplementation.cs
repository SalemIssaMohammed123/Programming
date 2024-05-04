using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programming
{
    public  class CommandsImplementation
    {
       public static void Help(string command = "")
        {
            // Implement help functionality here
            if (string.IsNullOrWhiteSpace(command))
            {
                Console.WriteLine("List of available commands:");
                Console.WriteLine("cd - Change the current default directory.If the argument is not present, report the current directory. If the directory does not exist an appropriate error should be reported.");
                Console.WriteLine("cls - Clear the screen.");
                Console.WriteLine("quit - Quit the shell.");
                Console.WriteLine("dir - List the contents of directory .");
                Console.WriteLine("copy - Copies one or more files to another location.");
                Console.WriteLine("del - Deletes one or more files.");
                Console.WriteLine("help -Provides Help information for commands.");
                Console.WriteLine("md - Creates a directory.");
                Console.WriteLine("rd - Removes a directory.");
                Console.WriteLine("rename -  Renames a file.");
                Console.WriteLine("type - Displays the contents of a text file.");
                Console.WriteLine("import – import text file(s) from your computer");
                Console.WriteLine("export – export text file(s) to your computer");

            }
            else
            {
                // Implement help for specific command
                Console.WriteLine($"Help information for {command}:");
                switch (command)
                {
                    case "cd":
                        Console.WriteLine("cd - Change the current default directory to the specified path.");
                        break;
                    case "cls":
                        Console.WriteLine("cls - Clear the screen.");
                        break;
                    case "quit":
                        Console.WriteLine("quit - Quit the shell.");
                        break;
                    // Add more cases for other commands here
                    case "copy":
                        Console.WriteLine("copy - Copies one or more files to another location.");
                        break;
                    case "del":
                        Console.WriteLine("del - Deletes one or more files.");
                        break;
                    case "help":
                        Console.WriteLine("help -Provides Help information for commands.");
                        break;
                    case "md":
                        Console.WriteLine("md - Creates a directory.");
                        break;
                    case "rd":
                        Console.WriteLine("rd - Removes a directory.");
                        break;
                    case "rename":
                        Console.WriteLine("rename -  Renames a file.");
                        break;
                    case "type":
                        Console.WriteLine("type - Displays the contents of a text file.");
                        break;
                    case "export":
                        Console.WriteLine("export – export text file(s) to your computer");
                        break;
                    case "import":
                        Console.WriteLine("import – import text file(s) from your computer");
                        break;
                    default:
                        Console.WriteLine("Command not found.");
                        break;
                }
            }
        }
        public static void md (string name)
        {
            
            if (Program.current_Directory.search(name) != null)
            {
                Console.WriteLine("Directory already exists.");
            }
            else
            {
                Directory_Entry newDirectory = new Directory_Entry()
                {
                    name = name.ToCharArray(),
                    size = 0,
                    first_cluster = 0,
                    attribute = 1,
                };
                Program.current_Directory.DirectoryTable.Add(newDirectory);
                FAT_Table.write_fat_table();
                Console.WriteLine("Directory created successfully.");
            }

        }
        public static void rd(string name)
        {
            Directory_Entry directory = 

            if (Program.current_Directory.search(name) == null)
            {
                Console.WriteLine("Directory not found.");
            }
            else
            {
                if (directory.attribute != 1)
                {
                    Console.WriteLine("Not a directory.");
                }
                else
                {
                    int firstCluster = directory.first_cluster;
                    _Directory newDirectory = new _Directory()
                    {
                        name = directory.name,
                        attribute = 1,
                        size = 0,
                        first_cluster = firstCluster,
                        parent = Program.current_Directory.parent
                    };

                    Program.current_Directory.DirectoryTable.Remove(newDirectory);
                    Console.WriteLine("Directory deleted successfully.");
                }
            }
        }
        public void dir(string name)
        {
            if (Program.current_Directory.search(name) == null)
            {
                Console.WriteLine("Directory doesnot exist.");
            }
            else
            {
                for (int i = 0; i < Program.current_Directory.DirectoryTable.Count; i++)
                {
                    Console.WriteLine(Program.current_Directory.DirectoryTable[i].name);
                }
            }
        }
    }
}
/*
 static void ChangeDirectory(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            Console.WriteLine(currentDirectory);
        }
        else
        {
            try
            {
                Directory.SetCurrentDirectory(path);
                currentDirectory = Directory.GetCurrentDirectory();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
*/
