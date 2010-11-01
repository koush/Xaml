using System;
using System.Collections.Generic;
using System.Text;
using OpenGLES;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;
//using System.Windows.Media.Interop;

namespace System.Windows.Media
{
    public enum PixelFormat
    {
        Rgb565 = 0,//PixelFormatID.PixelFormat16bppRGB565,
        Bgra32 = 1,//PixelFormatID.PixelFormat32bppARGB,
    }

    // Summary:
    //     Describes how content is resized to fill its allocated space.
    public enum Stretch
    {
        None = 0,
        Fill = 1,
        Uniform = 2,
        UniformToFill = 3,
    }
    
    public class BitmapSource : IDisposable
    {
        internal BitmapSource()
        {
        }

        ~BitmapSource()
        {
            //Dispose();
            //GC.SuppressFinalize(this);
        }

        internal uint myName;

        internal int myWidth;

        public int Width
        {
            get { return myWidth; }
        }
        internal int myHeight;

        public int Height
        {
            get { return myHeight; }
        }

        internal bool myIsTransparent = false;

        public static int GetValidTextureDimensionFromSize(int size)
        {
            int shiftAmount = 0;
            int minDim = size;
            while ((minDim >> 1) >= 1)
            {
                shiftAmount++;
                minDim >>= 1;
            }
            minDim <<= shiftAmount;
            if (minDim < size)
                minDim <<= 1;

            return minDim;
        }

        /*
        static IImagingFactory myImagingFactory = Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("327ABDA8-072B-11D3-9D7B-0000F81EF32E"))) as IImagingFactory;
        */

        unsafe static void ConvertToRGBA(byte[] pixels, int width, int height)
        {
            fixed (byte* pixelPointer = pixels)
            {
                ConvertToRGBA((IntPtr)pixelPointer, width, height);
            }
        }

