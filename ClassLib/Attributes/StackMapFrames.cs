using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Attributes
{
    public class SameFrame : StackMapFrame
    {
        public SameFrame(byte frameType) : base(frameType) { }
    }

    public class SameLocals1StackItemFrame : StackMapFrame
    {
        public VerificationTypeInfo Stack { get; }

        public SameLocals1StackItemFrame(byte frameType, VerificationTypeInfo stack)
            : base(frameType)
        {
            Stack = stack;
        }
    }

    public class SameLocals1StackItemFrameExtended : StackMapFrame
    {
        public ushort OffsetDelta { get; }
        public VerificationTypeInfo Stack { get; }

        public SameLocals1StackItemFrameExtended(byte frameType, ushort offsetDelta, VerificationTypeInfo stack)
            : base(frameType)
        {
            OffsetDelta = offsetDelta;
            Stack = stack;
        }
    }

    public class ChopFrame : StackMapFrame
    {
        public ushort OffsetDelta { get; }

        public ChopFrame(byte frameType, ushort offsetDelta)
            : base(frameType)
        {
            OffsetDelta = offsetDelta;
        }
    }

    public class SameFrameExtended : StackMapFrame
    {
        public ushort OffsetDelta { get; }

        public SameFrameExtended(byte frameType, ushort offsetDelta)
            : base(frameType)
        {
            OffsetDelta = offsetDelta;
        }
    }

    public class AppendFrame : StackMapFrame
    {
        public ushort OffsetDelta { get; }
        public IReadOnlyList<VerificationTypeInfo> Locals { get; }

        public AppendFrame(byte frameType, ushort offsetDelta, IReadOnlyList<VerificationTypeInfo> locals)
            : base(frameType)
        {
            OffsetDelta = offsetDelta;
            Locals = locals;
        }
    }

    public class FullFrame : StackMapFrame
    {
        public ushort OffsetDelta { get; }
        public IReadOnlyList<VerificationTypeInfo> Locals { get; }
        public IReadOnlyList<VerificationTypeInfo> Stack { get; }

        public FullFrame(byte frameType, ushort offsetDelta, IReadOnlyList<VerificationTypeInfo> locals, IReadOnlyList<VerificationTypeInfo> stack)
            : base(frameType)
        {
            OffsetDelta = offsetDelta;
            Locals = locals;
            Stack = stack;
        }
    }

    
}
