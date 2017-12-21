using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenStreamDeck.Functions;
using OpenStreamDeck.Handler;

namespace OpenStreamDeck.Classes.Functions
{
    class GoBack : KeyFunction
    {
        public int PageReference;

        public GoBack(DeckHandler dh) : base(dh)
        {
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
