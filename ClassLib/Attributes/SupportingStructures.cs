using JVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Attributes
{
    public class ExceptionTableEntry
    {
        public ushort StartPc { get; set; }
        public ushort EndPc { get; set; }
        public ushort HandlerPc { get; set; }
        public ushort CatchType { get; set; }

        public static ExceptionTableEntry[] ReadExceptionTable(ByteReader reader)
        {
            ushort count = reader.ReadU2();
            var array = new ExceptionTableEntry[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = new ExceptionTableEntry
                {
                    StartPc = reader.ReadU2(),
                    EndPc = reader.ReadU2(),
                    HandlerPc = reader.ReadU2(),
                    CatchType = reader.ReadU2()
                };
            }
            return array;
        }
    }

    public class LineNumberEntry
    {
        public ushort StartPc { get; set; }
        public ushort LineNumber { get; set; }

        public static LineNumberEntry[] ReadLineNumberTable(ByteReader reader)
        {
            ushort count = reader.ReadU2();
            var array = new LineNumberEntry[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = new LineNumberEntry
                {
                    StartPc = reader.ReadU2(),
                    LineNumber = reader.ReadU2()
                };
            }
            return array;
        }
    }

    public class LocalVariableEntry
    {
        public ushort StartPc { get; set; }
        public ushort Length { get; set; }
        public ushort NameIndex { get; set; }
        public ushort DescriptorIndex { get; set; }
        public ushort Index { get; set; }

        public static LocalVariableEntry[] ReadLocalVariableTable(ByteReader reader)
        {
            ushort count = reader.ReadU2();
            var array = new LocalVariableEntry[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = new LocalVariableEntry
                {
                    StartPc = reader.ReadU2(),
                    Length = reader.ReadU2(),
                    NameIndex = reader.ReadU2(),
                    DescriptorIndex = reader.ReadU2(),
                    Index = reader.ReadU2()
                };
            }
            return array;
        }
    }

    public class InnerClassEntry
    {
        public ushort InnerClassInfoIndex { get; set; }
        public ushort OuterClassInfoIndex { get; set; }
        public ushort InnerNameIndex { get; set; }
        public ushort InnerClassAccessFlags { get; set; }

        public static InnerClassEntry[] ReadInnerClassEntries(ByteReader reader)
        {
            ushort count = reader.ReadU2();
            var array = new InnerClassEntry[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = new InnerClassEntry
                {
                    InnerClassInfoIndex = reader.ReadU2(),
                    OuterClassInfoIndex = reader.ReadU2(),
                    InnerNameIndex = reader.ReadU2(),
                    InnerClassAccessFlags = reader.ReadU2()
                };
            }
            return array;
        }
    }

    public class BootstrapMethodEntry
    {
        public ushort BootstrapMethodRef { get; set; }
        public ushort[] BootstrapArguments { get; set; }

        public static BootstrapMethodEntry[] ReadBootstrapMethods(ByteReader reader)
        {
            ushort count = reader.ReadU2();
            var array = new BootstrapMethodEntry[count];
            for (int i = 0; i < count; i++)
            {
                ushort methodRef = reader.ReadU2();
                ushort argCount = reader.ReadU2();
                var args = new ushort[argCount];
                for (int j = 0; j < argCount; j++)
                    args[j] = reader.ReadU2();

                array[i] = new BootstrapMethodEntry
                {
                    BootstrapMethodRef = methodRef,
                    BootstrapArguments = args
                };
            }
            return array;
        }
    }

}

