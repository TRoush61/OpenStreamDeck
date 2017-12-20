using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamDeckSharp;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using OpenStreamDeck.ProfileObjects;
using OpenStreamDeck.ConfigManagement;
using OpenStreamDeck.Functions;

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
            CurrentProfile = ProfileManager.loadProfile(path);
            if (CurrentProfile == null)
            {
                CurrentProfile = new Profile("Default");
                buildTestProfile();
                ProfileManager.saveProfile(CurrentProfile);
            }

            try
            {
                Deck = StreamDeck.FromHID();
            }
            catch (Exception e)
            {
                //MessageBox.Show(String.Format("Cannot connect to StreamDeck ({0})", e.Message));
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
                    KeyFunctions.KeyPressedHandler(this, keyPressed);
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
                KeyFunctions.KeyHeldHandler(this, keyPressed);
            }
        }

        public void renderPage()
        {
            var i = 0;
            CurrentProfile.Pages[CurrentPage].Keys.TrimExcess();
            foreach (var key in CurrentProfile.Pages[CurrentPage].Keys)
            {
                Deck.SetKeyBitmap(i, key.getImage());
                ++i;
            }
        }

        //TODO: Stop needing to do this to get a working profile
        private void buildTestProfile()
        {
            CurrentProfile.Pages[0].PageName = "Test";
            CurrentProfile.Pages[0].Keys[14].setImage(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OpenStreamDeck/images/chrome.png");
            CurrentProfile.Pages[0].Keys[14].KeyPressedFunction = KeyFunctions.KeyFunctionsEnum.OpenWebBrowser;
            CurrentProfile.Pages[0].Keys[14].WebUrl = "http://";
            CurrentProfile.Pages[0].Keys[14].KeyHeldFunction = KeyFunctions.KeyFunctionsEnum.ChangePage;
            CurrentProfile.Pages[0].Keys[14].PageReference = 1;
            CurrentProfile.Pages[0].Keys[0].setImage(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OpenStreamDeck/images/taskmanager.png");
            CurrentProfile.Pages[0].Keys[0].KeyPressedFunction = KeyFunctions.KeyFunctionsEnum.OpenProgram;
            CurrentProfile.Pages[0].Keys[0].PathToExe = System.Environment.GetFolderPath(System.Environment.SpecialFolder.SystemX86) + "/taskmgr.exe";
            CurrentProfile.Pages[0].Keys[4].setImage(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OpenStreamDeck/images/taskmanager.png");
            CurrentProfile.Pages[0].Keys[4].KeyPressedFunction = KeyFunctions.KeyFunctionsEnum.PlaySoundBite;

            CurrentProfile.Pages.Add(new Page("Page 2"));
            CurrentProfile.Pages[1].Keys[4].setImage(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OpenStreamDeck/images/back.png");
            CurrentProfile.Pages[1].Keys[4].KeyPressedFunction = KeyFunctions.KeyFunctionsEnum.ChangePage;
            CurrentProfile.Pages[1].Keys[4].PageReference = 0;
            CurrentProfile.Pages[1].Keys[4].KeyHeldFunction = KeyFunctions.KeyFunctionsEnum.GoHome;
            CurrentProfile.Pages[1].Keys[14].setImage(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OpenStreamDeck/images/chrome.png");
            CurrentProfile.Pages[1].Keys[14].KeyPressedFunction = KeyFunctions.KeyFunctionsEnum.OpenWebBrowser;
            CurrentProfile.Pages[1].Keys[14].WebUrl = "http://";
            CurrentProfile.Pages[1].Keys[13].setImage(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OpenStreamDeck/images/reddit.png");
            CurrentProfile.Pages[1].Keys[13].KeyPressedFunction = KeyFunctions.KeyFunctionsEnum.OpenWebBrowser;
            CurrentProfile.Pages[1].Keys[13].WebUrl = "https://reddit.com";
            CurrentProfile.Pages[1].Keys[12].setImage(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OpenStreamDeck/images/facebook.png");
            CurrentProfile.Pages[1].Keys[12].KeyPressedFunction = KeyFunctions.KeyFunctionsEnum.OpenWebBrowser;
            CurrentProfile.Pages[1].Keys[12].WebUrl = "https://facebook.com";
            CurrentProfile.Pages[1].Keys[11].setImage(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OpenStreamDeck/images/youtube.png");
            CurrentProfile.Pages[1].Keys[11].KeyPressedFunction = KeyFunctions.KeyFunctionsEnum.OpenWebBrowser;
            CurrentProfile.Pages[1].Keys[11].WebUrl = "https://youtube.com";
            CurrentProfile.Pages[1].Keys[10].setImage(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OpenStreamDeck/images/twitch.png");
            CurrentProfile.Pages[1].Keys[10].KeyPressedFunction = KeyFunctions.KeyFunctionsEnum.OpenWebBrowser;
            CurrentProfile.Pages[1].Keys[10].WebUrl = "https://twitch.tv";
            CurrentProfile.Pages[1].Keys[0].setImage(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OpenStreamDeck/images/folder.png");
            CurrentProfile.Pages[1].Keys[0].KeyPressedFunction = KeyFunctions.KeyFunctionsEnum.ChangePage;
            CurrentProfile.Pages[1].Keys[0].PageReference = 2;

            CurrentProfile.Pages.Add(new Page("Page 3"));
            CurrentProfile.Pages[2].Keys[4].setImage(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OpenStreamDeck/images/back.png");
            CurrentProfile.Pages[2].Keys[4].KeyPressedFunction = KeyFunctions.KeyFunctionsEnum.ChangePage;
            CurrentProfile.Pages[2].Keys[4].PageReference = 1;
            CurrentProfile.Pages[2].Keys[4].KeyHeldFunction = KeyFunctions.KeyFunctionsEnum.GoHome;
        }

        private void onApplicationExit(object sender, EventArgs e)
        {
            Deck.ClearKeys();
            Deck.SetBrightness(0);
        }
    }
}
