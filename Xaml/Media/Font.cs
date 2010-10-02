using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using OpenGLES;


using GDIFont = System.Drawing.Font;
using GDIFontFamily = System.Drawing.FontFamily;
using GDIFontStyle = System.Drawing.FontStyle;

namespace System.Windows.Media
{
    public enum FontStyle
    {
        Regular = GDIFontStyle.Regular,
        Bold = GDIFontStyle.Bold,
        Italic = GDIFontStyle.Italic,
        Underline = GDIFontStyle.Underline,
        Strikeout = GDIFontStyle.Strikeout,
    }

    public enum FontFamily
    {
        GenericSerif,
        GenericMonospace,
        GenericSansSerif,
    }

    struct tagTEXTMETRIC
    {
        public int tmHeight;
        public int tmAscent;
        public int tmDescent;
        public int tmInternalLeading;
        public int tmExternalLeading;
        public int tmAveCharWidth;
        public int tmMaxCharWidth;
        public int tmWeight;
        public int tmOverhang;
        public int tmDigitizedAspectX;
        public int tmDigitizedAspectY;
        public byte tmFirstChar;
        public byte tmLastChar;
        public byte tmDefaultChar;
        public byte tmBreakChar;
        public byte tmItalic;
        public byte tmUnderlined;
        public byte tmStruckOut;
        public byte tmPitchAndFamily;
        public byte tmCharSet;
    };


    public class Font : IDisposable
    {
        ~Font()
        {
            //Dispose();
        }

        readonly static SolidBrush myWhiteBrush = new SolidBrush(System.Drawing.Color.White);

        [DllImport("coredll")]
        extern static bool GetCharWidth32(IntPtr hdc, int iFirstChar, int iLastChar, [Out] int[] lpBuffer);

        [DllImport("coredll")]
        extern static IntPtr SelectObject(IntPtr hdc, IntPtr obj);

        [DllImport("coredll")]
        extern static bool GetTextMetrics(IntPtr hdc, ref tagTEXTMETRIC lptm);

        const char myFirstCharacterOfInterest = ' ';
        const char myLastCharacterOfInterest = '~';

        internal int myLeadingSpace;
        internal int myTrailingSpace;
        internal TextureCoordinate[] TextureCoordinates = new TextureCoordinate[256 * 4];
        internal int[] CharacterWidths = new int[256];
        internal Point[] CharacterLocations = new Point[256];
        internal int myHeight;

        readonly static string myCharactersOfInterest;

        static Font()
        {
            myCharactersOfInterest = string.Empty;
            for (char i = myFirstCharacterOfInterest; i <= myLastCharacterOfInterest; i++)
            {
                myCharactersOfInterest += i;
            }

            myTempBitmap = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format16bppRgb565);
            myTempGraphics = Graphics.FromImage(myTempBitmap);
            Default = new Font(FontFamily.GenericSerif, 10, FontStyle.Regular);
        }

        readonly static Bitmap myTempBitmap;
        readonly static Graphics myTempGraphics;
        public static readonly Font Default;
        internal BitmapSource mySource = new BitmapSource();

        unsafe void InitializeFont(GDIFont font)
        {
            IntPtr hdc = myTempGraphics.GetHdc();
            IntPtr hfont = font.ToHfont();
            SelectObject(hdc, hfont);

            if (!GetCharWidth32(hdc, 0, 255, CharacterWidths))
                throw new SystemException("Unable to measure character widths.");

            tagTEXTMETRIC metrics = new tagTEXTMETRIC();
            GetTextMetrics(hdc, ref metrics);
            myLeadingSpace = metrics.tmInternalLeading;
            myTrailingSpace = metrics.tmExternalLeading;

            myTempGraphics.ReleaseHdc(hdc);

            int width = 0;
            for (int i = myFirstCharacterOfInterest; i <= myLastCharacterOfInterest; i++)
            {
                CharacterWidths[i] += myLeadingSpace + myTrailingSpace;
                width += CharacterWidths[i];
            }
            myHeight = (int)Math.Round(myTempGraphics.MeasureString(myCharactersOfInterest, font).Height);

            int squareDim = (int)Math.Ceiling(Math.Sqrt(width * myHeight));
            squareDim = BitmapSource.GetValidTextureDimensionFromSize(squareDim);
            int squareWidth = squareDim;
            int squareHeight = squareDim;
            float fSquareWidth = squareWidth;
            float fSquareHeight = squareHeight;
            Bitmap bitmap;

            bool fit;
            do
            {
                bitmap = new Bitmap(squareWidth, squareHeight, System.Drawing.Imaging.PixelFormat.Format16bppRgb565);

                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    int x = 0;
                    int y = 0;

                    for (char i = myFirstCharacterOfInterest; i <= myLastCharacterOfInterest; i++)
                    {
                        if (x + CharacterWidths[i] >= fSquareWidth)
                        {
                            y += myHeight;
                            x = 0;
                        }
                        CharacterLocations[i] = new Point(x, y);

                        float uStart = x / fSquareWidth;
                        float uEnd = (x + CharacterWidths[i]) / fSquareWidth;
                        float vStart = y / fSquareHeight;
                        float vEnd = (y + myHeight) / fSquareHeight;

                        int offset = i * 4;
                        TextureCoordinates[offset] = new TextureCoordinate(uStart, vEnd);
                        TextureCoordinates[offset+ 1] = new TextureCoordinate(uStart, vStart);
                        TextureCoordinates[offset + 2] = new TextureCoordinate(uEnd, vEnd);
                        TextureCoordinates[offset + 3] = new TextureCoordinate(uEnd, vStart);

                        g.DrawString(i.ToString(), font, myWhiteBrush, x, y);
                        // adding a 1 pixel extra margin on the left seems to clear up some artifacting
                        // that occurs as a result of glyphs being too close together.
                        x += CharacterWidths[i] + 1;
                    }

                    fit = y + myHeight < fSquareHeight;
                    if (!fit)
                    {
                        squareWidth <<= 1;
                        fSquareWidth = squareWidth;
                    }
                }
            }
            while (!fit);

