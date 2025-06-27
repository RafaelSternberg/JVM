using ClassLib.ConstantPoolEntries;
using ClassLib.FieldsMethodsAttributes;
using JVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Attributes
{
    internal class AttributeTypes
    {
        public class ConstantValueAttribute : AttributeInfo
        {
            public ushort ConstantValueIndex { get; set; }
        }

        public class CodeAttribute : AttributeInfo
        {
            public ushort MaxStack { get; set; }
            public ushort MaxLocals { get; set; }
            public byte[] Code { get; set; }
            public ExceptionTableEntry[] ExceptionTable { get; set; }
            public AttributeInfo[] Attributes { get; set; }

            public static CodeAttribute Read(ByteReader reader, ushort attributeNameIndex, uint attributeLength, ConstantPoolInfo[] constantPool)
            {
                var attr = new CodeAttribute
                {
                    AttributeNameIndex = attributeNameIndex,
                    AttributeLength = attributeLength,
                    MaxStack = reader.ReadU2(),
                    MaxLocals = reader.ReadU2()
                };

                uint codeLength = reader.ReadU4();
                attr.Code = reader.ReadBytes((int)codeLength);

                ushort exceptionTableLength = reader.ReadU2();
                attr.ExceptionTable = ExceptionTableEntry.ReadExceptionTable(reader);

                ushort attributesCount = reader.ReadU2();
                attr.Attributes = new AttributeInfo[attributesCount];
                for (int i = 0; i < attributesCount; i++)
                {
                    attr.Attributes[i] = ClassFile.ReadAttribute(reader, constantPool);
                }

                return attr;
            }
        }

        public class StackMapTableAttribute : AttributeInfo
        {
            public StackMapFrame[] Entries { get; set; }

            public static StackMapTableAttribute Read(ByteReader reader, ushort attributeNameIndex, uint attributeLength, ConstantPoolInfo[] constantPool)
            {
                var attr = new StackMapTableAttribute
                {
                    AttributeNameIndex = attributeNameIndex,
                    AttributeLength = attributeLength
                };

                ushort numberOfEntries = reader.ReadU2();
                attr.Entries = new StackMapFrame[numberOfEntries];
                for (int i = 0; i < numberOfEntries; i++)
                {
                    attr.Entries[i] = StackMapFrame.Read(reader, constantPool);
                }
                return attr;
            }
        }

        public class ExceptionsAttribute : AttributeInfo
        {
            public ushort[] ExceptionIndexTable { get; set; }

            public static ExceptionsAttribute Read(ByteReader reader, ushort attributeNameIndex, uint attributeLength)
            {
                var attr = new ExceptionsAttribute
                {
                    AttributeNameIndex = attributeNameIndex,
                    AttributeLength = attributeLength
                };
                ushort numberOfExceptions = reader.ReadU2();
                attr.ExceptionIndexTable = new ushort[numberOfExceptions];
                for (int i = 0; i < numberOfExceptions; i++)
                {
                    attr.ExceptionIndexTable[i] = reader.ReadU2();
                }
                return attr;
            }
        }
        public class BootstrapMethodsAttribute : AttributeInfo
        {
            public BootstrapMethodEntry[] BootstrapMethods { get; set; }

            public static BootstrapMethodsAttribute Read(ByteReader reader, ushort attributeNameIndex, uint attributeLength)
            {
                var attr = new BootstrapMethodsAttribute
                {
                    AttributeNameIndex = attributeNameIndex,
                    AttributeLength = attributeLength
                };
                ushort numBootstrapMethods = reader.ReadU2();
                attr.BootstrapMethods = BootstrapMethodEntry.ReadBootstrapMethods(reader);
                return attr;
            }
        }
    }
}
