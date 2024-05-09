using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Programming;
using System.Drawing;
namespace Programming
{
    class _File : Directory_Entry
    {


        public _Directory parent;
        public string content;

        public _File(string fileName, byte fileAttr, int firstCluster, _Directory parent, string content, int file_size) : base(fileName, fileAttr, file_size ,firstCluster)
        {
            this.parent = parent;
            this.content = content;

        }

        public byte[] Getfilebyte(string f)
        {
            byte[] arry = Encoding.ASCII.GetBytes(f);
            return arry;


        }
        public _File()
        {

        }




        public void writefile()
        {


            byte[] ContentBytes = Encoding.ASCII.GetBytes(content);
            double required_blocks = Math.Ceiling(ContentBytes.Length / 1024.0);
            int full_size_block = ContentBytes.Length / 1024;
            int reminder = ContentBytes.Length % 1024;

            if (required_blocks <= FAT_Table.get_available_block())
            {
                List<byte[]> ls = new List<byte[]>();
                if (ContentBytes.Length > 0)
                {

                    byte[] b = new byte[1024];
                    for (int i = 0; i < full_size_block; i++)
                    {
                        for (int j = i * 1024; j < ((i + 1) * 1024); j++)
                            b[j % 1024] = ContentBytes[j];
                        ls.Add(b);
                    }
                    if (reminder > 0)
                    {
                        b = new byte[1024];
                        for (int i = full_size_block * 1024, k = 0; k < reminder; i++, k++)
                        {
                            b[k] = ContentBytes[i];
                        }
                        ls.Add(b);
                    }


                }
                else
                {

                }

                int FirstIndex;
                int LastFirstIndex = -1;
                if (this.first_cluster != 0)
                {
                    FirstIndex = this.first_cluster;
                }
                else
                {
                    FirstIndex = FAT_Table.get_available_block();
                    this.first_cluster = FirstIndex;
                }

                for (int i = 0; i < ls.Count; i++)
                {

                    Virtual_Disk.Write_Block(ls[i], FirstIndex);
                    FAT_Table.set_value(FirstIndex, -1);
                    if (LastFirstIndex != -1)
                        FAT_Table.set_value(LastFirstIndex, FirstIndex);
                    LastFirstIndex = FirstIndex;
                    FirstIndex = FAT_Table.get_available_block();

                }
                if (ContentBytes.Length == 0)
                {
                    if (first_cluster != 0)
                    {
                        FAT_Table.set_value(first_cluster, 0);
                        first_cluster = 0;

                    }
                }

                FAT_Table.write_fat_table();



            }
            else { Console.WriteLine("directory size  exceeds free space size "); }

        }

        public void readfile()
        {

            if (this.first_cluster != 0)
            {
                int FirstIndex = this.first_cluster;
                int next = FAT_Table.get_value(FirstIndex);
                List<byte> ls = new List<byte>();
                do
                {
                    ls.AddRange(Virtual_Disk.Read_Block(FirstIndex));
                    FirstIndex = next;
                    if (next != -1)
                        next = FAT_Table.get_value(FirstIndex);
                } while (next != -1);

                this.content = string.Empty;

                for (int i = 0; i < ls.Count; i++)
                    if ((char)ls[i] != '\0')
                        this.content += (char)ls[i];


            }
        }

        public int Search_Directory(string name)
        {
            _Directory d = new _Directory();
            readfile();
            for (int i = 0; i < d.DirectoryTable.Count; i++)
            {
                if (d.DirectoryTable[i].name.Equals(name))
                {
                    return i;
                }
            }
            if (name.Length < 11)
            {
                name = name + " ";

            }
            else
            {
                name = name.Substring(0, 11);
            }



            return -1;
        }


        public void Delete()
        {

            if (first_cluster != 0)
            {

                int Fatindex = first_cluster;
                int next = FAT_Table.get_value(Fatindex);
                do
                {
                    FAT_Table.set_value(Fatindex, 0);
                    Fatindex = next;
                    if (Fatindex != -1)
                    {
                        next = FAT_Table.get_value(Fatindex);

                    }
                }
                while (Fatindex != -1);
            }
            if (parent != null)
            {
                parent.Read_Directory();

                string b = new string(name);
                int i = parent.search(b);
                if (i != -1)
                {
                    parent.DirectoryTable.RemoveAt(i);
                }

                parent.Write_Directory();

            }
            FAT_Table.write_fat_table();

        }


        public void update(Directory_Entry d)
        {
            _Directory d = new _Directory();

            d.Read_Directory();
            int index = Search_Directory(new string(d.name));


            if (index != -1)
            {
                d.DirectoryTable.RemoveAt(index);
                d.DirectoryTable.Insert(index, d);
                d.Write_Directory();

            }
            else
            {
                Console.WriteLine("Not found such file or directory");
            }
        }


    }
}



