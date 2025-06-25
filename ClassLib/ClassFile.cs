using JVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLib.ConstantPoolEntries;

namespace ClassLib
{
    public class ClassFile
    {
        public uint Magic { get; private set; }
        public ushort MinorVersion { get; private set; }
        public ushort MajorVersion { get; private set; }

        public ConstantPoolEntry[] ConstantPool { get; private set; }

        public ushort AccessFlags { get; private set; }
        public ushort ThisClass { get; private set; }
        public ushort SuperClass { get; private set; }

        public ushort[] Interfaces { get; private set; }

        private ByteReader reader;

        public ClassFile(byte[] classData)
        {
            reader = new ByteReader(classData);
            Parse();
        }

        private void Parse()
        {
            // 1. Magic Number (0xCAFEBABE)
            Magic = reader.ReadU4();
            if (Magic != 0xCAFEBABE)
            {
                throw new InvalidOperationException("Invalid class file magic number.");
            } 

            // 2. Version
            MinorVersion = reader.ReadU2();
            MajorVersion = reader.ReadU2();

            // 3. Constant Pool Count
            ushort constantPoolCount = reader.ReadU2();
            ConstantPool = new ConstantPoolEntry[constantPoolCount];

            //Continue parsing tomorrow
        }
    }
}
