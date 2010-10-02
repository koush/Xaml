using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;

namespace System.Windows.Controls
{
    public class ContentControl : Control
    {
        object myContent;
        public object Content
        {
            get
            {
                return myContent;
            }
            set
            {
                object oldContent = myContent;
                myContent = value;
                Control content = myContent as Control;
                if (content != null)
                {
                    myChildren.Clear();
                    AddChild(content);
                }

                OnContentChanged(oldContent, myContent); 
            }
        }

        protected virtual void OnContentChanged(object oldContent, object newContent)
        {
        }

        public ContentControl()
        {
            InitializeChildren();
        }
    }
}