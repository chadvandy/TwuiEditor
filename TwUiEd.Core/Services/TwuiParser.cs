using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TwUiEd.Core.Models;

namespace TwUiEd.Core.Services
{
    public class TwuiParser
    {
        private static XmlReaderSettings DefaultSettings = new() { Async = true };

        // We need to parse out our Twui file and figure out all of the internals that
        // we care about for this Twui model. We'll run this operation threaded in the
        // background so we can update other UI in the meantime instead of having
        // this hold us up.
        public async Task ParseTwui(string FilePath)
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
                ThisComponent.Id = Guid.Parse(id);
            }

            // Now we'll keep reading the start elements, getting their names, assigning them as children
            // to the current component, getting their GUIDs, recursively until we are done.
            do {

            } while (hierarchy.Read());
        }
    }
}
