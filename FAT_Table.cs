using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programming
{
    public static class FAT_Table
    /*This class is used to manage the virtual disk and determine empty and busy blocks plus
       some other utility methods.
     */
    {
        private const int BlockSize = 1024;
        private const int FatSize = 4;
        private static int[] fatTable=new int[BlockSize/*(1024)*/];//default for this array is zero specialized from index 5 to index 1023
        public static void initialize()
        {
            fatTable[0] =-1;
            fatTable[1]=2;
            fatTable[2]=3;
            fatTable[3]=4;
            fatTable[4]=-1;
        }
        public static void write_fat_table()
        /*This method writes the fat table data into the virtual disk (text file)*/

        {
            //Create an array of bytes with size
            byte[] data = new byte[FatSize*BlockSize/*(4096)*/];
            //Open the virtual disk (text file)
            using (FileStream fs = new FileStream("VirtualDisk.txt", FileMode.Open, FileAccess.ReadWrite))
            {
                Buffer.BlockCopy(fatTable, 0, data, 0, fatTable.Length);
                //count assigned for the length of the src array
                //count because we convert the src array to its destination
                //seek()
                fs.Seek(1024,SeekOrigin.Begin);
                //write()
                fs.Write(data, 0, data.Length/*(4096)*/);
            }

            }
        public static void read_fat_table()
        /*This method read the data from the virtual disk (text file) into the fat table array*/

        {
            //Create an array of bytes with size
            byte[] data = new byte[FatSize * BlockSize/*(4096)*/];
            //Open the virtual disk (text file)
            using (FileStream fs = new FileStream("VirtualDisk.txt", FileMode.Open, FileAccess.ReadWrite))
            {
                //seek()
                fs.Seek(1024, SeekOrigin.Begin);
                //read()
                fs.Read(data, 0, data.Length/*(4096)*/);
                Buffer.BlockCopy( data, 0,fatTable,0, data.Length);
                //count assigned for the length of the src array
                //count because we convert the src array to its destination
            }

        }
        public static void print_fat_table()
        /*This method is used just for testing purposes to print all the values in the fat table*/
        {
            /*Loop over all the elements of the fat table
            array and print each element */
            for (int i = 0; i < fatTable.Length; i++)
            {
                Console.WriteLine(fatTable[i]);
            }
        }
        public static int get_available_block()
        /*This method is returns the index of the first empty block*/
        {
            /*Loop over the elements of the fat table
            array*/
            for (int i=0;i<fatTable.Length; i++)
            {
                if (fatTable[i] == 0)
                {  /*Return the first appearance of an empty
                   block?*/
                    return i;
                }
            }
            /*No empty blocks? Return -1*/
            return -1;
            
        }
        public static int get_value(int index) 
        {
            //Return the fat[index]
            return fatTable[index];
        }
        public static void set_value(int index, int value) 
        {
            //Set fat[index] = value
            fatTable[index] = value;
        }
        public static int get_number_of_free_blocks()
        {
            int count = 0;
            /*Loop over the elements of the fat table
            array*/
            for (int i = 0; i < fatTable.Length; i++)
            {
                if (fatTable[i] == 0)
                { 
             /*Count the number of the empty blocks?*/
                   count++;
                }
            }
            return count;
        }
        public static int get_free_space()
        {
            int availableBlocks = 0;
            /*Loop over the elements of the fat table
            array*/
            for (int i = 0; i < fatTable.Length; i++)
            {
                if (fatTable[i] == 0)
                {
                    /*Count the number of the empty blocks?*/
                    availableBlocks++;
                }
            }
            /*returns the sizeof Available blocks= Available blocks* 1024*/
            return availableBlocks * BlockSize;
        }
    }
}