        unsafe static void ConvertToRGBA(IntPtr pixelPointer, int width, int height)
        {
            int stride = width * height * 4;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < stride; x += 4)
                {
                    byte* bp = (byte*)pixelPointer + stride * y + x;
                    byte temp = bp[0];
                    bp[0] = bp[2];
                    bp[2] = temp;
                }
            }
        }

        /*
        unsafe IBitmapImage GetSquareBitmap(int squareDim, IntPtr pixelPointer, PixelFormat pixelFormat)
        {
            IBitmapImage bitmap = null;
            BitmapImageData data = new BitmapImageData();
            data.Scan0 = pixelPointer;
            data.Width = myWidth;
            data.Height = myHeight;
            data.Stride = myWidth * myHeight * 4;
            data.PixelFormat = (PixelFormatID)pixelFormat;
            myImagingFactory.CreateBitmapFromBuffer(ref data, out bitmap);
            IImage image = bitmap as IImage;
            IBitmapImage other;
            myImagingFactory.CreateBitmapFromImage(image, squareDim, squareDim, (PixelFormatID)pixelFormat, InterpolationHint.InterpolationHintDefault, out other);
            Marshal.FinalReleaseComObject(bitmap);
            Marshal.FinalReleaseComObject(image);
            return other;
        }
        */

        // Create assumes that the contents are ready to be loaded into opengl--
        // the dimensions are square, and it is in rgba
        unsafe internal void Create(int squareDim, IntPtr pixelPointer, PixelFormat pixelFormat)
        {
            uint tex;
            gl.GenTextures(1, &tex);
            myName = tex;
            myIsTransparent = pixelFormat != PixelFormat.Rgb565;
            
            uint glFormat = gl.GL_RGB;
            uint glType = gl.GL_UNSIGNED_SHORT_5_6_5;
            if (myIsTransparent)
            {
                glFormat = gl.GL_RGBA;
                glType = gl.GL_UNSIGNED_BYTE;
            }

            gl.BindTexture(gl.GL_TEXTURE_2D, myName);
            gl.TexImage2D(gl.GL_TEXTURE_2D, 0, glFormat, squareDim, squareDim, 0, glFormat, glType, pixelPointer);

            gl.TexParameteri(gl.GL_TEXTURE_2D, gl.GL_TEXTURE_MIN_FILTER, gl.GL_LINEAR);
            gl.TexParameteri(gl.GL_TEXTURE_2D, gl.GL_TEXTURE_MAG_FILTER, gl.GL_LINEAR);
            gl.TexParameteri(gl.GL_TEXTURE_2D, gl.GL_TEXTURE_WRAP_S, gl.GL_CLAMP_TO_EDGE);
            gl.TexParameteri(gl.GL_TEXTURE_2D, gl.GL_TEXTURE_WRAP_T, gl.GL_CLAMP_TO_EDGE);
        }

        unsafe internal static BitmapSource Create(int width, int height, PixelFormat pixelFormat, IntPtr pixelPointer, bool isReadOnly)
        {
            BitmapSource ret = new BitmapSource();
            ret.myWidth = width;
            ret.myHeight = height;

            // do a conversion to rgba
            byte[] rgbaConversion = null;
            if (pixelFormat == PixelFormat.Bgra32)
            {
                if (isReadOnly)
                {
                    rgbaConversion = new byte[width * height * 4];
                    Marshal.Copy(pixelPointer, rgbaConversion, 0, rgbaConversion.Length);
                    ConvertToRGBA(rgbaConversion, width, height);
                }
                else
                {
                    ConvertToRGBA(pixelPointer, width, height);
                }
            }

            fixed (byte* rgbaPointer = rgbaConversion)
            {
                if (rgbaPointer != null)
                    pixelPointer = (IntPtr)rgbaPointer;

                // now make it a square
                width = GetValidTextureDimensionFromSize(width);
                height = GetValidTextureDimensionFromSize(height);
                int squareDim = Math.Max(width, height);

                /*
                IBitmapImage bitmap = null;
                BitmapImageData data = new BitmapImageData();
                if (squareDim != ret.myWidth || squareDim != ret.myHeight)
                {
                    bitmap = ret.GetSquareBitmap(squareDim, pixelPointer, pixelFormat);
                    System.Drawing.Size size;
                    bitmap.GetSize(out size);
                    RECT rect = new RECT(0, 0, size.Width, size.Height);
                    bitmap.LockBits(ref rect, System.Windows.Media.Interop.ImageLockMode.ImageLockModeRead, (PixelFormatID)pixelFormat, out data);
                }

                if (data.Scan0 != IntPtr.Zero)
                    pixelPointer = data.Scan0;

                ret.Create(squareDim, pixelPointer, pixelFormat);

                if (data.Scan0 != IntPtr.Zero)
                    bitmap.UnlockBits(ref data);
                */
            }

            return ret;
        }

        unsafe public static BitmapSource Create(int width, int height, PixelFormat pixelFormat, byte[] pixels)
        {
            fixed (byte* pixelPointer = pixels)
            {
                return Create(width, height, pixelFormat, (IntPtr)pixelPointer, true);
            }
        }

        unsafe public static BitmapSource Create(Stream bitmapStream)
        {
            return null;
            /*
            int bytesLength;
            byte[] bytes;
            MemoryStream memStream = bitmapStream as MemoryStream;
            if (memStream != null)
            {
                bytesLength = (int)memStream.Length;
                bytes = memStream.GetBuffer();
            }
            else
            {
                bytesLength = (int)bitmapStream.Length;
                bytes = new byte[bytesLength];
                bitmapStream.Read(bytes, 0, bytesLength);
            }

            IImage image;
            ImageInfo info;
            uint hresult = myImagingFactory.CreateImageFromBuffer(bytes, (uint)bytesLength, BufferDisposalFlag.BufferDisposalFlagNone, out image);
            image.GetImageInfo(out info);

            int resizedWidth = (int)info.Width;
            int resizedHeight = (int)info.Height;

            resizedWidth = GetValidTextureDimensionFromSize(resizedWidth);
            resizedHeight = GetValidTextureDimensionFromSize(resizedHeight);

            if (resizedWidth == (int)info.Width && resizedHeight == (int)info.Height)
            {
                resizedWidth = 0;
                resizedHeight = 0;
            }

            PixelFormatID pixelFormat;
            if ((info.pixelFormat & PixelFormatID.PixelFormatAlpha) != 0)
                pixelFormat = PixelFormatID.PixelFormat32bppARGB;
            else
                pixelFormat = PixelFormatID.PixelFormat16bppRGB565;

            IBitmapImage bitmap;
            myImagingFactory.CreateBitmapFromImage(image, resizedWidth, resizedHeight, pixelFormat, InterpolationHint.InterpolationHintDefault, out bitmap);
            Marshal.FinalReleaseComObject(image);

            System.Drawing.Size size;
            bitmap.GetSize(out size);
            RECT rect = new RECT(0, 0, size.Width, size.Height);
            BitmapImageData data;
            bitmap.LockBits(ref rect, System.Windows.Media.Interop.ImageLockMode.ImageLockModeWrite | System.Windows.Media.Interop.ImageLockMode.ImageLockModeRead, pixelFormat, out data);

            BitmapSource ret = Create(resizedWidth, resizedHeight, (PixelFormat)data.PixelFormat, data.Scan0, false);
            bitmap.UnlockBits(ref data);

            ret.myWidth = info.Width;
            ret.myHeight = info.Height;

            return ret;
            */
        }
    
        #region IDisposable Members

        unsafe public void Dispose()
        {
            uint name = myName;
            gl.DeleteTextures(1, &name);
            myName = 0;
        }

        #endregion
    }
}