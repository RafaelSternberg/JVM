using ClassLib.ConstantPoolEntries;
using JVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Attributes
{
    public abstract class StackMapFrame
    {
        public byte FrameType { get; }

        protected StackMapFrame(byte frameType)
        {
            FrameType = frameType;
        }

        public static StackMapFrame Read(ByteReader reader, ConstantPoolInfo[] constantPool)
        {
            byte frameType = reader.ReadU1();

            if (frameType <= 63)
            {
                // same_frame
                return new SameFrame(frameType);
            }
            else if (frameType <= 127)
            {
                // same_locals_1_stack_item_frame
                var stack = VerificationTypeInfo.Read(reader, constantPool);
                return new SameLocals1StackItemFrame(frameType, stack);
            }
            else if (frameType <= 246)
            {
                // reserved for future use
                throw new NotSupportedException($"Reserved or unsupported StackMapFrame type: {frameType}");
            }
            else if (frameType == 247)
            {
                // same_locals_1_stack_item_frame_extended
                ushort offsetDelta = reader.ReadU2();
                var stack = VerificationTypeInfo.Read(reader, constantPool);
                return new SameLocals1StackItemFrameExtended(frameType, offsetDelta, stack);
            }
            else if (frameType >= 248 && frameType <= 250)
            {
                // chop_frame
                ushort offsetDelta = reader.ReadU2();
                return new ChopFrame(frameType, offsetDelta);
            }
            else if (frameType == 251)
            {
                // same_frame_extended
                ushort offsetDelta = reader.ReadU2();
                return new SameFrameExtended(frameType, offsetDelta);
            }
            else if (frameType >= 252 && frameType <= 254)
            {
                // append_frame
                ushort offsetDelta = reader.ReadU2();
                int k = frameType - 251;
                var locals = new List<VerificationTypeInfo>();
                for (int i = 0; i < k; i++)
                {
                    locals.Add(VerificationTypeInfo.Read(reader, constantPool));
                }
                return new AppendFrame(frameType, offsetDelta, locals);
            }
            else if (frameType == 255)
            {
                // full_frame
                ushort offsetDelta = reader.ReadU2();
                ushort numberOfLocals = reader.ReadU2();
                var locals = new List<VerificationTypeInfo>();
                for (int i = 0; i < numberOfLocals; i++)
                {
                    locals.Add(VerificationTypeInfo.Read(reader, constantPool));
                }
                ushort numberOfStackItems = reader.ReadU2();
                var stack = new List<VerificationTypeInfo>();
                for (int i = 0; i < numberOfStackItems; i++)
                {
                    stack.Add(VerificationTypeInfo.Read(reader, constantPool));
                }
                return new FullFrame(frameType, offsetDelta, locals, stack);
            }
            else
            {
                throw new NotSupportedException($"Unknown StackMapFrame type: {frameType}");
            }
        }
    }
}
