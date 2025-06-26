using ClassLib.FieldsMethodsAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib
{
    public class MethodInfo
    {
        public ushort AccessFlags { get; set; }
        public ushort NameIndex { get; set; }
        public ushort DescriptorIndex { get; set; }
        public ushort AttributesCount { get; set; }
        public List<AttributeInfo> Attributes { get; set; } = new();
    }
}
