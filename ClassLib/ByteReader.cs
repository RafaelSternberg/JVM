using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JVM
{
    internal class ByteReader
    {
        private readonly byte[] data;
        private int position;

        public ByteReader(byte[] data)
        {
            this.data = data;
            position = 0;
        }

        public byte ReadU1()
        {
            if (position >= data.Length)
            {
                throw new IndexOutOfRangeException("Reached end of data");
            }

            return data[position++];
        }

        public ushort ReadU2()
        {
            byte high = ReadU1();
            byte low = ReadU1();
            return (ushort)((high << 8) | low);
        }

        public uint ReadU4()
        {
            byte b1 = ReadU1();
            byte b2 = ReadU1();
            byte b3 = ReadU1();
            byte b4 = ReadU1();
            return (uint)((b1 << 24) | (b2 << 16) | (b3 << 8) | b4);
        }

        public byte[] ReadBytes(int length)
        {
            if (position + length > data.Length) 
            {
                throw new IndexOutOfRangeException("Attempted to read past end of data");
            }

            byte[] result = new byte[length];
            Array.Copy(data, position, result, 0, length);
            position += length;
            return result;
        }

        public byte PeekU1()
        {
            if (position >= data.Length)
            {
                throw new IndexOutOfRangeException("Reached end of data");
            }

            return data[position];
        }

        public bool IsEOF()
        {
            return position >= data.Length;
        }
    }
}
