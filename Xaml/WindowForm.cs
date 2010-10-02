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
        void OnPaint()
        {
            if (!IsMeasureValid)
            {
                //Control content = Content as Control;
                //if (content != null && !content.IsMeasureValid)
                //{
                //    content.Measure(new Size(myForm.ClientSize.Width, myForm.ClientSize.Height));
                //    content.Arrange(new Rect(0, 0, myForm.ClientSize.Width, myForm.ClientSize.Height));
                //}
                Measure(new Size(myForm.ClientSize.Width, myForm.ClientSize.Height));
            }

            gl.Clear(gl.GL_COLOR_BUFFER_BIT);

            gl.MatrixMode(gl.GL_PROJECTION);
            gl.LoadIdentity();
            gl.Orthof(0, myForm.ClientSize.Width, myForm.ClientSize.Height, 0, -10, 10);

            gl.MatrixMode(gl.GL_MODELVIEW);
            gl.LoadIdentity();

            Render(this, myDrawingContext);
            egl.SwapBuffers(myDisplay, mySurface);
        }

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