using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using OpenStreamDeck.Handler;
using OpenStreamDeck.Functions;
using TwitchLib;
using TwitchLib.Models.API.v5.Streams;
using OpenStreamDeck.ProfileObjects;
using OpenStreamDeck.Secrets;
using System.Security.Policy;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading;

namespace OpenStreamDeck.Functions
{
    class TwitchSubscriptions : KeyFunction
    {
        [JsonIgnore]
        System.Windows.Forms.Timer CacheTimer;
        [JsonIgnore]
        FollowedStreams liveChannels;
        [JsonIgnore]
        Thread updateLive;

        [JsonConstructor]
        public TwitchSubscriptions()
        {
            base.isNavigationKey = true;
            updateLive = new Thread(() =>
            {
                CacheTimer = new System.Windows.Forms.Timer();
                CacheTimer.Interval = 60000;
                CacheTimer.Tick += updateLiveChannels;
                CacheTimer.Start();
                updateLiveChannels(null, null);
            });
            updateLive.Start();
        }

        public TwitchSubscriptions(DeckHandler dh = null) : base(dh)
        {
            base.isNavigationKey = true;
            updateLive = new Thread(() =>
            {
                CacheTimer = new System.Windows.Forms.Timer();
                CacheTimer.Interval = 60000;
                CacheTimer.Tick += updateLiveChannels;
                CacheTimer.Start();
                updateLiveChannels(null, null);
            });
            updateLive.Start();
        }

        ~TwitchSubscriptions()
        {
            updateLive.Abort();
        }

        public override string getFunctionName()
        {
            return "Twitch/Get live channels";
        }

        public override void Run(DeckHandler dh)
        {
            Page PageReference = new Page("Twitch Live Subs");
            PageReference.Keys[4].setImage("Resources\\back.png");
            PageReference.Keys[4].KeyPressedFunction = new GoBack(dh);
            PageReference.Keys[4].KeyHeldFunction = new GoHome(dh);

            //Wait for liveChannels to be populated
            while(liveChannels == null)
            {
                Thread.Sleep(1);
            }

            var i = 0;
            foreach (var stream in liveChannels.Streams)
            {
                string saveLocation = "image_cache\\" + stream.Channel.DisplayName + ".jpeg";
                if (!File.Exists(saveLocation))
                {
                    var logoUrl = stream.Channel.Logo.Replace("300x300", "70x70");
                    Console.WriteLine(logoUrl);
                    WebClient wc = new WebClient();
                    byte[] bytes = wc.DownloadData(logoUrl);
                    MemoryStream ms = new MemoryStream(bytes);
                    Image img = Image.FromStream(ms);
                    img = ResizeImage(img, 72, 72);
                    if (!Directory.Exists("image_cache\\"))
                    {
                        Directory.CreateDirectory("image_cache\\");
                    }
                    img.Save(saveLocation, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                PageReference.Keys[i].setImage(saveLocation);
                PageReference.Keys[i].KeyPressedFunction = new OpenWebLink(stream.Channel.Url);
                i++;
                if (i == 4)
                {
                    i++;
                }
            }

            dh.PageStack.Push(dh.CurrentPage);
            dh.CurrentPage = PageReference;
            dh.renderPage();
        }

        public override void ShowForm()
        {
            MessageBox.Show("No settings to change for this function");
        }

        private async void updateLiveChannels(Object sender, EventArgs e)
        {
            TwitchAPI api = new TwitchAPI(Twitch.clientID, Twitch.authToken);
            liveChannels = await api.Streams.v5.GetFollowedStreamsAsync(streamType: "live", limit: 14);
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
