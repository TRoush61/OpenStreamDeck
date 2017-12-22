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
    class GoHome : KeyFunction
    {
        [JsonConstructor]
        public GoHome()
        {
            base.isNavigationKey = true;
        }

        public GoHome(DeckHandler dh) : base(dh)
        {
            base.isNavigationKey = true;
        }

        public override string getFunctionName()
        {
            return "Go to home page";
        }

        public override void Run(DeckHandler dh)
        {
            dh.CurrentPage = 0;
            dh.renderPage();
        }

        public override void ShowForm()
        {
            MessageBox.Show("No settings to change for this function");
        }
    }
}