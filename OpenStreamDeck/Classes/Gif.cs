using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace OpenStreamDeck.Classes
{
    /// <summary>
    /// Represents a gif that can be used as key images.
    /// </summary>
    class OpenStreamDeckGif
    {

        /// <summary>
        /// Creates a StreamDeck-compatible Bitmap array from a Gif file.
        /// </summary>
        /// <param name="gif">Red channel</param>
        /// <returns></returns>
        public void BitmapsFromGif(Image gif)
        {
            Console.WriteLine("GIF Received (as Image).");
            // Retrieve gif frames as Image Array.
            Image[] frames = GifToFrames(gif);

            // Convert Image Array to Bitmap Array.

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

        // Options for exporting image to:
        // FromRawBitmap(byte[] bitmapData)
        // FromStream(Stream bitmapStream)
        // FromFile(string bitmapFile)
        // (internal) FromDrawingBitmap(Bitmap bitmap)

        /// <summary>
        /// Converts an Image to a StreamDeck bitmap.
        /// </summary>
        /// <param name="imageOriginal">Received Image</param>
        /// <returns></returns>
        private byte ImageToBytes(Image frameImage)
        {
            // Store image and export as a bitmap.

            byte frameBytes = 0;
            // Returns the image as a bitmap in bytes.
            return frameBytes;
        }

    }
}
