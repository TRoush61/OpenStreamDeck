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
        TextBox pageReferenceBox;

        [JsonConstructor]
        public OpenWebLink()
        {

        }

        public OpenWebLink(DeckHandler dh)
        {

        }

        public override string getFunctionName()
        {
            return "Open Web Link";
        }

        public override void Run(DeckHandler dh)
        {

        }

        public override void ShowForm()
        {
            Form userInput = new Form();
            userInput.Height = 200;
            userInput.Width = 300;
            userInput.FormBorderStyle = FormBorderStyle.FixedDialog;
            userInput.Text = "";
            pageReferenceBox = new TextBox()
            {
                Location = new System.Drawing.Point(75, 50),
                Width = 125
            };
            userInput.Controls.Add(pageReferenceBox);
            Button submitButton = new Button()
            {
                Location = new System.Drawing.Point(100, 75),
                Text = "Submit"
            };
            submitButton.Click += submitButtonClicked;
            userInput.Controls.Add(submitButton);

            userInput.Show();
        }

        private void submitButtonClicked(object sender, EventArgs e)
        {

        }
    }
}
