using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenStreamDeck.Handler;

namespace OpenStreamDeck
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DeckHandler deckHandler = new DeckHandler();
            //TODO: Manage streamdeck not being connected and manage searching for the connection periodically when one isn't detected
            if (deckHandler.Deck == null)
            {

            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainPage(deckHandler));
        }
    }
}
