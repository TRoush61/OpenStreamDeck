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
    class GoHome : KeyFunction
    {
        public GoHome(DeckHandler dh) : base(dh)
        {

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