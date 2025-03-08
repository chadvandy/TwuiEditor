using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;


namespace TwUiEd.Core.Services
{
    // Another attempt at the TwuiParser, but specifically using the XmlSerializer system
    // instead of the XmlReader system. Serializer should be clearer for POCO mapping.
    public class TwuiParserSerialized
    {
        public static TestTwuiModel? ParseXmlFile(string xmlFile)
        {
            XmlSerializer xml = new(typeof(TestTwuiModel));
            XmlReader xmlReader = XmlReader.Create(xmlFile);

            TestTwuiModel model;

            if (xml.CanDeserialize(xmlReader))
            {
                Debug.Print("We can deserialize the XML file provided!");

                model = (TestTwuiModel)xml.Deserialize(xmlReader);
                return model;
            }

            return null;
        }
    }

    [XmlRoot("layout", IsNullable = false)]
    public class TestTwuiModel
    {
        [XmlAttribute("version")]
        public uint Version { get; set; }

        [XmlAttribute("comment")]
        public string Comment { get; set; }

        [XmlAttribute("precache_condition")]
        public string PrecacheCondition { get; set; }

        // Sub-Elements of the "root node" in the XML file.

        // TODO how to resemble the "hierarchy" structure using the
        // XML attributes system?!
        //[XmlElement]
        //[XmlArray()]

        // NOTE should this just fully not worry about hierarchy, and that
        // can be read after the entire file is serialized to apply the different
        // child/parent mappings for each component built from this XML file?

        [XmlElement("hierarchy")]
        public TestHierarchyModel Hierarchy { get; set; }

        //[XmlArrayItem(typeof(TestComponentModel))]
        //[XmlArray("components")]
        //public TestComponentModel[] Components { get; set; } = [];

        [XmlElement("components", typeof(TestComponentModel))]
        public TestComponentModel[] Components { get; set; }


        //public TestTwuiLayoutModel Layout { get; set; }
        //public TestTwuiBaseComponentModel Root { get; set; }
    }

    //public class TestComponents
    //{
    //    [XmlArrayItem(typeof(TestComponentModel))]
    //    [XmlArray]
    //    public TestComponentModel[] Components { get; set; }
    //}

    public class TestComponentModel
    {
        [XmlAttribute("this")]
        public string GUID { get; set; }

        [XmlAttribute("id")]
        public string Id { get; set; }
    }

    public class TestHierarchyModel
    {
        // The main hierarchy element that recursively holds all of the hierarchical
        // information of the components within this TWUI file.
        [XmlElement("root")]
        public TestHierarchyNodeModel RootNote { get; set; }
    }


    public class TestHierarchyNodeModel
    {
        //[XmlAnyElement]
        //public XmlElement[] ChildNodes { get; set; }

        [XmlArrayItem(Type = typeof(TestHierarchyNodeModel))]
        [XmlArray]
        public TestHierarchyNodeModel[] ChildNodes { get; set; }

        [XmlAttribute("this")]
        public string GUID { get; set; }
    }

    public class TestTwuiLayoutModel
    {
        public uint? Version { get; set; } = 0;
        public string? Comment { get; set; } = string.Empty;
        public string? PrecacheCondition { get; set; } = string.Empty;
    }

    public class TestTwuiBaseComponentModel
    {
        // Handles the attributes "this" and "uniqueguid"
        public string Guid { get; set; } = string.Empty;
    }
}
