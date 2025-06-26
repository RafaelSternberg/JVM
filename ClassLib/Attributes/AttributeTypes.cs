using ClassLib.FieldsMethodsAttributes;
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
        }

        public class StackMapTableAttribute : AttributeInfo
        {
            public byte[] Info { get; set; }
        }

        public class ExceptionsAttribute : AttributeInfo
        {
            public ushort[] ExceptionIndexTable { get; set; }
        }

        public class InnerClassesAttribute : AttributeInfo
        {
            public InnerClassEntry[] Classes { get; set; }
        }

        public class EnclosingMethodAttribute : AttributeInfo
        {
            public ushort ClassIndex { get; set; }
            public ushort MethodIndex { get; set; }
        }

        public class SyntheticAttribute : AttributeInfo { }

        public class SignatureAttribute : AttributeInfo
        {
            public ushort SignatureIndex { get; set; }
        }

        public class SourceFileAttribute : AttributeInfo
        {
            public ushort SourceFileIndex { get; set; }
        }

        public class SourceDebugExtensionAttribute : AttributeInfo
        {
            public byte[] DebugExtension { get; set; }
        }

        public class LineNumberTableAttribute : AttributeInfo
        {
            public LineNumberEntry[] LineNumberTable { get; set; }
        }

        public class LocalVariableTableAttribute : AttributeInfo
        {
            public LocalVariableEntry[] LocalVariableTable { get; set; }
        }

        public class LocalVariableTypeTableAttribute : AttributeInfo
        {
            public LocalVariableEntry[] LocalVariableTypeTable { get; set; }
        }

        public class DeprecatedAttribute : AttributeInfo { }

        public class RuntimeVisibleAnnotationsAttribute : AttributeInfo
        {
            public byte[] Info { get; set; }
        }

        public class RuntimeInvisibleAnnotationsAttribute : AttributeInfo
        {
            public byte[] Info { get; set; }
        }

        public class RuntimeVisibleParameterAnnotationsAttribute : AttributeInfo
        {
            public byte[] Info { get; set; }
        }

        public class RuntimeInvisibleParameterAnnotationsAttribute : AttributeInfo
        {
            public byte[] Info { get; set; }
        }

        public class AnnotationDefaultAttribute : AttributeInfo
        {
            public byte[] Info { get; set; }
        }

        public class BootstrapMethodsAttribute : AttributeInfo
        {
            public BootstrapMethodEntry[] BootstrapMethods { get; set; }
        }

        public class UnknownAttribute : AttributeInfo
        {
            public byte[] Info { get; set; }
        }
    }
}
