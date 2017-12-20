using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamDeckSharp;
using Newtonsoft.Json;
using System.Drawing;
using System.IO;
using OpenStreamDeck.Functions;

namespace OpenStreamDeck.ProfileObjects
{
    public delegate void KeyFunction(object sender);
    public enum LabelPosition
    {
        Positon_Not_Rendered,
        Position_Top,
        Position_Middle,
        Position_Bottom
    };

    public enum ImageSaveType
    {
        Image_File,
        Image_Color
    };

    [JsonObject("Key")]
    public class Key
    {
        [JsonProperty("Label")]
        public string Label { get; set; }
        [JsonProperty("LabelPos")]
        public LabelPosition LabelPos { get; set; }
        [JsonProperty("ImageType")]
        private ImageSaveType ImageType { get; set; }
        [JsonProperty("ImageLocation")]
        private string ImageLocation { get; set; }
        [JsonProperty("ImageColor")]
        private Color ImageColor { get; set; }
        [JsonProperty("KeyPressedFunction")]
        public KeyFunctions.KeyFunctionsEnum KeyPressedFunction { get; set; }
        [JsonProperty("KeyHeldFunction")]
        public KeyFunctions.KeyFunctionsEnum KeyHeldFunction { get; set; }
        [JsonProperty("PageReference")]
        public int PageReference { get; set; }
        [JsonProperty("WebUrl")]
        public string WebUrl { get; set; }
        [JsonProperty("PathToExe")]
        public string PathToExe { get; set; }
        [JsonProperty("SoundFilePath")]
        public string SoundFilePath { get; set; }

        [JsonConstructor]
        public Key()
        {
            Label = "";
            LabelPos = LabelPosition.Positon_Not_Rendered;
            ImageColor = Color.Black;
            ImageType = ImageSaveType.Image_Color;
            KeyPressedFunction = 0;
            KeyHeldFunction = 0;
            PageReference = -1;
            WebUrl = null;
            PathToExe = null;
            SoundFilePath = null;
        }

        public Image getImageForForm()
        {
            if (ImageType == ImageSaveType.Image_File)
            {
                if (File.Exists(ImageLocation))
                {
                    return Bitmap.FromFile(ImageLocation);
                }
                else
                {
                    return Bitmap.FromFile(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OpenStreamDeck/images/default.png");
                }
            }
            else if (ImageType == ImageSaveType.Image_Color)
            {
                Bitmap b = new Bitmap(72, 72);
                using (Graphics g = Graphics.FromImage(b))
                {
                    g.Clear(ImageColor);
                }
                return b;
            }
            else
            {
                return Bitmap.FromFile(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OpenStreamDeck/images/default.png");
            }
        }

        public StreamDeckKeyBitmap getImage()
        {
            if (ImageType == ImageSaveType.Image_File)
            {
                if (File.Exists(ImageLocation))
                {
                    return StreamDeckKeyBitmap.FromFile(ImageLocation);
                }
                else
                {
                    return StreamDeckKeyBitmap.FromFile(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OpenStreamDeck/images/default.png");
                }
            }
            else if (ImageType == ImageSaveType.Image_Color)
            {
                return StreamDeckKeyBitmap.FromRGBColor(ImageColor.R, ImageColor.G, ImageColor.B);
            }
            else
            {
                return StreamDeckKeyBitmap.FromFile(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OpenStreamDeck/images/default.png");
            }
        }

        public void setImage(Color color)
        {
            ImageColor = color;
            ImageType = ImageSaveType.Image_Color;
        }

        public void setImage(string imageLocation)
        {
            ImageLocation = imageLocation;
            ImageType = ImageSaveType.Image_File;
        }
    }
}
