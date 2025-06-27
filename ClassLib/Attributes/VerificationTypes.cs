using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Attributes
{
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

    public class LongVariableInfo : VerificationTypeInfo
    {
        public LongVariableInfo() : base(4) { }
    }

    public class DoubleVariableInfo : VerificationTypeInfo
    {
        public DoubleVariableInfo() : base(3) { }
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
        public ObjectVariableInfo(ushort cpoolIndex) : base(7)
        {
            CpoolIndex = cpoolIndex;
        }
    }

    public class UninitializedVariableInfo : VerificationTypeInfo
    {
        public ushort Offset { get; }
        public UninitializedVariableInfo(ushort offset) : base(8)
        {
            Offset = offset;
        }
    }
}
