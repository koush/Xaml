using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DependencyObjectCodeGenerator
{
    public class DependencyObject
    {
        public string Name;
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public List<DependencyProperty> DependencyProperty = new List<DependencyProperty>();

        internal void Write(StringBuilder builder)
        {
            builder.AppendFormat("\tpartial class {0}", Name);
            builder.AppendLine();
            builder.AppendLine("\t{");

            foreach (DependencyProperty property in DependencyProperty)
            {
                property.Write(builder);
            }

            builder.AppendLine("\t\tprotected override void Initialize()");
            builder.AppendLine("\t\t{");
            builder.AppendLine("\t\t\tbase.Initialize();");
            foreach (DependencyProperty property in DependencyProperty)
            {
                if (property.Storage)
                    property.WriteStorage(builder);
            }
            builder.AppendLine("\t\t}");


            builder.AppendLine("\t}");
        }
    }
}