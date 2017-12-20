using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenStreamDeck.ProfileObjects;
using OpenStreamDeck.Handler;
using OpenStreamDeck.Plugins;

namespace OpenStreamDeck.Functions
{
    public class KeyFunctions
    {
        public enum KeyFunctionsEnum
        {
            NoAction,
            OpenWebBrowser,
            OpenProgram,
            PlaySoundBite,
            ChangePage,
            GoHome
        }

        public static void KeyPressedHandler(DeckHandler dh, object sender)
        {
            var key = sender as Key;
            switch (key.KeyPressedFunction)
            {
                case KeyFunctionsEnum.NoAction:
                    { 
                        break;
                    }
                case KeyFunctionsEnum.OpenWebBrowser:
                    {
                        if (String.IsNullOrEmpty(key.WebUrl))
                        {
                            OpenWebBrowser("http://");
                        }
                        OpenWebBrowser(key.WebUrl);
                        break;
                    }
                case KeyFunctionsEnum.OpenProgram:
                    {
                        if (String.IsNullOrEmpty(key.PathToExe))
                        {
                            break;
                        }
                        OpenProgram(key.PathToExe);
                        break;
                    }
                case KeyFunctionsEnum.PlaySoundBite:
                    {
                        /*if (String.IsNullOrEmpty(key.PathToExe))
                        {
                            break;
                        }*/
                        PlaySoundBite(key.SoundFilePath);
                        break;
                    }
                case KeyFunctionsEnum.ChangePage:
                    {
                        if (key.PageReference == -1)
                        {
                            break;
                        }
                        ChangePage(dh, key);
                        break;
                    }
                case KeyFunctionsEnum.GoHome:
                    {
                        GoHome(dh);
                        break;
                    }
            }
        }

        public static void KeyHeldHandler(DeckHandler dh, object sender)
        {
            var key = sender as Key;
            switch (key.KeyHeldFunction)
            {
                case KeyFunctionsEnum.NoAction:
                    {
                        KeyPressedHandler(dh, sender);
                        break;
                    }
                case KeyFunctionsEnum.OpenWebBrowser:
                    {
                        if (String.IsNullOrEmpty(key.WebUrl))
                        {
                            OpenWebBrowser("http://");
                        }
                        OpenWebBrowser(key.WebUrl);
                        break;
                    }
                case KeyFunctionsEnum.OpenProgram:
                    {
                        if (String.IsNullOrEmpty(key.PathToExe))
                        {
                            break;
                        }
                        OpenProgram(key.PathToExe);
                        break;
                    }
                    //TODO: Work on sound board implementation
                case KeyFunctionsEnum.PlaySoundBite:
                    {
                        /*if (String.IsNullOrEmpty(key.PathToExe))
                        {
                            break;
                        }*/
                        PlaySoundBite(key.SoundFilePath);
                        break;
                    }
                case KeyFunctionsEnum.ChangePage:
                    {
                        if (key.PageReference == -1)
                        {
                            break;
                        }
                        ChangePage(dh, key);
                        break;
                    }
                case KeyFunctionsEnum.GoHome:
                    {
                        GoHome(dh);
                        break;
                    }
            }
        }

        private static void OpenWebBrowser(string url)
        {
            System.Diagnostics.Process.Start(url);
        }

        private static void OpenProgram(string path)
        {
            System.Diagnostics.Process.Start(path);
        }

        private static void PlaySoundBite(string path)
        {
            TSPlugin.StartClient();
        }

        private static void ChangePage(DeckHandler dh, Key key)
        {
            dh.CurrentPage = key.PageReference;
            dh.renderPage();
        }

        private static void GoHome(DeckHandler dh)
        {
            dh.CurrentPage = 0;
            dh.renderPage();
        }
    }
}
