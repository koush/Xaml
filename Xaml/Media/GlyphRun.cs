using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Media
{
    public enum TextAlignment
    {
        Left,
        Right,
        Center,
        Justify,
    }

    public class GlyphRun
    {
        internal Font Font;
        internal Point[] Glyphs;
        internal TextureCoordinate[] FontCoords;
        internal short[] Indices;

        public Geometry GetGeometry()
        {
            GlyphGeometry ret = new GlyphGeometry();
            ret.Indices = Indices;
            ret.Points = Glyphs;
            ret.myBoundingBox = new Rect(0, 0, myWidth, myHeight);
            return ret;
        }

        static int FindWhitespace(string text, int startIndex)
        {
            int space = text.IndexOf(' ', startIndex);
            int linebreak = text.IndexOf('\n', startIndex);
            if (space == -1)
                return linebreak;
            else if (linebreak == -1)
                return space;
            else
                return Math.Min(space, linebreak);
        }

        int MeasureString(string text)
        {
            int ret = 0;
            foreach (char c in text)
            {
                ret += Font.CharacterWidths[c] - Font.myLeadingSpace - Font.myTrailingSpace;
            }
            return ret;
        }

        struct LineBreak
        {
            public string Text;
            public int Index;
            public int Width;
        };


        LineBreak FitString(string text, int startIndex, int width)
        {
            LineBreak lineBreak = new LineBreak();
            lineBreak.Width = 0;
            int dims = 0;

            int currentIndex = FindWhitespace(text, startIndex);
            if (currentIndex == -1)
                currentIndex = text.Length;
            int lastIndex = startIndex;

            while (currentIndex != -1 && (dims = MeasureString(text.Substring(startIndex, currentIndex - startIndex))) <= width)
            {
                // record the width while succesfully fit
                lineBreak.Width = dims;

                // done
                if (currentIndex == text.Length)
                {
                    lastIndex = currentIndex;
                    currentIndex = -1;
                }
                else if (text[currentIndex] == '\n')
                {
                    lastIndex = currentIndex + 1;
                    currentIndex = -1;
                }
                else
                {
                    // get next word
                    lastIndex = currentIndex + 1;
                    currentIndex = FindWhitespace(text, lastIndex);
                    // end of string
                    if (currentIndex == -1)
                        currentIndex = text.Length;
                }
            }

            if (lastIndex == startIndex)
            {
                // the string was either too long or we are at the end of the string
                if (currentIndex == -1)
                    throw new Exception("Somehow executing unreachable code while drawing text.");

                currentIndex = lastIndex + 1;
                while ((dims = MeasureString(text.Substring(startIndex, currentIndex - startIndex))) <= width)
                {
                    lineBreak.Width = dims;
                    currentIndex++;
                }
                lineBreak.Width = Math.Min(dims, width);
                lineBreak.Text = text.Substring(startIndex, currentIndex - startIndex);
                lineBreak.Index = currentIndex;
                return lineBreak;
            }
            else
            {
                // return the index we're painting to
                lineBreak.Text = text.Substring(startIndex, lastIndex - startIndex);
                lineBreak.Index = lastIndex;
                return lineBreak;
            }
        }

        void MakeRect(int index, float left, float top, float width, float height, char c)
        {
            float right = left + width;
            float bottom = top + height;
            Glyphs[index] = new Point(left, bottom);
            Glyphs[index + 1] = new Point(left, top);
            Glyphs[index + 2] = new Point(right, bottom);
            Glyphs[index + 3] = new Point(right, top);

            int charIndex = c * 4;
            for (int i = 0; i < 4; i++)
            {
                FontCoords[index + i] = Font.TextureCoordinates[charIndex + i];
            }
        }

        void BuildLine(string text, int startIndex, float x, float y, float spaceAdjust)
        {
            for (int i = 0; i < text.Length; i++, startIndex++)
            {
                short quadIndex = (short)(startIndex * 4);

                char c = text[i];
                MakeRect(quadIndex, x, y, Font.CharacterWidths[c], Font.myHeight, c);
                x += Font.CharacterWidths[c] - Font.myLeadingSpace - Font.myTrailingSpace;
                if (c == ' ')
                    x += spaceAdjust;

                short index = (short)(startIndex * 6);
                Indices[index] = quadIndex;
                Indices[index + 1] = (short)(quadIndex + 1);
                Indices[index + 2] = (short)(quadIndex + 2);
                Indices[index + 3] = (short)(quadIndex + 2);
                Indices[index + 4] = (short)(quadIndex + 1);
                Indices[index + 5] = (short)(quadIndex + 3);
            }
        }

        float myWidth;
        public float Width
        {
            get
            {
                return myWidth;
            }
        }
        float myHeight;
        public float Height
        {
            get
            {
                return myHeight;
            }
        }

        int myTriangleCount;
        public int TriangleCount
        {
            get
            {
                return myTriangleCount;
            }
        }

        public GlyphRun(string text, Font font)
            : this(text, font, TextAlignment.Left, float.PositiveInfinity, float.PositiveInfinity, true)
        {
        }

        public GlyphRun(string text, Font font, TextAlignment alignment)
            : this(text, font, alignment, float.PositiveInfinity, float.PositiveInfinity, true)
        {
        }

        public GlyphRun(string text, Font font, TextAlignment alignment, float maxWidth, float maxHeight, bool autoEllipsis)
        {
            Font = font;
            List<LineBreak> linebreaks = new List<LineBreak>();

            string processingText = text;
            int totalHeight = 0;
            int totalWidth = 0;
            int totalChars = 0;
            int intWidth;
            if (maxWidth != float.PositiveInfinity)
                intWidth = (int)maxWidth;
            else
                intWidth = int.MaxValue;
            int intHeight;
            if (maxHeight != float.PositiveInfinity)
                intHeight = (int)maxHeight;
            else
                intHeight = int.MaxValue;
            LineBreak lineBreak = FitString(processingText, 0, intWidth);

            while (lineBreak.Index != processingText.Length)
            {
                LineBreak nextBreak = FitString(processingText, lineBreak.Index, intWidth);
                // see if this line needs ellipsis
                if (Font.myHeight + Font.myHeight + totalHeight > intHeight && autoEllipsis)
                {
                    string lineText = lineBreak.Text;
                    int ellipsisStart = lineText.Length - 3;
                    if (ellipsisStart < 0)
                        ellipsisStart = 0;
                    lineText = lineText.Substring(0, ellipsisStart) + "...";
                    lineBreak.Width = MeasureString(lineText);
                    lineBreak.Text = lineText;
                    break;
                }

                linebreaks.Add(lineBreak);
                totalWidth = Math.Max(totalWidth, lineBreak.Width);
                totalHeight += Font.myHeight;
                totalChars += lineBreak.Text.Length;
                lineBreak = nextBreak;
            }
            linebreaks.Add(lineBreak);
            totalHeight += Font.myHeight;
            totalWidth = Math.Max(totalWidth, lineBreak.Width);
            totalChars += lineBreak.Text.Length;
            myTriangleCount = totalChars * 2;

            Glyphs = new Point[totalChars * 4];
            FontCoords = new TextureCoordinate[totalChars * 4];
            Indices = new short[totalChars * 6];

            myWidth = totalWidth;
            myHeight = totalHeight;

            float y = 0;
            int curChars = 0;
            for (int i = 0; i < linebreaks.Count; i++)
            {
                LineBreak lbreak = linebreaks[i];
                float x;
                float spaceAdjust = 0;
                string lbreakText = lbreak.Text;
                switch (alignment)
                {
                    case TextAlignment.Left:
                        x = 0;
                        break;
                    case TextAlignment.Right:
                        x = myWidth - lbreak.Width;
                        break;
                    case TextAlignment.Center:
                        x = (myWidth - lbreak.Width) / 2;
                        break;
                    case TextAlignment.Justify:
                        x = 0;
                        if (i != linebreaks.Count - 1)
                        {
                            lbreakText = lbreakText.TrimStart(' ').TrimEnd(' ');
                            int spaceCount = 0;
                            foreach (char c in lbreakText)
                            {
                                if (c == ' ')
                                    spaceCount++;
                            }
                            int newWidth = MeasureString(lbreakText);
                            if (spaceCount != 0)
                                spaceAdjust = (myWidth - newWidth) / spaceCount;
                        }
                        break;
                    default:
                        throw new ArgumentException("Unknown alignment type.");
                }

                BuildLine(lbreakText, curChars, x, y, spaceAdjust);
                y += Font.myHeight;
                curChars += lbreakText.Length;
            }
        }
    }
}
