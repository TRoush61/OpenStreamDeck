using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace OpenStreamDeck.Classes
{
    class OpenStreamDeckGif
    {
        // Going to get a GIF as a source file and provide an array of bitmaps.
        public void BitmapsFromGif(Image gif)
        {
            Console.WriteLine("GIF Received.");
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
    }
}
