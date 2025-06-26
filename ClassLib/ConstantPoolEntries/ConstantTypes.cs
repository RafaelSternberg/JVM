using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.ConstantPoolEntries
{
    internal class ConstantTypes
    {
        public class ConstantUtf8 : ConstantPoolInfo
        {
            public string Value { get; set; }
            public ConstantUtf8(string value, byte tag = 0) => (Value, Tag) = (value, tag);
        }

        public class ConstantInteger : ConstantPoolInfo
        {
            public int Value { get; set; }
            public ConstantInteger(int value, byte tag = 0) => (Value, Tag) = (value, tag);
        }
        public class ConstantFloat : ConstantPoolInfo
        {
            public float Value { get; set; }
            public ConstantFloat(float value, byte tag = 0) => (Value, Tag) = (value, tag);
        }

        public class ConstantLong : ConstantPoolInfo
        {
            public long Value { get; set; }
            public ConstantLong(long value, byte tag = 0) => (Value, Tag) = (value, tag);
        }

        public class ConstantDouble : ConstantPoolInfo
        {
            public double Value { get; set; }
            public ConstantDouble(double value, byte tag = 0) => (Value, Tag) = (value, tag);
        }

        public class ConstantClass : ConstantPoolInfo
        {
            public ushort NameIndex { get; set; }
            public ConstantClass(ushort nameIndex, byte tag = 0) => (NameIndex, Tag) = (nameIndex, tag);
        }

        public class ConstantString : ConstantPoolInfo
        {
            public ushort StringIndex { get; set; }
            public ConstantString(ushort stringIndex, byte tag = 0) => (StringIndex, Tag) = (stringIndex, tag);
        }

        public class ConstantFieldref : ConstantPoolInfo
        {
            public ushort ClassIndex { get; set; }
            public ushort NameAndTypeIndex { get; set; }
            public ConstantFieldref(ushort classIndex, ushort nameAndTypeIndex, byte tag = 0) =>
                (ClassIndex, NameAndTypeIndex, Tag) = (classIndex, nameAndTypeIndex, tag);
        }

        public class ConstantMethodref : ConstantPoolInfo
        {
            public ushort ClassIndex { get; set; }
            public ushort NameAndTypeIndex { get; set; }
            public ConstantMethodref(ushort classIndex, ushort nameAndTypeIndex, byte tag = 0) =>
                (ClassIndex, NameAndTypeIndex, Tag) = (classIndex, nameAndTypeIndex, tag);
        }

        public class ConstantInterfaceMethodref : ConstantPoolInfo
        {
            public ushort ClassIndex { get; set; }
            public ushort NameAndTypeIndex { get; set; }
            public ConstantInterfaceMethodref(ushort classIndex, ushort nameAndTypeIndex, byte tag = 0) =>
                (ClassIndex, NameAndTypeIndex, Tag) = (classIndex, nameAndTypeIndex, tag);
        }

        public class ConstantNameAndType : ConstantPoolInfo
        {
            public ushort NameIndex { get; set; }
            public ushort DescriptorIndex { get; set; }
            public ConstantNameAndType(ushort nameIndex, ushort descriptorIndex, byte tag = 0) =>
                (NameIndex, DescriptorIndex, Tag) = (nameIndex, descriptorIndex, tag);
        }
        public class ConstantInvokeDynamic : ConstantPoolInfo
        {
            public ushort BootstrapMethodAttrIndex { get; set; }
            public ushort NameAndTypeIndex { get; set; }
            public ConstantInvokeDynamic(ushort bootstrapMethodAttrIndex, ushort nameAndTypeIndex, byte tag = 0) =>
                (BootstrapMethodAttrIndex, NameAndTypeIndex, Tag) = (bootstrapMethodAttrIndex, nameAndTypeIndex, tag);
        }
        public class ConstantMethodHandle : ConstantPoolInfo
        {
            public byte ReferenceKind { get; set; }
            public ushort ReferenceIndex { get; set; }
            public ConstantMethodHandle(byte referenceKind, ushort referenceIndex, byte tag = 0) =>
                (ReferenceKind, ReferenceIndex, Tag) = (referenceKind, referenceIndex, tag);
        }
        public class ConstantMethodType : ConstantPoolInfo
        {
            public ushort DescriptorIndex { get; set; }
            public ConstantMethodType(ushort descriptorIndex, byte tag = 0) => (DescriptorIndex, Tag) = (descriptorIndex, tag);
        }
    }
}