using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using TwUiEd.Core.Models;

namespace TwUiEd.Core.Services
{
    public class TwuiParser
    {
        private static XmlReaderSettings DefaultSettings = new() { Async = true };
        
        private static void ParseHierarchy(XElement Node, TwuiComponentModel Component)
        {
            // Parse the Hierarchy, starting at our first node - the root - and then working
            // our way down to the lowest component. We just need to parse this node's
            // name, GUID, then grab its children, create a new component model for each of them, assign
            // this component as its parent, and parse that node, until we're done the hierarchy.

            Component.Guid = (string)Node.Attribute("this");
            Component.Name = Node.Name.LocalName;

            // Loop through and create all of our children!
            // Node.Elements() only grab our direct child elements, not their descendants (which will be parsed
            // during their parent's loop).
            IEnumerable<XElement> Children = Node.Elements();
            foreach(XElement ChildNode in Children)
            {
                // Create our child, then send them back to be parsed!
                var ChildComponent = new TwuiComponentModel();
                ChildComponent.Parent = Component;
                Component.Children.Add(ChildComponent);

                ParseHierarchy(ChildNode, ChildComponent);
            }
        }
        private void ParseTwuiComponentNode(ref XElement Node, ref TwuiComponentModel Component)
        {
            // Grab our newly created ComponentModel, our current XmlElement that we're at,
            // and run all of our parsing here.
        }

        // Parse our Twui XML File using XDocument.
        public static TwuiModel ParseTwuiFile(string fileContents)
        {
            // We need to take a TWUI.XML file and convert the entirety of its contents into C# structs
            // or POCOs that our program here can handle. The CA TWUI.XML file has a "specific" format
            // (that doesn't adhere to some XML conventions or this would be a lot easier)
            // that we need to handle to convert the inner elements, attributes, et. al, into our
            // exposed C# objects.

            // At the top of the file, we have our <?xml> callout, which we can effectively ignore.
            
            // <layout>:
                // Version: the TWUI.XML "schema" version used internally.
                // Comment: ignored text that can display in the CA TWUI editor.
                // Precache_condition: a CCO-code condition that is used to determine whether we want to dispose this component entirely, in-game.
                // <hierarchy>
                // <components>
            // </layout>

            // <hierarchy>:
                // <root this="GUID">
                    // subcomponents in the same format
                    // NOTE: only one subcomponent allowed on root; others can have multiple.
                // </root>

            // <components>:
                // <component_name
                    // this: GUID
                    // uniqueguid: GUID
                    // id: name
                    // tooltipslocalised: bool
                    // currentstate: TwuiState GUID.
            // </components>

            // Create our end-result TwuiModel that will represent the entirety of this file's contents
            // and our XDocument, which we're using to actually parse the contents of the TWUI XML file.
            TwuiModel ParsedModel = new();
            var doc = XDocument.Parse(fileContents);

            // Start off by querying and handling our Layout object.
            XElement? layout = doc.Element("layout");

            if (layout != null )
            {
                ParsedModel.Layout.Version = layout.Attribute("version")?.Value;
                ParsedModel.Layout.Comment = layout.Attribute("comment")?.Value;
                ParsedModel.Layout.PrecacheCondition = layout.Attribute("precache_condition")?.Value;

                // Let's do a nice little handling of our hierarchy element. We need to grab our hierarchy child node of layout,
                // then grab its root, set up its information, and then run a loop through all of its children, setting them up in the
                // ParsedModel with their name, guid, and their parent/child elements.
                XElement? hierarchyNode = layout.Element("hierarchy");

                if (hierarchyNode != null )
                {
                    var rootNode = hierarchyNode.Element("root");
                    var rootModel = ParsedModel.Root;

                    if (rootNode != null)
                    {
                        ParseHierarchy(rootNode, rootModel);
                        //ParseTwuiComponentNode(ref rootNode, ref rootModel);

                        //IEnumerable<XElement> descendantNodes = rootNode.Descendants();
                        //foreach (var descendantNode in descendantNodes)
                        //{
                        //    ParseTwuiComponentNode(ref descendantNode, ref thisComponent);
                        //}
                    }
                    //IEnumerable<XElement> hierarchyNode.Descendants();
                }
            }

            return ParsedModel;
        }

        // We need to parse out our Twui file and figure out all of the internals that
        // we care about for this Twui model. We'll run this operation threaded in the
        // background so we can update other UI in the meantime instead of having
        // this hold us up.
        public async Task ParseTwuiWithReader(string FilePath)
        {
            TwuiModel ParsedModel = new();

            // Let's grab the stream to decipher this xml file. Starting off by
            // grabbing the layout, version, and other "header" details, then we'll
            // decipher the hierarchy, and finally we'll loop through our individual
            // components in the <components> tag.
            using (XmlReader reader = XmlReader.Create(FilePath, DefaultSettings))
            {
                // Grab our Layout info to start!
                reader.ReadToFollowing("layout");
                HandleLayoutElement(ParsedModel, reader);

                reader.ReadToFollowing("hierarchy");
                HandleHierarchyElement(ParsedModel, reader);

                //reader.ReadStartElement();
                while (await reader.ReadAsync())
                {
                    switch (reader.NodeType)
                    {
                        // <element> node; we've hit a starting element.
                        case XmlNodeType.Element:
                            //switch (reader.Name)
                            //{
                                //case "layout":
                                //    await reader.ReadElementContentAsStringAsync();
                                //    HandleLayoutElement(ParsedModel);
                                //    break;
                            //}
                            // <layout> to get version and comment info.
                            // <hierarchy> to get global hierarchy info.
                            // <components> to get individual component infos.
                            break;
                    }
                }
            }
        }

        private void HandleLayoutElement(TwuiModel Model, XmlReader reader)
        {
            // TODO grab the attribute "version", which is really the big thing here huh.
            Model.Layout.Version = reader.GetAttribute("version") ?? string.Empty;
            Model.Layout.Comment = reader.GetAttribute("comment") ?? string.Empty;
            //reader.ReadAttributeValue();
        }

        private void HandleHierarchyElement(TwuiModel Model, XmlReader reader)
        {
            // We'll need to run through the hierarchy, starting first by defining our Root component on the TwuiModel,
            // then looping through all children start elements and caching their name and their GUID, recursively.
            XmlReader hierarchy = reader.ReadSubtree();
            TwuiComponentModel ThisComponent = Model.Root;

            // Grab the root node of the hierarchy, and assign its GUID.
            hierarchy.ReadStartElement("root");
            string? id = hierarchy.GetAttribute("this");
            if (id != null)
            {
                //ThisComponent.Id = Guid.Parse(id);
                ThisComponent.Guid = id;
            }

            // Now we'll keep reading the start elements, getting their names, assigning them as children
            // to the current component, getting their GUIDs, recursively until we are done.
            do {

            } while (hierarchy.Read());
        }
    }
}
