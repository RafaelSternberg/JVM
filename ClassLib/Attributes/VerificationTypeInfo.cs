using ClassLib.ConstantPoolEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Attributes
{
    // VerificationTypeInfo as per JVM spec
    public abstract class VerificationTypeInfo
    {
        public byte Tag { get; }
        protected VerificationTypeInfo(byte tag) => Tag = tag;

        // Reads a VerificationTypeInfo from the ByteReader
        public static VerificationTypeInfo Read(JVM.ByteReader reader, ConstantPoolInfo[] constantPool)
        {
            byte tag = reader.ReadU1();
            return tag switch
            {
                0 => new TopVariableInfo(),
                1 => new IntegerVariableInfo(),
                2 => new FloatVariableInfo(),
                3 => new DoubleVariableInfo(),
                4 => new LongVariableInfo(),
                5 => new NullVariableInfo(),
                6 => new UninitializedThisVariableInfo(),
                7 => new ObjectVariableInfo(reader.ReadU2()), // cpool_index
                8 => new UninitializedVariableInfo(reader.ReadU2()), // offset
                _ => throw new InvalidOperationException($"Unknown verification type tag: {tag}")
            };
        }

        public class TopVariableInfo : VerificationTypeInfo
        {
            public TopVariableInfo() : base(0) { }
        }

        public class IntegerVariableInfo : VerificationTypeInfo
        {
            public IntegerVariableInfo() : base(1) { }
        }

        public class FloatVariableInfo : VerificationTypeInfo
        {
            public FloatVariableInfo() : base(2) { }
        }

        public class DoubleVariableInfo : VerificationTypeInfo
        {
            public DoubleVariableInfo() : base(3) { }
        }

        public class LongVariableInfo : VerificationTypeInfo
        {
            public LongVariableInfo() : base(4) { }
        }

        public class NullVariableInfo : VerificationTypeInfo
        {
            public NullVariableInfo() : base(5) { }
        }

        public class UninitializedThisVariableInfo : VerificationTypeInfo
        {
            public UninitializedThisVariableInfo() : base(6) { }
        }

        public class ObjectVariableInfo : VerificationTypeInfo
        {
            public ushort CpoolIndex { get; }
            public ObjectVariableInfo(ushort cpoolIndex) : base(7) => CpoolIndex = cpoolIndex;
        }

        public class UninitializedVariableInfo : VerificationTypeInfo
        {
            public ushort Offset { get; }
            public UninitializedVariableInfo(ushort offset) : base(8) => Offset = offset;
        }
    }
}
