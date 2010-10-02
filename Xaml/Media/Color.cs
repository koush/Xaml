using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Media
{
    public struct Color
    {
        float myR;
        float myG;
        float myB;
        float myA;

        public Color(byte r, byte g, byte b, byte a)
        {
            myR = r / 255f;
            myG = g / 255f;
            myB = b / 255f;
            myA = a / 255f;
        }

        public Color(float r, float g, float b, float a)
        {
            myR = r;
            myG = g;
            myB = b;
            myA = a;
        }

        public float ScA
        {
            get
            {
                return myA;
            }
            set
            {
                myA = value;
            }
        }

        public float ScR
        {
            get
            {
                return myR;
            }
            set
            {
                myR = value;
            }
        }

        public float ScB
        {
            get
            {
                return myB;
            }
            set
            {
                myB = value;
            }
        }

        public float ScG
        {
            get
            {
                return myG;
            }
            set
            {
                myG = value;
            }
        }

        public byte A
        {
            get
            {
                return (byte)(myA * 255);
            }
            set
            {
                myA = value / 255f;
            }
        }
        public byte R
        {
            get
            {
                return (byte)(myR * 255);
            }
            set
            {
                myR = value / 255f;
            }
        }
        public byte G
        {
            get
            {
                return (byte)(myG * 255);
            }
            set
            {
                myG = value / 255f;
            }
        }
        public byte B
        {
            get
            {
                return (byte)(myB * 255);
            }
            set
            {
                myB = value / 255f;
            }
        }

        public Color Blend(Color other, float ratio)
        {
            Color ret = new Color();
            ret.myA = myA * ratio + (other.myA * (1 - ratio));
            ret.myR = myR * ratio + (other.myR * (1 - ratio));
            ret.myG = myG * ratio + (other.myG * (1 - ratio));
            ret.myB = myB * ratio + (other.myB * (1 - ratio));

            return ret;
        }
    }
}
