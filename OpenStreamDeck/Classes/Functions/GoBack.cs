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
    class GoBack : KeyFunction
    {
        [JsonConstructor]
        public GoBack()
        {
            base.isNavigationKey = true;
        }

        public GoBack(DeckHandler dh) : base(dh)
        {
            base.isNavigationKey = true;

        }

        public override string getFunctionName()
        {
            return "Go back a page";
        }

        public override void Run(DeckHandler dh)
        {
            if (dh.PageStack.Count > 0)
            {
                dh.CurrentPage = dh.PageStack.Pop();
                dh.renderPage();
            }
            else
            {
                MessageBox.Show("No page to go back to");
            }
        }

        public override void ShowForm()
        {
            MessageBox.Show("No settings to change for this function");
        }
    }
}
