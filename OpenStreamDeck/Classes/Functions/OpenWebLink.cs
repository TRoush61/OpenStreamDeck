using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenStreamDeck.Handler;
using System.Windows.Forms;

namespace OpenStreamDeck.Functions
{
    //Not working yet. Need to implement the form for setting the URL
    class OpenWebLink : KeyFunction
    {
        [JsonProperty("WebUrl")]
        public string WebUrl;
        TextBox urlTextBox;

        [JsonConstructor]
        public OpenWebLink()
        {
        }

        public OpenWebLink(DeckHandler dh)
        {
            WebUrl = "";
        }

        public OpenWebLink(string url)
        {
            WebUrl = url;
        }

        public override string getFunctionName()
        {
            return "Open Web Link";
        }

        public override void Run(DeckHandler dh)
        {
            if (!String.IsNullOrEmpty(WebUrl) && IsValidUri(WebUrl))
            {
                System.Diagnostics.Process.Start(WebUrl);
            }
            else
            {
                MessageBox.Show(String.Format("Couldn't load that link. It may not be valid. ({0})", WebUrl));
            }
        }

        public override void ShowForm()
        {
            Form userInput = new Form();
            userInput.Height = 200;
            userInput.Width = 300;
            userInput.FormBorderStyle = FormBorderStyle.FixedDialog;
            userInput.Text = "Input URL";
            urlTextBox = new TextBox()
            {
                Text = WebUrl,
                Location = new System.Drawing.Point(75, 50),
                Width = 125
            };
            urlTextBox.TextChanged += textBoxChanged;
            userInput.Controls.Add(urlTextBox);

            userInput.Show();
        }

        private void textBoxChanged(object sender, EventArgs e)
        {
            WebUrl = urlTextBox.Text;
        }

        public static bool IsValidUri(string uri)
        {
            if (!Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                return false;
            Uri tmp;
            if (!Uri.TryCreate(uri, UriKind.Absolute, out tmp))
                return false;
            return tmp.Scheme == Uri.UriSchemeHttp || tmp.Scheme == Uri.UriSchemeHttps;
        }
    }
}
