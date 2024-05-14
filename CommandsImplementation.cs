using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                    case "dir":
                        Console.WriteLine("dir - Displays directory of files and directories stored on disk.");
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

        public static void cd(string name)
        {
            int index = Program.current_Directory.search(name);
            if (index != -1)
            {

                int Firstcluster = Program.current_Directory.DirectoryTable[index].first_cluster;
                _Directory d = new _Directory(name, 0x1, Firstcluster, 0, Program.current_Directory);

                Program.current_Directory = d;
                Program.current_Path += "\\" + name;
                Program.current_Directory.Read_Directory();

            }
            else
            {
                Console.WriteLine("the system not found path");
            }

        }

        public static void copy(string s, string s2)
        {
            int index1 = Program.current_Directory.search(s);
            if (index1 != -1)
            {
                int start_index = s2.LastIndexOf("\\");
                string name = s2.Substring(start_index + 1);

                int index_destenation = Program.current_Directory.search(name);
                if (index_destenation == -1)
                {

                    if (s2 != Program.current_Directory.name.ToString())
                    {
                        int firstcluster = Program.current_Directory.DirectoryTable[index1].first_cluster;
                        int filesize = Program.current_Directory.DirectoryTable[index1].size;
                        Directory_Entry entry = new Directory_Entry(s, 0x1, firstcluster, filesize);
                        _Directory dir = new _Directory(s2, 0x1, firstcluster, filesize, Program.current_Directory.parent);
                        dir.DirectoryTable.Add(entry);



                    }
                    else
                    {
                        Console.WriteLine("not find");
                    }

                }
            }
        }


        public static void del(string name)
        {

            int index = Program.current_Directory.search(name);
            if (index != -1)
            {
                if (Program.current_Directory.DirectoryTable[index].attribute == 0x0)
                {

                    int firstcluster = Program.current_Directory.DirectoryTable[index].first_cluster;
                    int filesize = Program.current_Directory.DirectoryTable[index].size;
                    // string content = Program.current_directory.Directory_Table[index].fileContent;
                    _File file = new _File(name, 0x0, firstcluster, Program.current_Directory, "", filesize);
                    file.Delete();
                    Program.current_Path = new string(Program.current_Directory.name).Trim();

                }
                else
                {
                    Console.WriteLine("system cannot find this file ");
                }
            }
            else
            {
                Console.WriteLine("system cannot find this file ");
            }
        }

        public static void md (string name)
        {
            
            if (Program.current_Directory.search(name) != -1)
            {
                Console.WriteLine("Directory or file " + name + " already exists.");
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
                Program.current_Directory.Write_Directory();
                if (Program.current_Directory.parent != null)
                {
                    Program.current_Directory.parent.update(newDirectory);
                    Program.current_Directory.parent.Write_Directory();

                }
                //FAT_Table.write_fat_table();
                Console.WriteLine("Directory created successfully.");
            }

        }
        public static void rd(string name)
        {
            int index = Program.current_Directory.search(name);
            if (index != -1)
            {
                int firstCluster = Program.current_Directory.DirectoryTable[index].first_cluster;


                _Directory d1 = new _Directory(name, 0x1, firstCluster, 0, Program.current_Directory);
                d1.DeleteDirectory();

                Program.current_Path = new string(Program.current_Directory.name).Trim();
            }
            else
            {
                Console.WriteLine("The system cannot find the path specified.");
                Console.WriteLine("And cannot remove this directory.");
            }
        }
        public static void dir()
        {

            foreach (var directory in Program.current_Directory.DirectoryTable)
            {
                if (directory.name[0] != '\0')
                    Console.WriteLine(new string(directory.name).TrimEnd('\0'));
            }
           
        }

        public static void rename(string old, string New)
        {

            int index = Program.current_Directory.search(old);

            if (index != -1)
            {
                int index1 = Program.current_Directory.search(New);
                if (index1 == -1)
                {
                    Directory_Entry d = Program.current_Directory.DirectoryTable[index];

                    d.name = New.ToCharArray();

                    Program.current_Directory.DirectoryTable.RemoveAt(index);
                    Program.current_Directory.DirectoryTable.Insert(index1 + 1, d);
                    Program.current_Directory.Write_Directory();
                    Program.current_Directory.Read_Directory();

                }

                else
                {
                    Console.WriteLine("duplicate filename exists or file cannnot be found now");



                }
            }
            else
            {
                Console.WriteLine("the system cannot find this file ");
            }
        }

        public static void type(string name)
        {

            int index = Program.current_Directory.search(name);
            if (index != -1)
            {
                string content = null;

                int firstcluster = Program.current_Directory.DirectoryTable[index].first_cluster;
                int filesize = Program.current_Directory.DirectoryTable[index].size;
                _File file = new _File(name, 0x0, firstcluster, Program.current_Directory, content, filesize);
                file.readfile();

                Console.WriteLine(file.content);

            }
            else Console.WriteLine(" system cannot find file");

        }

        public static void import(string Name)
        {
            if (File.Exists(Name))
            {
                string content = File.ReadAllText(Name);
                int size = content.Length;
                int startname = Name.LastIndexOf("\\");
                string name;
                name = Name.Substring(startname + 1);
                int index = Program.current_Directory.search(name);
                if (index == -1)
                {
                    int fisrtCluster;
                    if (size > 0)
                    {
                        fisrtCluster = FAT_Table.get_available_block();
                    }
                    else
                    {
                        fisrtCluster = 0;
                    }
                    _File f = new _File(name, 0x0, fisrtCluster, Program.current_Directory, content, size);
                    f.writefile();
                    Directory_Entry d = new Directory_Entry(name, 0x0, fisrtCluster, size);
                    Program.current_Directory.DirectoryTable.Add(d);
                    Program.current_Directory.Write_Directory();

                }
                else
                {
                    Console.WriteLine("file already Exist");
                }
            }
            else
            {
                Console.WriteLine("this file Not Exists ");
            }
        }

        public static void export(string s, string d)
        {
            int index = Program.current_Directory.search(s);
            if (index != -1)
            {
                if (System.IO.Directory.Exists(d))
                {
                    int f_c = Program.current_Directory.DirectoryTable[index].first_cluster;
                    int f_size = Program.current_Directory.DirectoryTable[index].size;
                    string f_content = null;
                    _File file = new _File(s, 0x0, f_c, Program.current_Directory, f_content, f_size);
                    file.readfile();



                    StreamWriter stream_writer = new StreamWriter(d + "\\" + s);
                    stream_writer.Write(file.content);
                    stream_writer.Flush();
                    stream_writer.Close();
                }
                else Console.WriteLine("the system cannot find this path ");
            }
            else Console.WriteLine("this file is not exist");
        }

    }
}

