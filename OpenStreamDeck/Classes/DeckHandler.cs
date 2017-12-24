using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamDeckSharp;
using System.Windows.Forms;
using OpenStreamDeck.ProfileObjects;
using OpenStreamDeck.ConfigManagement;
using OpenStreamDeck.Functions;
using System.IO;
using System.Diagnostics;

namespace OpenStreamDeck.Handler
{
    public class DeckHandler
    {
        public IStreamDeck Deck;
        public int CurrentPage;
        public Profile CurrentProfile;
        public double KeyHeldInterval = 500.0;
        public byte DeckBrightness = 100;
        private System.Timers.Timer KeyPressedTimer;
        private StreamDeckKeyEventArgs LastKeyPressed;
        private double KeyHeldFor;

        public DeckHandler()
        {
            //Load the default profile
            var path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OpenStreamDeck/profiles/default.json";
            try
            {
                CurrentProfile = ProfileManager.loadProfile(path);
            }
            catch (Exception e)
            {
                MessageBox.Show(String.Format("An error occurred while loading your default profile. Generating a new default profile. ({0})", e.Message));
                if (CurrentProfile == null)
                {
                    CurrentProfile = new Profile("Default");
                    ProfileManager.saveProfile(CurrentProfile);
                }
            }

            try
            {
                Deck = StreamDeck.FromHID();
            }
            catch (Exception e)
            {
                //MessageBox.Show(String.Format("Cannot connect to StreamDeck ({0})", e.Message));
                Debug.WriteLine("Cannot connect to StreamDeck {0}", e.Message);
                return;
            }
            Application.ApplicationExit += onApplicationExit;
            Deck.ClearKeys();
            Deck.SetBrightness(DeckBrightness);
            Deck.KeyPressed += onKeyPressed;
            KeyPressedTimer = new System.Timers.Timer();
            KeyPressedTimer.Interval = 10;
            KeyPressedTimer.Elapsed += onKeyHeld;

            CurrentPage = 0;
            renderPage();
        }

        private void onKeyPressed(object sender, StreamDeckKeyEventArgs e)
        {
            var d = sender as IStreamDeck;
            if (d == null) return;
            LastKeyPressed = e;
            var keyPressed = CurrentProfile.Pages[CurrentPage].Keys[e.Key];

            //On key down
            if (e.IsDown)
            {
                KeyHeldFor = 0;
                KeyPressedTimer.Start();
            }
            //On key up
            else
            {
                KeyPressedTimer.Stop();
                if (!(KeyHeldFor >= KeyHeldInterval))
                {
                    keyPressed.KeyPressedFunction.Run(this);
                }
            }
        }

        private void onKeyHeld(object sender, EventArgs e)
        {
            KeyHeldFor += 10;
            if (KeyHeldFor >= KeyHeldInterval)
            {
                KeyPressedTimer.Stop();
                var keyPressed = CurrentProfile.Pages[CurrentPage].Keys[LastKeyPressed.Key];
                keyPressed.KeyHeldFunction.Run(this);
            }
        }

        public void renderPage()
        {
            if (Deck != null)
            {
                var i = 0;
                CurrentProfile.Pages[CurrentPage].Keys.TrimExcess();
                foreach (var key in CurrentProfile.Pages[CurrentPage].Keys)
                {
                    Deck.SetKeyBitmap(i, key.getImage());
                    ++i;
                }
            }
        }

        private void onApplicationExit(object sender, EventArgs e)
        {
            if (Deck != null)
            {
                Deck.ClearKeys();
                Deck.SetBrightness(0);
            }
        }
    }
}
