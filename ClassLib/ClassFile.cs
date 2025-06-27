using ClassLib.Attributes;
using ClassLib.ConstantPoolEntries;
using ClassLib.FieldsMethodsAttributes;
using JVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClassLib.Attributes.AttributeTypes;

namespace ClassLib
{
    public class ClassFile
    {
        public uint Magic { get; private set; }
        public ushort MinorVersion { get; private set; }
        public ushort MajorVersion { get; private set; }

        public ConstantPoolInfo[] ConstantPool { get; private set; }

        public ushort AccessFlags { get; private set; }
        public ushort ThisClass { get; private set; }
        public ushort SuperClass { get; private set; }

        public ushort[] Interfaces { get; private set; }

        public FieldInfo[] Fields { get; private set; }

        public MethodInfo[] Methods { get; private set; }

        public AttributeInfo[] Attributes { get; private set; }

        private ByteReader reader;

        public ClassFile(byte[] classData)
        {
            reader = new ByteReader(classData);
            Parse();
        }

        private void Parse()
        {
            // 1. Magic Number (0xCAFEBABE)
            Magic = (uint)reader.ReadU4();
            if (Magic != 0xCAFEBABE)
            {
                throw new InvalidOperationException("Invalid class file magic number.");
            }

            // 2. Version
            MinorVersion = reader.ReadU2();
            MajorVersion = reader.ReadU2();

            // 3. Constant Pool Count and Entries
            ushort constantPoolCount = reader.ReadU2();
            ConstantPool = new ConstantPoolInfo[constantPoolCount];

            for (int i = 1; i < constantPoolCount; i++) // Start from 1 as index 0 is reserved
            {
                ConstantPool[i] = ReadConstantPoolEntry();
            }

            // 5. Access Flags
            AccessFlags = reader.ReadU2();

            // 6. This Class and Super Class

            ThisClass = reader.ReadU2();
            SuperClass = reader.ReadU2();

            // 7. Interfaces

            ushort interfacesCount = reader.ReadU2();

            Interfaces = new ushort[interfacesCount];
            for (int i = 0; i < interfacesCount; i++)
            {
                Interfaces[i] = reader.ReadU2();
            }

            //8. Fields

            ushort fieldsCount = reader.ReadU2();
            FieldInfo[] fields = new FieldInfo[fieldsCount];
            for (int i = 0; i < fieldsCount; i++)
            {
                var field = new FieldInfo
                {
                    AccessFlags = reader.ReadU2(),
                    NameIndex = reader.ReadU2(),
                    DescriptorIndex = reader.ReadU2()
                };

                ushort attributesCountFields = reader.ReadU2();
                field.Attributes = new AttributeInfo[attributesCountFields];
                for (int j = 0; j < attributesCountFields; j++)
                {
                    field.Attributes[j] = ReadAttribute(reader, ConstantPool);
                }

                Fields[i] = field;
            }

            //9. Methods

            ushort methodsCount = reader.ReadU2();
            MethodInfo[] methods = new MethodInfo[methodsCount];
            for (int i = 0; i < methodsCount; i++)
            {
                var method = new MethodInfo
                {
                    AccessFlags = reader.ReadU2(),
                    NameIndex = reader.ReadU2(),
                    DescriptorIndex = reader.ReadU2()
                };
                ushort attributesCountMethods = reader.ReadU2();
                method.Attributes = new AttributeInfo[attributesCountMethods];
                for (int j = 0; j < attributesCountMethods; j++)
                {
                    method.Attributes[j] = ReadAttribute(reader, ConstantPool);
                }
                Methods[i] = method;
            }

            // 10. Attributes

            ushort attributesCount = reader.ReadU2();
            Attributes = new AttributeInfo[attributesCount];
            for (int i = 0; i < attributesCount; i++)
            {
                Attributes[i] = ReadAttribute(reader, ConstantPool);
            }

        }
        public static AttributeInfo ReadAttribute(ByteReader reader, ConstantPoolInfo[] ConstantPool)
        {
            ushort attributeNameIndex = reader.ReadU2();
            uint attributeLength = reader.ReadU4();

            var utf8Entry = ConstantPool[attributeNameIndex] as ConstantTypes.ConstantUtf8;
            if (utf8Entry == null)
                throw new InvalidOperationException($"Attribute name at index {attributeNameIndex} is not a UTF8 entry.");

            string attributeName = utf8Entry.Value;

            AttributeInfo attribute;

            switch (attributeName)
            {
                case "ConstantValue":
                    attribute = new ConstantValueAttribute
                    {
                        AttributeNameIndex = attributeNameIndex,
                        AttributeLength = attributeLength,
                        ConstantValueIndex = reader.ReadU2()
                    };
                    break;

                case "Code":
                    attribute = CodeAttribute.Read(reader, attributeNameIndex, attributeLength, ConstantPool);
                    break;

                case "StackMapTable":
                    attribute = StackMapTableAttribute.Read(reader, attributeNameIndex, attributeLength, ConstantPool);
                    break;

                case "Exceptions":
                    attribute = ExceptionsAttribute.Read(reader, attributeNameIndex, attributeLength);
                    break;

                case "BootstrapMethods":
                    attribute = BootstrapMethodsAttribute.Read(reader, attributeNameIndex, attributeLength);
                    break;

                default:
                    throw new Exception($"Unknown attribute name: {attributeName} at index {attributeNameIndex}");
            }

            return attribute;
        }

        private ConstantPoolInfo ReadConstantPoolEntry()
        {
            byte tag = reader.ReadU1();
            ConstantPoolInfo entry = tag switch
            {
                1 => new ConstantTypes.ConstantUtf8(Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadU2())), tag),
                3 => new ConstantTypes.ConstantInteger((int)reader.ReadU4(), tag),
                4 => new ConstantTypes.ConstantFloat(BitConverter.ToSingle(reader.ReadBytes(4), 0), tag),
                5 => new ConstantTypes.ConstantLong(BitConverter.ToInt64(reader.ReadBytes(8), 0), tag),
                6 => new ConstantTypes.ConstantDouble(BitConverter.ToDouble(reader.ReadBytes(8), 0), tag),
                7 => new ConstantTypes.ConstantClass(reader.ReadU2(), tag),
                8 => new ConstantTypes.ConstantString(reader.ReadU2(), tag),
                9 => new ConstantTypes.ConstantFieldref(reader.ReadU2(), reader.ReadU2(), tag),
                10 => new ConstantTypes.ConstantMethodref(reader.ReadU2(), reader.ReadU2(), tag),
                11 => new ConstantTypes.ConstantInterfaceMethodref(reader.ReadU2(), reader.ReadU2(), tag),
                12 => new ConstantTypes.ConstantNameAndType(reader.ReadU2(), reader.ReadU2(), tag),
                15 => new ConstantTypes.ConstantMethodHandle(reader.ReadU1(), reader.ReadU2(), tag),
                16 => new ConstantTypes.ConstantMethodType(reader.ReadU2(), tag),
                18 => new ConstantTypes.ConstantInvokeDynamic(reader.ReadU2(), reader.ReadU2(), tag),
                _ => throw new InvalidOperationException($"Unknown constant pool tag: {tag}")
            };
            entry.Tag = tag;
            return entry;
        }
    }
}