            byte[] alphaBytes = new byte[bitmap.Width * bitmap.Height];
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format16bppRgb565);

            int pixCount = 0;
            for (int y = 0; y < bitmap.Height; y++)
            {
                short* yp = (short*)((int)data.Scan0 + data.Stride * y);
                for (int x = 0; x < bitmap.Width; x++, pixCount++)
                {
                    short* p = (short*)(yp + x);
                    short pixel = *p;
                    byte b = (byte)((pixel & 0x1F) << 3);
                    byte g = (byte)(((pixel >> 5) & 0x3F) << 2);
                    byte r = (byte)(((pixel >> 11) & 0x1F) << 3);
                    byte totalAlpha = (byte)((r + g + b) / 3);
                    alphaBytes[pixCount] = totalAlpha;
                }
            }
            bitmap.UnlockBits(data);
            bitmap.Dispose();

            uint tex = 0;
            gl.GenTextures(1, &tex);
            gl.BindTexture(gl.GL_TEXTURE_2D, tex);

            fixed (byte* alphaBytesPointer = alphaBytes)
            {
                gl.TexImage2D(gl.GL_TEXTURE_2D, 0, gl.GL_ALPHA, squareWidth, squareHeight, 0, gl.GL_ALPHA, gl.GL_UNSIGNED_BYTE, (IntPtr)alphaBytesPointer);
            }

            gl.TexParameteri(gl.GL_TEXTURE_2D, gl.GL_TEXTURE_MIN_FILTER, gl.GL_LINEAR);
            gl.TexParameteri(gl.GL_TEXTURE_2D, gl.GL_TEXTURE_MAG_FILTER, gl.GL_LINEAR);
            gl.TexParameteri(gl.GL_TEXTURE_2D, gl.GL_TEXTURE_WRAP_S, gl.GL_CLAMP_TO_EDGE);
            gl.TexParameteri(gl.GL_TEXTURE_2D, gl.GL_TEXTURE_WRAP_T, gl.GL_CLAMP_TO_EDGE);

            mySource = new BitmapSource();
            mySource.myWidth = squareWidth;
            mySource.myHeight = squareHeight;
            mySource.myName = tex;
            mySource.myIsTransparent = true;
        }

        public Font(string name, float emSize, FontStyle style)
        {
            GDIFont font = new System.Drawing.Font(name, emSize, (GDIFontStyle)style);
            InitializeFont(font);
        }

        public Font(FontFamily family, float emSize, FontStyle style)
        {
            GDIFontFamily fam;
            switch (family)
            {
                case FontFamily.GenericMonospace:
                    fam = GDIFontFamily.GenericMonospace;
                    break;
                case FontFamily.GenericSansSerif:
                    fam = GDIFontFamily.GenericSansSerif;
                    break;
                default:
                    fam = GDIFontFamily.GenericSerif;
                    break;
            }
            GDIFont font = new System.Drawing.Font(fam, emSize, (GDIFontStyle)style);
            InitializeFont(font);
        }

        #region IDisposable Members

        public void Dispose()
        {
            ExtensionMethods.DisposeObject(ref mySource);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
