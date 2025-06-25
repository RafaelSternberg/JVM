using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.ConstantPoolEntries
{
    internal class ConsantTypes
    {
        public class ConstantUtf8 : ConstantPoolEntry
        {
            public string Value { get; set; }
        }

        public class ConstantInteger : ConstantPoolEntry
        {
            public int Value { get; set; }
        }

        public class ConstantFloat : ConstantPoolEntry
        {
            public float Value { get; set; }
        }

        public class ConstantLong : ConstantPoolEntry
        {
            public long Value { get; set; }
        }

        public class ConstantDouble : ConstantPoolEntry
        {
            public double Value { get; set; }
        }

        public class ConstantClass : ConstantPoolEntry
        {
            public ushort NameIndex { get; set; }
        }

        public class ConstantString : ConstantPoolEntry
        {
            public ushort StringIndex { get; set; }
        }

        public class ConstantFieldref : ConstantPoolEntry
        {
            public ushort ClassIndex { get; set; }
            public ushort NameAndTypeIndex { get; set; }
        }

        public class ConstantMethodref : ConstantPoolEntry
        {
            public ushort ClassIndex { get; set; }
            public ushort NameAndTypeIndex { get; set; }
        }

        public class ConstantInterfaceMethodref : ConstantPoolEntry
        {
            public ushort ClassIndex { get; set; }
            public ushort NameAndTypeIndex { get; set; }
        }

        public class ConstantNameAndType : ConstantPoolEntry
        {
            public ushort NameIndex { get; set; }
            public ushort DescriptorIndex { get; set; }
        }
    }
}
