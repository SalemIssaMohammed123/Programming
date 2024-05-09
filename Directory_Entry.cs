using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programming
{
	public class Directory_Entry
	{
		public char[] name = new char[11]; // 11 byte
		public byte attribute;
		public byte[] empty =new byte[12]; // 12 byte
		public int size;//4 byte
		public int first_cluster;//4 byte
		//total 32 byte
		public Directory_Entry() { }
		public Directory_Entry(string n, byte attr, int siz ,int fc)
		{
			attribute = attr;
			if(attribute == 0)//file
			{
				if (n.Length > 11)
				{
					name = (n.Substring(7) + n.Substring(n.Length - 4)).ToCharArray();
				}
				else
				{
					name=n.ToCharArray();
				}

			}
            else //folder
            {
				name=(n.Substring(0,Math.Min(11,n.Length))).ToCharArray();
			}
			size = siz;
			if (fc == 0)
			{
				first_cluster = FAT_Table.get_available_block();
			}
			else// specially in case creating folder root
			{
				first_cluster = fc;
			}

        }
		public byte[] Convert_Directory_Entry_TobyteArray()
		{
			byte[] data=new byte[32];//Directory entry data
			for(int i=0; i < 11; i++)//0:11
			{
				data[i] = Convert.ToByte(name[i]);
			}
			data[11] = attribute;//12
			for (int i = 0; i < 12; i++)//12:23
			{
				data[i+12] = empty[i];
			}
			byte[] temp=new byte[4];
			temp = BitConverter.GetBytes(size);
			for (int i = 0; i < 4; i++)//23:27
			{
				data[i+24] = temp[i];
			}
			temp = BitConverter.GetBytes(first_cluster);
			for (int i = 0; i < 4; i++)//28:31
			{
				data[i + 28] = temp[i];
			}
			return data;
		}
		public Directory_Entry Get_Directory_Entry(byte[] data)
		{
			Directory_Entry de =new Directory_Entry();
			for (int i = 0; i < 11; i++)
			{
				de.name[i] = Convert.ToChar(data[i]);
			}
			de.attribute = data[12];
			for (int i = 0; i < 12; i++)
			{
				de.empty[i] = data[i + 12];
			}
			byte[] temp = new byte[4];
			for (int i = 0; i < 4; i++)
			{
				temp[i] = data[i+24];
			}
			de.size=BitConverter.ToInt32(temp, 0);
			for (int i = 0; i < 4; i++)
			{
				temp[i] = data[i + 28];
			}
			de.first_cluster = BitConverter.ToInt32(temp, 0);
			return de;
		}

	}
}
