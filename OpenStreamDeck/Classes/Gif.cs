using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using StreamDeckSharp;

namespace OpenStreamDeck.Classes
{
    /// <summary>
    /// Represents a gif that can be used as key images.
    /// </summary>
    class OpenStreamDeckGif
    {
        /// <summary>
        /// Creates a StreamDeck-compatible Byte array of bitmaps in List form from a Gif file.
        /// </summary>
        /// <param name="gif">A 72 x 72 gif. Use wiggle.gif as placeholder.</param>
        /// <returns></returns>
        static public List<byte[]> BitmapsFromGif(Image gif)
        {
            Console.WriteLine("GIF Received (as Image).");
            // Retrieve gif frames as Image Array, and initialize the variables.
            Image[] frames = GifToFrames(gif);
            byte[] emptyBytes = { 0x20, 0x20 };
            List<byte[]> bitmaps = new List<byte[]>();
            // Convert Image Array to Bitmap Array.
            for (int i = 0; i < frames.Length; i++)
            {
                bitmaps.Add( ImageToBytes(frames[i]) );
            }
            return bitmaps;
        }

        static private Image[] GifToFrames(Image animatedGif)
        {
            // Find number of frames.
            int totalFrames = animatedGif.GetFrameCount(FrameDimension.Time);
            // Create array and add each gif frame to it.
            Image[] gifFrames = new Image[totalFrames];
            for (int i = 0; i < totalFrames; i++)
            {
                animatedGif.SelectActiveFrame(FrameDimension.Time, i);
                gifFrames[i] = ((Image)animatedGif.Clone());
            }
            // Returns the found frames.
            return gifFrames;
        }
        /// <summary>
        /// Converts an Image to a StreamDeck bitmap.
        /// </summary>
        /// <param name="frameImage">Received Image</param>
        /// <returns></returns>
        
        static private byte[] ImageToBytes(Image frameImage)
        {
            // Initialize some variables.
            byte[] frameBytes = { 0x20 };
            Bitmap bitmap = (Bitmap)frameImage;
            ImageFormat format = ImageFormat.Bmp;
            BitmapData data = null;
            // Convert the image to a bitmap and bytes.
            try
            {
                data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
                var managedRGB = new byte[15552];

                //HACK: This is from an internal method from SDS, and may be able to be bettered.
                unsafe
                {
                    byte* bdata = (byte*)data.Scan0;

                    if (data.PixelFormat == PixelFormat.Format24bppRgb)
                    {
                        for (int y = 0; y < 72; y++)
                        {
                            for (int x = 0; x < 72; x++)
                            {
                                var ps = data.Stride * y + x * 3;
                                var pt = 72 * 3 * (y + 1) - (x + 1) * 3;
                                managedRGB[pt + 0] = bdata[ps + 0];
                                managedRGB[pt + 1] = bdata[ps + 1];
                                managedRGB[pt + 2] = bdata[ps + 2];
                            }
                        }
                    }
                    else if (data.PixelFormat == PixelFormat.Format32bppArgb)
                    {
                        for (int y = 0; y < 72; y++)
                        {
                            for (int x = 0; x < 72; x++)
                            {
                                var ps = data.Stride * y + x * 4;
                                var pt = 72 * 3 * (y + 1) - (x + 1) * 3;
                                double alpha = (double)bdata[ps + 3] / 255f;
                                managedRGB[pt + 0] = (byte)Math.Round(bdata[ps + 0] * alpha);
                                managedRGB[pt + 1] = (byte)Math.Round(bdata[ps + 1] * alpha);
                                managedRGB[pt + 2] = (byte)Math.Round(bdata[ps + 2] * alpha);
                            }
                        }
                    }
                    else
                    {
                        throw new NotSupportedException("Unsupported pixel format");
                    }
                }

                return managedRGB;
            }
            finally
            {
                if (data != null)
                    bitmap.UnlockBits(data);
            }
        }
    }
}
