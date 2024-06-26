﻿using Programming;
using System;
using System.IO;
namespace Programming
{
    public static class Virtual_Disk
    /*This class is used to store data or read it from the virtual disk (The text file)*/
    {
        public const int BlockSize = 1024;
        private const int DiskSize = 1024 * 1024;
        private const int FatSize = 4;
        //private static byte[] FAT_Table;
        //public static string Name = @"D:\VirtualDisk.txt";
        public static void Initialize()
        {
           
            if (!File.Exists("VirtualDisk.txt"))
            {
                using (FileStream fs = new FileStream("VirtualDisk.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    for (int i = 0; i < 1024; i++)
                    {   //using WriteByte()
                        fs.WriteByte((byte)'0');
                    }
                    //The following 4 blocks filled (1024 * 4) with ‘*’
                    for (int i = 0; i < FatSize * 1024; i++)
                    {   //using WriteByte()
                        fs.WriteByte((byte)'*');
                    }
                    //And the rest (1019 * 1024) with ‘#’
                    for (int i = 0; i < 1019 * 1024; i++)
                    {   //using WriteByte()
                        fs.WriteByte((byte)'#');
                    }
                }
                /*We will also initialize the fat table (look fat
                table class)*/
                Programming.FAT_Table.initialize();
                //Write fat table (look fat table class)
                Programming.FAT_Table.write_fat_table();
            }
            else
            {   /*If the file already exists:
            We read the fat table (look fat table class)*/
                Programming.FAT_Table.read_fat_table();
                //Program.current_Directory.Read_Directory();
            }
        }

        public static void Write_Block(byte[] data, int index)
        {
            if (data.Length != BlockSize/*(1024)*/)
            {
                throw new ArgumentException("Data length must be equal to block size.");
            }
            if (index < 0 || index >= DiskSize / BlockSize)
            {
                throw new ArgumentOutOfRangeException("Index out of range.");
            }
            ////////////////////////////////
            //Opens the virtual disk (text file)
            using (FileStream fs = new FileStream("VirtualDisk.txt", FileMode.Open, FileAccess.ReadWrite))
            {   //seek()
                fs.Seek(index * BlockSize/*(1024)*/, SeekOrigin.Begin);
                //Write()
                fs.Write(data, 0, BlockSize/*(1024)*/);
            }
            ///////////////////////////////
            //UpdateFatTable(index);
            FAT_Table.write_fat_table();
        }
        public static byte[] Read_Block(int index)
        {
            if (index < 0 || index >= DiskSize / BlockSize)
            {
                throw new ArgumentOutOfRangeException("Index out of range.");
            }
            byte[] data = new byte[BlockSize/*(1024)*/];
            //Opens the virtual disk (text file)
            using (FileStream fs = new FileStream("VirtualDisk.txt", FileMode.Open, FileAccess.ReadWrite))
            {     //Seek()
                fs.Seek(index * BlockSize/*(1024)*/, SeekOrigin.Begin);
                //Read()
                fs.Read(data, 0, BlockSize/*(1024)*/);
            }
            //Return data
            return data;
        }

        //private static byte[] ReadFatTable()
        //{
        //    //byte[] fatTable = new byte[FatSize * BlockSize];
        //    //using (FileStream fs = new FileStream("VirtualDisk.txt", FileMode.Open))
        //    //{
        //    //    fs.Seek(0, SeekOrigin.Begin);
        //    //    fs.Read(fatTable, 0, FatSize * BlockSize);
        //    //}
        //    //return fatTable;
        //    FAT_Table read_fat_table();
        //}

        //    private static void WriteFatTable()
        //{
        //    //using (FileStream fs = new FileStream("VirtualDisk.txt", FileMode.Open))
        //    //{
        //    //    fs.Seek(0, SeekOrigin.Begin);
        //    //    fs.Write(FAT_Table.fatTable, 0, FatSize * BlockSize);
        //    //}
        //    FAT_Table.write_fat_table();
        //}

        //private static void UpdateFatTable(int index)
        //{
        //    FAT_Table[index / 8] |= (byte)(1 << (index % 8));
        //}
    }
}