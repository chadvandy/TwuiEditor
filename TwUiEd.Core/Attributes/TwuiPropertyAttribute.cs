using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwUiEd.Core.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class TwuiPropertyAttribute : System.Attribute
    {
        // Required fields that are in the constructor.
        public string Node;


        // Optional fields in the constructor, w/ default values.
        public string Name = string.Empty;
        public string Description = string.Empty;
        public uint VersionAdded = 0;
        public uint VersionRemoved = 999;
        public bool Required = false;

        public TwuiPropertyAttribute(string node)
        {
            Node = node;
        }
    }
}
