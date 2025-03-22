using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace GroovyCommon.Controls
{
    /// <summary>
    /// A RichTextBox that can bind to a Document.
    /// </summary>
    public class RicherTextBox : RichTextBox
    {
        public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register(
            nameof(BoundDocument),
            typeof(FlowDocument),
            typeof(RicherTextBox),
            new FrameworkPropertyMetadata(
                null, OnDocumentChanged));

        public FlowDocument BoundDocument
        {
            get => (FlowDocument)GetValue(DocumentProperty);
            set => SetValue(DocumentProperty, value);
        }

        public static void OnDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RicherTextBox rtb)
            {
                //rtb.
                rtb.Document = (FlowDocument)e.NewValue;
            }
        }
    }
}
