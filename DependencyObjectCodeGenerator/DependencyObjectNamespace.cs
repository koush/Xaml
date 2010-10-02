using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DependencyObjectCodeGenerator
{
    public class DependencyObjectNamespace
    {
        public string Name;

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public List<DependencyObject> DependencyObject = new List<DependencyObject>();

        internal void Write(StringBuilder builder)
        {
            builder.AppendLine("using System;");
            builder.AppendLine("using System.Linq;");
            builder.AppendLine("using System.Collections.Generic;");
            builder.AppendLine("using System.Text;");
            builder.AppendLine("using System.Windows.Media;");
            builder.AppendLine("using System.Windows.Threading;");
            builder.AppendLine("using System.ComponentModel;");
            builder.AppendLine("using System.Windows.Controls;");
            builder.AppendLine("using System.Windows;");
            builder.AppendLine("using System.Windows.Shapes;");
            builder.AppendLine();
            
            builder.AppendFormat("namespace {0}", Name);
            builder.AppendLine();
            builder.Append("{");
            builder.AppendLine();

            foreach (DependencyObject o in DependencyObject)
            {
                o.Write(builder);
            }

            builder.Append("}");
            builder.AppendLine();
        }
    }
}
