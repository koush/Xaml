using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace DependencyObjectCodeGenerator
{
    partial class GeneratorForm : Form
    {
        public GeneratorForm()
        {
            InitializeComponent();
        }

        private void myOpenButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "XML Files|*.xml";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                XmlSerializer ser = new XmlSerializer(typeof(DependencyObjectNamespace));
                using (FileStream f = new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read))
                {
                    StringBuilder builder = new StringBuilder();
                    DependencyObjectNamespace ns = ser.Deserialize(f) as DependencyObjectNamespace;

                    ns.Write(builder);

                    myGeneratedCodeBox.Text = builder.ToString();
                }
            }
        }
    }
}
