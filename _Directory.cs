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
        public List<Directory_Entry> DirectoryTable;
		public _Directory()
		{
		}

		public _Directory(string n, byte attr, int siz, int fc,_Directory parent) : base( n,  attr, siz,  fc)
		{
            DirectoryTable = new List<Directory_Entry>();


            if (parent != null)
            {
                this.parent = parent;
            }
            if (first_cluster != 0)
                fc = first_cluster;
            else
            {
                fc = FAT_Table.get_available_block();
                first_cluster = fc;
            }
        }
        /*public void Write_Directory() {
    // Converts `DirectoryTable` to bytes and stores them,Wirte in Virual and
    // Fat

    List<byte> directoryBytes = new List<byte>();

    // Convert each entry to bytes and store in directoryBytes
    foreach (var entry in DirectoryTable) {
      directoryBytes.AddRange(entry.Convert_Directory_Entry());
    }

    int totalBlocksNeeded =
        (int)Math.Ceiling((double)directoryBytes.Count / VirtualDisk.BlockSize);
    int fullBlocks = directoryBytes.Count / VirtualDisk.BlockSize;
    int remainingBytes = directoryBytes.Count % VirtualDisk.BlockSize;

    int fc;
    if (first_cluster != 0)
      fc = first_cluster;
    else {
      fc = Fat_Table.GetAvailableBlock();
      first_cluster = fc;
    }
    int nc = -1;
    for (int i = 0; i < totalBlocksNeeded; i++) {
      byte[] blockData; // representing the current block
      if (i <
          fullBlocks) // it means that the current block is not the last block
      {
        blockData =
            directoryBytes
                .GetRange(i * VirtualDisk.BlockSize, VirtualDisk.BlockSize)
                .ToArray();
      } else // last block
      {
        blockData = new byte[VirtualDisk.BlockSize];
        directoryBytes.CopyTo(i * VirtualDisk.BlockSize, blockData, 0,
                              remainingBytes);
        for (int j = remainingBytes; j < VirtualDisk.BlockSize; j++) {
          blockData[j] = 0xFF; // Fill remaining bytes with some value
        }
        nc = -1;
      }
      VirtualDisk.WriteBlock(blockData, fc);
      Fat_Table.SetValue(fc, nc);
      nc = fc;
      fc = Fat_Table.GetAvailableBlock();
    }

    Fat_Table.WriteFatTable();
  }*/
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
            int fc;
            if (first_cluster != 0)
                fc = first_cluster;
            else
            {
                fc = FAT_Table.get_available_block();
                first_cluster = fc;
            }
            int nc = -1;
			int totalBlocks = (int)Math.Ceiling(array2.Count / 1024.0);
			int fullBlocks = array2.Count / 1024;
			int remainderBlocks = array2.Count % 1024;
			//creat an array to make the remainder block as fullblock
			byte[] reserved = new byte[1024 - remainderBlocks];
			byte[] totalData = array2.ToArray();
			totalData = totalData.Concat(reserved).ToArray();
            for (int i = 0; i < totalBlocks; i++)
            {
                byte[] blockData; // representing the current block
                if (i <
                    fullBlocks) // it means that the current block is not the last block
                {
                    blockData = array2
                            .GetRange(i * Virtual_Disk.BlockSize, Virtual_Disk.BlockSize)
                            .ToArray();

                }
                else // last block
                {
                    blockData = new byte[Virtual_Disk.BlockSize];
                    array2.CopyTo(i * Virtual_Disk.BlockSize, blockData, 0,
                                          remainderBlocks);
                    for (int j = remainderBlocks; j < Virtual_Disk.BlockSize; j++)
                    {
                        blockData[j] = 0xFF; // Fill remaining bytes with some value
                    }
                    nc = -1;
                }
                Virtual_Disk.Write_Block(blockData, fc);
                FAT_Table.set_value(fc, nc);
                nc = fc;
                fc = FAT_Table.get_available_block();
            }
            FAT_Table.write_fat_table();
		}
		public void Read_Directory()
		{
            //create list of bytes
            List<byte> list = new List<byte>();
            this.DirectoryTable.Clear();
			int fc = first_cluster;
            if (fc == 0)
            {
                return;
            }
            //int nc = FAT_Table.get_value(fc);
			while(fc != -1)
			{
                list.AddRange(Virtual_Disk.Read_Block(fc));
				fc=FAT_Table.get_value(fc);
                for (int i = 0; i < Virtual_Disk.BlockSize;
                    i += Directory_Entry.EntrySize)
                { // Adds the parsed Directory_Entry
                  // object to the DirectoryTable.
                    List<byte> entryBytes =
                        list.GetRange(i, Directory_Entry.EntrySize).ToList();
                    for (int j = 0; j < entryBytes.Count; j += Directory_Entry.EntrySize)
                    {
                        Directory_Entry entry = new Directory_Entry();
                        entry.Get_Directory_Entry(entryBytes.ToArray());
                        if (entryBytes.GetRange(j, Directory_Entry.EntrySize)
                                .All(x => x == 0xFF))
                        {
                            break;
                        }
                        DirectoryTable.Add(entry);
                    }
                }
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
                if (directoryName == name.TrimEnd(' '))

                {
					//Return the index if both names are identical
					return i;
                }
            }
			return -1;

		}
       
        public void DeleteDirectory()
		{
            if (first_cluster != 0)
            {
                int fatindex = first_cluster;
                int next = FAT_Table.get_value(fatindex);
                do
                {
                    FAT_Table.set_value(fatindex, 0);
                    fatindex = next;
                    if (fatindex != -1)
                    {
                        next = FAT_Table.get_value(fatindex);

                    }
                }
                while (next != -1);
            }

            if (parent != null)
            {
                parent.Read_Directory();

                string y = new string(name);


                int i = parent.search(y);
                if (i != -1)
                {
                    parent.DirectoryTable.RemoveAt(i);
                    parent.Write_Directory();
                }


            }
            FAT_Table.write_fat_table();
        }

        public void update(Directory_Entry d)
        {
            Read_Directory();
            int index = search(new string(d.name));


            if (index != -1)
            {
                DirectoryTable.RemoveAt(index);
                DirectoryTable.Insert(index, d);
            }
        }

    }
}
