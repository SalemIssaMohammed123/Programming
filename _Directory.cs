using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programming
{
	public class _Directory : Directory_Entry
	{
		public _Directory parent;
        public List<Directory_Entry> DirectoryTable = new List<Directory_Entry>();
		public _Directory()
		{
		}

		public _Directory(string n, byte attr, byte[] empty, int siz, int fc,_Directory parent) : base( n,  attr,  empty,  siz,  fc)
		{
			this.parent = parent;
            if (first_cluster != 0)
                fc = first_cluster;
            else
            {
                fc = FAT_Table.get_available_block();
                first_cluster = fc;
            }
        }
		public void Write_Directory()
		{
			int count = DirectoryTable.Count;
			byte[] array1 = new byte[32];
			List<byte> array2 = new List<byte>();// or use byte[]array2=new byte[32*count];
			foreach (var DirectoryEntry in DirectoryTable)
			{
				Array.Copy(DirectoryEntry.Convert_Directory_Entry_TobyteArray(), 0, array1, 0, array1.Length);
				array2.AddRange(array1);
			}
			/////////////////////////////////////////////////code here
			int nc = -1;
			int totalBlocks = (int)Math.Ceiling(array2.Count / 1024.0);
			int fullBlocks = array2.Count / 1024;
			int remainderBlocks = array2.Count % 1024;
			//creat an array to make the remainder block as fullblock
			byte[] reserved = new byte[1024 - remainderBlocks];
			byte[] totalData = array2.ToArray();
			totalData = totalData.Concat(reserved).ToArray();
			for (int i = 0; i < fullBlocks; i++)
			{
				byte[] block = new byte[1024];
				int start = i * 1024;
				int end = start + 1024;
				Array.Copy(totalData, start, block, 0, block.Length);
				Virtual_Disk.Write_Block(block, i);
				if (nc == -1)
				{
					FAT_Table.set_value(first_cluster, -1);
					nc = first_cluster;
					first_cluster = FAT_Table.get_available_block();
				}
				if (i == 0)
				{
					nc = FAT_Table.get_value(first_cluster);
				}
				if (nc != -1)
				{
					FAT_Table.set_value(first_cluster, nc);
				}
				else
				{
					FAT_Table.set_value(nc, first_cluster);
					nc = first_cluster;
					first_cluster = FAT_Table.get_available_block();
				}
			}
			FAT_Table.write_fat_table();
		}
		public void Read_Directory()
		{
            //create list of bytes
            List<byte> list = new List<byte>();
			int fc = first_cluster;
			int nc = FAT_Table.get_value(fc);
			while(nc != -1)
			{
				byte[]block=Virtual_Disk.Read_Block(fc);
				list.AddRange(block);
				fc = nc;
				nc=FAT_Table.get_value(fc);
			}

			for(int i = 0; i < list.Count / 32; i++)
			{
				int start = i * 32;
				byte[] array = new byte[32];
                Array.Copy(list.ToArray(), start, array, 0, array.Length);
				Directory_Entry de = Get_Directory_Entry(array);
                DirectoryTable.Add(de);


            }
        }
		//vji0.0
		public int search(string name)
		{
            //Loop over the directory table
            for (int i = 0; i < DirectoryTable.Count; i++)
			{
                Directory_Entry directory=new Directory_Entry();
                directory = DirectoryTable[i];
				//casting the array of char to string and use function that exists in any string TrimEnd("\0")
                string directoryName = new string(directory.name);
                //Compare the directory entry name with the given parameter
                if (directoryName == name.TrimEnd('\0'))

                {
					//Return the index if both names are identical
					return i;
                }
            }
			return -1;

		}
        private int GetParentDirectoryIndex(_Directory de)
        {
            int parentDirectoryIndex = -1; // Default value if parent directory is not found
            int targetFirstCluster = DirectoryTable[search(de.name.ToString())].first_cluster;
            // Iterate through the DirectoryTable to find the parent directory
            for (int i = 0; i < DirectoryTable.Count; i++)
            {
                if (i != search(de.name.ToString()) && DirectoryTable[i].first_cluster == targetFirstCluster)
                {
                    // Parent directory found
                    parentDirectoryIndex = i;
                    break;
                }
            }

            return parentDirectoryIndex;
        }
        public void DeleteDirectory(_Directory de)
		{
            // Code to delete the directory from the FAT table using a chain

            int directoryIndex = search(de.name.ToString()); // Get the index of the directory entry you want to delete
            int firstCluster = DirectoryTable[directoryIndex].first_cluster; // Get the first cluster of the directory

            // Clear the chain of blocks associated with the directory
            int currentCluster = firstCluster;
            while (currentCluster != -1)
            {
                int nextCluster = FAT_Table.get_value(currentCluster); // Get the next cluster in the chain
                FAT_Table.set_value(currentCluster, 0); // Set the current cluster to 0 to mark it as free
                currentCluster = nextCluster; // Move to the next cluster
            }

            // Code to remove the directory from the parent directory's list
            int parentDirectoryIndex = GetParentDirectoryIndex(de); // Get the index of the parent directory entry
            DirectoryTable.RemoveAt(directoryIndex); //Remove the directory index from the directoryTable
            /*there exist code here*/// Remove the directory index from the parent's list of subdirectories
            FAT_Table.write_fat_table();
        }

    }
}
