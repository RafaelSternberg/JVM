using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JVM
{
    public class ByteReader
    {
        private ReadOnlyMemory<byte> data;

        public ByteReader(byte[] data)
        {
            this.data = new ReadOnlyMemory<byte>(data);
        }

        public ByteReader(ReadOnlyMemory<byte> data)
        {
            this.data = data;
        }

        public byte ReadU1()
        {
            if (data.Length < 1)
                throw new IndexOutOfRangeException("Reached end of data");

            byte result = data.Slice(1).Span[0];

            data = data.Slice(1); // Move the reader forward by 1 byte

            return result;
        }

        public ushort ReadU2()
        {
            if(data.Length < 2)
                throw new IndexOutOfRangeException("Not enough data to read a 2-byte value");

            var bytes = data.Slice(0, 2);
            
            data = data.Slice(2); // Move the reader forward by 2 bytes

            return (ushort)(bytes.Span[0] << 8 | bytes.Span[1]);
        }

        public uint ReadU4()
        {
            if (data.Length < 4)
                throw new IndexOutOfRangeException("Not enough data to read a 4-byte value");

            var bytes = data.Slice(0, 4);

            data = data.Slice(4); // Move the reader forward by 4 bytes

            return (uint)(bytes.Span[0] << 24 | bytes.Span[1] << 16 | bytes.Span[2] << 8 | bytes.Span[3]);
        }

        public byte[] ReadBytes(int length)
        {
            if (data.Length < length)
                throw new IndexOutOfRangeException("Attempted to read past end of data");

            byte[] bytes = data.Slice(0, length).ToArray();
            data = data.Slice(length); // Move the reader forward by the length of the bytes read

            return bytes;
        }

        public byte PeekU1()
        {
            if (data.Length < 1)
                throw new IndexOutOfRangeException("Reached end of data");

            return data.Span[0];
        }

        public bool IsEOF()
        {
            return data.Length < 1;
        }

        internal ushort[] ReadU2Array()
        {
            throw new NotImplementedException();
        }
    }
}
