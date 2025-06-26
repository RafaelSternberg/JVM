using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.FieldsMethodsAttributes
{
    public abstract class AttributeInfo
    {
        public ushort AttributeNameIndex { get; set; }
        public uint AttributeLength { get; set; }
    }
}
