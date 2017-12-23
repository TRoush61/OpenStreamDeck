using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using OpenStreamDeck.Functions;
using OpenStreamDeck.Handler;
using OpenStreamDeck.ProfileObjects;

namespace OpenStreamDeck.Functions
{
    class FolderNavigate : KeyFunction
    {
        [JsonProperty("PageReference")]
        public Page PageReference;

        [JsonConstructor]
        public FolderNavigate()
        {
            base.isNavigationKey = true;
        }

        public FolderNavigate(DeckHandler dh) : base(dh)
        {
            base.isNavigationKey = true;
            PageReference = new Page("New Page");
            PageReference.Keys[4].setImage("Resources\\back.png");
            PageReference.Keys[4].KeyPressedFunction = new GoBack(dh);
            PageReference.Keys[4].KeyHeldFunction = new GoHome(dh);
        }

        public override string getFunctionName()
        {
            return "Navigate to folder";
        }

        public override void Run(DeckHandler dh)
        {
            dh.PageStack.Push(dh.CurrentPage);
            dh.CurrentPage = PageReference;
            dh.renderPage();
        }

        public override void ShowForm()
        {
            MessageBox.Show("No settings to change for this function");
        }
    }
}
