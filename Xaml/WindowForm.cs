using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using OpenGLES;
using System.Threading;

namespace System.Windows
{
    public partial class Window
    {
        internal class WindowForm : System.Windows.Forms.Form
        {
            public Window myWindow;

            public WindowForm()
            {
                Text = "Window";
                System.Windows.Forms.MainMenu menu = new System.Windows.Forms.MainMenu();
                var item = new System.Windows.Forms.MenuItem();
                item.Text = "Exit";
                item.Click += new EventHandler(item_Click);
                menu.MenuItems.Add(item);
                Menu = menu;
            }

            void item_Click(object sender, EventArgs e)
            {
                Close();
            }

            protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
            {
                myWindow.OnPaint();
            }

            protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)
            {
            }

            protected override void OnClosed(EventArgs e)
            {
                base.OnClosed(e);
                myWindow.OnClosed(e);
            }
        }
    }
}