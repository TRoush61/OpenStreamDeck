using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace OpenStreamDeck.Handler
{
    /// <summary>
    /// Stores a gif and handles conversion to StreamDeck.
    /// </summary>
    class GifHandler
    {
        // Values come from StreamDeckSharp, replicated here for selfsufficiency.
        public int ArraySize = 15552;
        public int ImageSize = 72;
        
        public Image SourceGif;
        public Image[] SourceFrames;
        public List<byte[]> BitmapList;
        public int Duration;
        public int CurrentFrame = 0;
        public DeckHandler myDeck;
        public int gifKey;

        /// <summary>
        /// Selects a GIF for the handler to use. 
        /// </summary>
        /// <param name="gif"></param>
        //TODO: Change this to be a proper constructor.
        public void Select(Image gif)
        {
            SourceGif = gif;
            Process(SourceGif);
        }
        /// <summary>
        /// Processes Gif and stores to class variables.
        /// </summary>
        /// <param name="listReturn">Set to True for a return. False only stores to class.</param>
        public void Process(Image gif)
        {
            // Get frames of the animated gif and store.
            SourceFrames = GifToFrames(gif);
            for (int i = 0; i < SourceFrames.Length; i++)
            {
                BitmapList.Add(ImageToBytes(SourceFrames[i]));
            }
            Duration = GetDuration(gif);
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

        private int GetDuration(Image gif)
        {
            PropertyItem gifItem = gif.GetPropertyItem(0x5100);
            // Property is in 1/100th of a second.
            int delay = (gifItem.Value[0] + gifItem.Value[1] * 256) * 10;
            return delay;
        }

        //HACK: Used Tyler's code, may need change/improve.
        public void sendToDeck(DeckHandler deck, int Key)
        {
            //TODO: Manage streamdeck not being connected and manage searching for the connection periodically when one isn't detected
            if (myDeck.Deck == null)
            {

            }
            myDeck = deck;
            gifKey = Key;
            Timer timer = new Timer();
            timer.Interval = 20;
            timer.Tick += SetGifFrame;
            timer.Start();
        }
        //HACK: Used Tyler's code, may need change/improve.
        public void SetGifFrame(object sender, EventArgs e)
        {
            if (CurrentFrame >= SourceFrames.Length)
            {
                CurrentFrame = 0;
            }
            myDeck.Deck.SetKeyBitmap(gifKey, BitmapList[CurrentFrame]);
            CurrentFrame++;
        }
    }
}
