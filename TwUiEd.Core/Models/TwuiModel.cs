using System;
using System.CodeDom;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TwUiEd.Core.Models
{
    // The "internal" model for our Twui; handles a lot of the data internal to a Twui file,
    // including the version used, the hierarchy, all the individual components, etc.
    public class TwuiModel
    {
        public uint Version { get => Layout.Version ?? 999; }
        public TwuiLayoutModel Layout { get; set; } = new();
        public TwuiComponentModel Root { get; set; } = new();

        public IEnumerable<TwuiComponentModel> AllComponents()
        {
            Stack<TwuiComponentModel> nodes = new();
            nodes.Push(Root);

            while (nodes.Count > 0)
            {
                var n = nodes.Pop();
                yield return n;

                for (int i = n.Children.Count - 1; i >= 0; i--)
                    nodes.Push(n.Children[i]);
            }
        }
    }

    public class TwuiLayoutModel
    {
        public uint? Version { get; set; } = 0;
        public string? Comment { get; set; } = string.Empty;
        public string? PrecacheCondition { get; set; } = string.Empty;
    }

    public class TwuiBaseComponentModel
    {
        // Handles the attributes "this" and "uniqueguid"
        public string Guid { get; set; } = string.Empty;
    }
}
