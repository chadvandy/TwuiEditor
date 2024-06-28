using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwUiEd.Core.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class TwuiElementAttribute : System.Attribute
    {
        // Required fields that are in the constructor.
        // Node: the expected name of the nodes in the XML file.
        // Type: the expected type of the value in the XML file.
        public string Node;
        public Type Type;

        // Optional fields in the constructor, w/ default values.
        public string Name = string.Empty;
        public string Description = string.Empty;
        public uint VersionAdded = 0;
        public uint VersionRemoved = 999;
        public bool Required = false;
        public bool IsList = false;

        public TwuiElementAttribute(string node, Type type)
        {
            Node = node;
            Type = type;
        }
    }
}
