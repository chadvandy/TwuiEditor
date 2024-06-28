using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using TwUiEd.Core.Attributes;
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
        private static void ParseTwuiComponentNode(XElement Node, TwuiModel Model)
        {
            // Grab our newly created ComponentModel, our current XmlElement that we're at,
            // and run all of our parsing here.

            TwuiComponentModel ThisComponent = Model.AllComponents().FirstOrDefault(x => x.Guid == Node.Attribute("this")?.Value);
            uint version = Model.Version;

            if (ThisComponent  != null)
            {
                // Loop through all of our attributes within this element, and handle their values.
                // We want to keep in mind the expected schema, while still allowing for any attribute to be loaded if
                // we don't already have it filled out in the schema.

                // Loop through all of our properties in the component that have a defined TwuiAttribute on them.
                IEnumerable<PropertyInfo> Propeties = typeof(TwuiComponentModel).GetProperties()
                    .Where(prop => prop.IsDefined(typeof(TwuiPropertyAttribute), false));

                // Grab the "attributes" in our XML file. These are the actual values that we'll be loading in.
                IEnumerable<XAttribute> attributes = Node.Attributes();

                List<string> LoadedProperties = [];

                foreach (PropertyInfo Prop in Propeties)
                {
                    // Grab all of our property attributes that we have defined for this property.
                    System.Attribute[] prop_attrs = System.Attribute.GetCustomAttributes(Prop);

                    // Test each attribute to see if we have any that are within the valid version range.
                    foreach (var attr in prop_attrs)
                    {
                        if (attr is TwuiPropertyAttribute twui_attr)
                        {
                            uint vers_added = twui_attr.VersionAdded;
                            uint vers_removed = twui_attr.VersionRemoved;

                            if (version >= vers_added && vers_removed > version) 
                            {
                                // This version is valid; let's load up the XAttribute and then jump to our next Property.
                                string name = twui_attr.Node;
                                //Type type = twui_attr.Type;
                                //Type type = Prop.GetType();

                                // Search the XML file for an attribute that matches our specified name.
                                var xattr = attributes.FirstOrDefault(x => x.Name.LocalName == name);
                                if (xattr != null)
                                {
                                    //TwuiPropertyModel twuiProperty = new(
                                    //    name,
                                    //    type,
                                    //    Convert.ChangeType(xattr.Value, type)
                                    //    )
                                    //{
                                    //    IsDecoded = true,
                                    //    Description = twui_attr.Description,
                                    //    IsRequired = twui_attr.Required
                                    //};

                                    var property_getter = Prop.GetGetMethod();
                                    var property_setter = Prop.GetSetMethod();
                                    var property = property_getter?.Invoke(ThisComponent, []);
                                    //TwuiPropertyModel? property 
                                    if (property is TwuiPropertyModel twuiProperty)
                                    {
                                        Type type = twuiProperty.DataType;

                                        twuiProperty.Name = name;
                                        twuiProperty.Value = Convert.ChangeType(xattr.Value, type);
                                        twuiProperty.Description = twui_attr.Description;
                                        twuiProperty.IsRequired = twui_attr.Required;
                                        twuiProperty.IsDecoded = true;
                                    }

                                    //if (property != null)
                                    //{
                                    //    Type prop_type = property.GetType();

                                    //    if (prop_type.IsGenericType && prop_type.GetGenericTypeDefinition() == typeof(TwuiPropertyModel<>))
                                    //    {
                                    //        Type prop_gen_type = prop_type.GetGenericArguments()[0];
                                    //        Type gen_type = typeof(TwuiPropertyModel<>).MakeGenericType(prop_gen_type);

                                    //        gen_type.GetProperty("Name")?.SetValue(property, name);
                                    //        gen_type.GetProperty("Value")?.SetValue(property, Convert.ChangeType(xattr.Value, prop_gen_type));
                                    //        gen_type.GetProperty("Description")?.SetValue(property, twui_attr.Description);
                                    //        gen_type.GetProperty("IsRequired")?.SetValue(property, twui_attr.Required);
                                    //        gen_type.GetProperty("IsDecoded")?.SetValue(property, true);
                                    //        gen_type.GetProperty("PropertyName")?.SetValue(property, Prop.Name);
                                    //    }
                                    //}
                                    //if (property is TwuiPropertyModel<> twuiProperty)
                                    //{
                                    //    Type type = twuiProperty.DataType;

                                    //    twuiProperty.Name = name;
                                    //    twuiProperty.Value = Convert.ChangeType(xattr.Value, type);
                                    //    twuiProperty.Description = twui_attr.Description;
                                    //    twuiProperty.IsRequired = twui_attr.Required;
                                    //    twuiProperty.IsDecoded = true;

                                    //    //property_setter?.Invoke(ThisComponent, [twuiProperty]);
                                    //}


                                    LoadedProperties.Add(name);

                                    // Get the set method for this specific property and pass the new TwuiPropertyModel 
                                    // to that set method for our current ComponentModel.
                                    //var property_setter = Prop.GetSetMethod();
                                    //property_setter?.Invoke(ThisComponent, [twuiProperty]);
                                    //ThisComponent.Properties.Add(twuiProperty);
                                    break;
                                }
                            }
                        }
                    }
                }

                // Run a second loop to load in all non-decoded attributes in the XML file.
                foreach (XAttribute Attribute in attributes)
                {
                    // Ensure that we don't already have a field loaded up with this name.
                    string name = Attribute.Name.LocalName;
                    //if (!ThisComponent.Properties.Any(x => x.Name == name))
                    if (!LoadedProperties.Contains(name))
                    {
                        // We'll assume that this is a string.
                        TwuiPropertyModel twuiProperty = new(typeof(string), string.Empty)
                        {
                            Name = name,
                            Value = Attribute.Value,
                            Description = "This field is not decoded! Edit this with care.",
                            IsDecoded = false,
                        };

                        ThisComponent.Properties.Add(twuiProperty);
                    }
                }

                // TODO loop through all of our sub-elements for this data model.
                    // Run through all properties of this data model that have the TwuiElementAttribute.
                    // Grab all XElements and find all the ones that match the naming expected.

            }
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
                ParsedModel.Layout.Version = uint.Parse(layout.Attribute("version")?.Value);
                ParsedModel.Layout.Comment = layout.Attribute("comment")?.Value;
                ParsedModel.Layout.PrecacheCondition = layout.Attribute("precache_condition")?.Value;

                XElement? hierarchyNode = layout.Element("hierarchy");
                XElement? componentNode = layout.Element("components");

                // Let's do a nice little handling of our hierarchy element. We need to grab our hierarchy child node of layout,
                // then grab its root, set up its information, and then run a loop through all of its children, setting them up in the
                // ParsedModel with their name, guid, and their parent/child elements.
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

                // Handle our <components> element. We'll start by grabbing our main element, the <components> node,
                // and then we'll loop through only the immediate children of that element. We need to grab the
                // pre-created TwuiComponentModel by filtering by name & GUID, and then we'll parse all of the
                // internal information for this TwuiComponent!
                if (componentNode != null)
                {
                    IEnumerable<XElement> componentNodes = componentNode.Elements();

                    foreach(XElement component in componentNodes)
                    {
                        ParseTwuiComponentNode(component, ParsedModel);
                    }
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
            //Model.Layout.Version = reader.GetAttribute("version") ?? string.Empty;
            //Model.Layout.Comment = reader.GetAttribute("comment") ?? string.Empty;
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
