using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using OpenStreamDeck.Functions;
using OpenStreamDeck.Handler;

namespace OpenStreamDeck.Functions
{
    class GoBack : KeyFunction
    {
        public int PageReference;

        [JsonConstructor]
        public GoBack()
        {
            base.isNavigationKey = true;
        }

        public GoBack(DeckHandler dh) : base(dh)
        {
            base.isNavigationKey = true;
            PageReference = dh.CurrentPage;
        }

        public override string getFunctionName()
        {
            return "Go back a page";
        }

        public override void Run(DeckHandler dh)
        {
            dh.CurrentPage = PageReference;
            dh.renderPage();
        }

        public override void ShowForm()
        {
            MessageBox.Show("No settings to change for this function");
        }
    }
}
