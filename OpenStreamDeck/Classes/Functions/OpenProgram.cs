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
    class OpenProgram : KeyFunction
    {
        [JsonProperty("ProgramPath")]
        public string ProgramPath;
        Label programLabel;

        [JsonConstructor]
        public OpenProgram()
        {

        }

        public OpenProgram(DeckHandler dh) : base(dh)
        {

        }

        public override string getFunctionName()
        {
            return "Open Program";
        }

        public override void Run(DeckHandler dh)
        {
            if (!String.IsNullOrEmpty(ProgramPath))
            {
                System.Diagnostics.Process.Start(ProgramPath);
            }
        }

        public override void ShowForm()
        {
            Form userInput = new Form();
            userInput.Height = 200;
            userInput.Width = 300;
            userInput.FormBorderStyle = FormBorderStyle.FixedDialog;
            userInput.Text = "";
            programLabel = new Label()
            {
                Text = "Program Path",
                Location = new System.Drawing.Point(25, 50),
                Width = 100
            };
            userInput.Controls.Add(programLabel);
            Button chooseProgramButton = new Button()
            {
                Location = new System.Drawing.Point(150, 50),
                Text = "Select"
            };
            chooseProgramButton.Click += chooseProgramButtonClicked;
            userInput.Controls.Add(chooseProgramButton);

            userInput.Show();
        }

        private void chooseProgramButtonClicked(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Program Links(*.EXE;*.LNK)|*.EXE;*.LNK|All files (*.*)|*.*";
            fd.InitialDirectory = String.Format("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs");
            fd.Multiselect = false;
            fd.ValidateNames = true;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                ProgramPath = fd.FileName;
                programLabel.Text = ProgramPath;
            }
        }
    }
}
