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
            Magic = (uint)reader.ReadU4Array();
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

                ushort attributesCount = reader.ReadU2();
                field.Attributes = new AttributeInfo[attributesCount];
                for (int j = 0; j < attributesCount; j++)
                {
                    field.Attributes[j] = ReadAttribute();
                }

                Fields[i] = field;
            }

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

        private static readonly string[] KnownAttributeNames =
        {
            "ConstantValue",
            "Code",
            "StackMapTable",
            "Exceptions",
            "InnerClasses",
            "EnclosingMethod",
            "Synthetic",
            "Signature",
            "SourceFile",
            "SourceDebugExtension",
            "LineNumberTable",
            "LocalVariableTable",
            "LocalVariableTypeTable",
            "Deprecated",
            "RuntimeVisibleAnnotations",
            "RuntimeInvisibleAnnotations",
            "RuntimeVisibleParameterAnnotations",
            "RuntimeInvisibleParameterAnnotations",
            "AnnotationDefault",
            "BootstrapMethods"
};

        private AttributeInfo ReadAttribute()
        {
            ushort nameIndex = reader.ReadU2();
            uint length = reader.ReadU4();
            string name = ((ConstantTypes.ConstantUtf8)ConstantPool[nameIndex]).Value;

            int attrIndex = Array.IndexOf(KnownAttributeNames, name);

            return attrIndex switch
            {
                0 => new ConstantValueAttribute
                {
                    AttributeNameIndex = nameIndex,
                    AttributeLength = length,
                    ConstantValueIndex = reader.ReadU2()
                },

                1 => new CodeAttribute
                {
                    AttributeNameIndex = nameIndex,
                    AttributeLength = length,
                    MaxStack = reader.ReadU2(),
                    MaxLocals = reader.ReadU2(),
                    Code = reader.ReadBytes(4), //need to be array
                    ExceptionTable = ExceptionTableEntry.ReadExceptionTable(reader),
                    Attributes = ReadAttributeList()
                },

                2 => new StackMapTableAttribute
                {
                    AttributeNameIndex = nameIndex,
                    AttributeLength = length,
                    Info = reader.ReadBytes((int)length)
                },

                3 => new ExceptionsAttribute
                {
                    AttributeNameIndex = nameIndex,
                    AttributeLength = length,
                    ExceptionIndexTable = reader.ReadU2Array() //need array
                },

                4 => new InnerClassesAttribute
                {
                    AttributeNameIndex = nameIndex,
                    AttributeLength = length,
                    Classes = ReadInnerClassEntries()
                },

                5 => new EnclosingMethodAttribute
                {
                    AttributeNameIndex = nameIndex,
                    AttributeLength = length,
                    ClassIndex = reader.ReadU2(),
                    MethodIndex = reader.ReadU2()
                },

                6 => new SyntheticAttribute { AttributeNameIndex = nameIndex, AttributeLength = length },

                7 => new SignatureAttribute
                {
                    AttributeNameIndex = nameIndex,
                    AttributeLength = length,
                    SignatureIndex = reader.ReadU2()
                },

                8 => new SourceFileAttribute
                {
                    AttributeNameIndex = nameIndex,
                    AttributeLength = length,
                    SourceFileIndex = reader.ReadU2()
                },

                9 => new SourceDebugExtensionAttribute
                {
                    AttributeNameIndex = nameIndex,
                    AttributeLength = length,
                    DebugExtension = reader.ReadBytes((int)length)
                },

                10 => new LineNumberTableAttribute
                {
                    AttributeNameIndex = nameIndex,
                    AttributeLength = length,
                    LineNumberTable = LineNumberEntry.ReadLineNumberTable(reader)
                },

                11 => new LocalVariableTableAttribute
                {
                    AttributeNameIndex = nameIndex,
                    AttributeLength = length,
                    LocalVariableTable = LocalVariableEntry.ReadLocalVariableTable(reader)
                },

                12 => new LocalVariableTypeTableAttribute
                {
                    AttributeNameIndex = nameIndex,
                    AttributeLength = length,
                    LocalVariableTypeTable = LocalVariableEntry.ReadLocalVariableTable(reader)
                },

                13 => new DeprecatedAttribute { AttributeNameIndex = nameIndex, AttributeLength = length },

                14 => new RuntimeVisibleAnnotationsAttribute
                {
                    AttributeNameIndex = nameIndex,
                    AttributeLength = length,
                    Info = reader.ReadBytes((int)length)
                },

                15 => new RuntimeInvisibleAnnotationsAttribute
                {
                    AttributeNameIndex = nameIndex,
                    AttributeLength = length,
                    Info = reader.ReadBytes((int)length)
                },

                16 => new RuntimeVisibleParameterAnnotationsAttribute
                {
                    AttributeNameIndex = nameIndex,
                    AttributeLength = length,
                    Info = reader.ReadBytes((int)length)
                },

                17 => new RuntimeInvisibleParameterAnnotationsAttribute
                {
                    AttributeNameIndex = nameIndex,
                    AttributeLength = length,
                    Info = reader.ReadBytes((int)length)
                },

                18 => new AnnotationDefaultAttribute
                {
                    AttributeNameIndex = nameIndex,
                    AttributeLength = length,
                    Info = reader.ReadBytes((int)length)
                },

                19 => new BootstrapMethodsAttribute
                {
                    AttributeNameIndex = nameIndex,
                    AttributeLength = length,
                    BootstrapMethods = BootstrapMethodEntry.ReadBootstrapMethods(reader)
                },

                _ => throw new Exception("invalid attribute")
            };
        }
    }
}
