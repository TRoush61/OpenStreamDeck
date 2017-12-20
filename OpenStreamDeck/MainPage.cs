using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenStreamDeck.Handler;
using OpenStreamDeck.ConfigManagement;


namespace OpenStreamDeck
{
    //TODO: Tons of UI work. Managing keys being the highest importance. Accounts and additional settings to come later
    public partial class MainPage : Form
    {
        DeckHandler deckHandler;
        NotifyIcon systrayIcon;
        List<PictureBox> keyPictureBoxes;
        int selectedIndex = -1;
        bool changes = false;

        public MainPage(DeckHandler dh)
        {
            InitializeComponent();

            deckHandler = dh;

            //Generate tray icon
            systrayIcon = new NotifyIcon();
            systrayIcon.BalloonTipTitle = "OpenStreamDeck";
            systrayIcon.BalloonTipText = "Double click to open OpenStreamDeck profile manager";
            systrayIcon.ShowBalloonTip(500);
            systrayIcon.DoubleClick += systrayIcon_DoubleClicked;

            var menu = new ContextMenu();
            var menuItem = new MenuItem();
            menuItem.Index = 0;
            menuItem.Text = "Exit";
            menuItem.Click += exitButton_Clicked;
            menu.MenuItems.Add(menuItem);
            systrayIcon.ContextMenu = menu;

            systrayIcon.Icon = new Icon(AppDomain.CurrentDomain.BaseDirectory + "/image.ico");
            systrayIcon.Visible = true;

            //Build array of picture boxes. Indexes matching indexes for their respective keys.
            keyPictureBoxes = new List<PictureBox>();
            keyPictureBoxes.Add(keyZeroPicBox);
            keyPictureBoxes.Add(keyOnePicBox);
            keyPictureBoxes.Add(keyTwoPicBox);
            keyPictureBoxes.Add(keyThreePicBox);
            keyPictureBoxes.Add(keyFourPicBox);
            keyPictureBoxes.Add(keyFivePicBox);
            keyPictureBoxes.Add(keySixPicBox);
            keyPictureBoxes.Add(keySevenPicBox);
            keyPictureBoxes.Add(keyEightPicBox);
            keyPictureBoxes.Add(keyNinePicBox);
            keyPictureBoxes.Add(keyTenPicBox);
            keyPictureBoxes.Add(keyElevenPicBox);
            keyPictureBoxes.Add(keyTwelvePicBox);
            keyPictureBoxes.Add(keyThirteenPicBox);
            keyPictureBoxes.Add(keyFourteenPicBox);

            //Populate fields
            profileTextBox.Text = dh.CurrentProfile.ProfileName;

            //Set picturebox settings
            var i = 0;
            foreach (var pictureBox in keyPictureBoxes)
            {
                pictureBox.Image = dh.CurrentProfile.Pages[dh.CurrentPage].Keys[i].getImageForForm();
                pictureBox.Click += pictureBoxClicked;
                i++;
            }
        }
        
        private void pictureBoxClicked(object sender, EventArgs e)
        {
            var pb = sender as PictureBox;

            //Remove border from old selected pb
            if (selectedIndex != -1)
            {
                keyPictureBoxes[selectedIndex].BorderStyle = BorderStyle.None;
            }
            selectedIndex = keyPictureBoxes.IndexOf(pb);
            //Set the clicked one as the selected one
            pb.BorderStyle = BorderStyle.Fixed3D;
        }

        private void systrayIcon_DoubleClicked(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            this.Activate();
        }

        private void MainPage_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        private void exitButton_Clicked(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void profileTextBox_TextChanged(object sender, EventArgs e)
        {
            deckHandler.CurrentProfile.ProfileName = profileTextBox.Text;
            deckHandler.CurrentProfile.nameChanged = true;
            changes = true;
        }

        private void profileTextBox_Leave(object sender, EventArgs e)
        {
            if (changes)
            {
                ProfileManager.saveProfile(deckHandler.CurrentProfile);
                changes = false;
            }
        }

        private void profileTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            var tb = sender as TextBox;
            if (e.KeyChar.Equals(Keys.Enter))
            {
                SendKeys.Send("{TAB}");
            }
        }
    }
}
