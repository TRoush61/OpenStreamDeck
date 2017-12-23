using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

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
        public List<byte[]> BitmapsFromGif(Image gif)
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

        private Image[] GifToFrames(Image animatedGif)
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
        private byte[] ImageToBytes(Image frameImage)
        {
            // Initialize some variables.
            byte[] frameBytes = { 0x20 };
            Bitmap frameBitmap = (Bitmap)frameImage;
            ImageFormat format = ImageFormat.Bmp;
            // Convert the image to a bitmap and bytes.
            using (MemoryStream ms = new MemoryStream())
            {
                frameImage.Save(ms, format);
                frameBytes = ms.ToArray();
            }
            // Returns the image as a bitmap in bytes.
            return frameBytes;
        }

    }
}

// Options for exporting image to:
// FromRawBitmap(byte[] bitmapData)
// FromStream(Stream bitmapStream)
// FromFile(string bitmapFile)
// (internal) FromDrawingBitmap(Bitmap bitmap)
