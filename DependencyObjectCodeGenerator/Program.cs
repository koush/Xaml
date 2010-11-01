using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace DependencyObjectCodeGenerator
{
    static class Program
    {
        static void Main()
        {
                XmlSerializer ser = new XmlSerializer(typeof(DependencyObjectNamespace));
                using (FileStream f = new FileStream("/Users/koush/src/Xaml/Xaml/Controls/Controls.xml", FileMode.Open, FileAccess.Read))
                {
                    StringBuilder builder = new StringBuilder();
                    DependencyObjectNamespace ns = ser.Deserialize(f) as DependencyObjectNamespace;

                    ns.Write(builder);

                    Console.WriteLine(builder.ToString());
                }
        }
    }
}
