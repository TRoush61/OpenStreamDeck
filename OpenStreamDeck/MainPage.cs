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
using System.IO;
using System.Resources;

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
            ResourceManager rm = new ResourceManager("Images", typeof(MainPage).Assembly);
            systrayIcon.Icon = new Icon ("Resources/appicon.ico");
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

            
        }

        private void populateFields()
        {
            //Populate fields
            profileTextBox.Text = deckHandler.CurrentProfile.ProfileName;
            keyPressedLabel.Text = "";
            keyHeldLabel.Text = "";
            titlePosComboBox.Items.Add("Not Rendered");
            titlePosComboBox.Items.Add("Rendered Top");
            titlePosComboBox.Items.Add("Rendered Center");
            titlePosComboBox.Items.Add("Rendered Bottom");

            //Set picturebox settings
            var i = 0;
            foreach (var pictureBox in keyPictureBoxes)
            {
                pictureBox.Image = deckHandler.CurrentProfile.Pages[deckHandler.CurrentPage].Keys[i].getImageForForm();
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

            //Populate fields
            keyPressedLabel.Text = deckHandler.CurrentProfile.Pages[deckHandler.CurrentPage].Keys[selectedIndex].KeyPressedFunction.ToString();
            keyHeldLabel.Text = deckHandler.CurrentProfile.Pages[deckHandler.CurrentPage].Keys[selectedIndex].KeyHeldFunction.ToString();
            titleTextBox.Text = deckHandler.CurrentProfile.Pages[deckHandler.CurrentPage].Keys[selectedIndex].Label;
            titleTextBox.ReadOnly = false;
            titlePosComboBox.SelectedIndex = (int)deckHandler.CurrentProfile.Pages[deckHandler.CurrentPage].Keys[selectedIndex].LabelPos;
            iconPreview.Image = deckHandler.CurrentProfile.Pages[deckHandler.CurrentPage].Keys[selectedIndex].getImageForForm();
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
            if (changes)
            {
                string oldProfileName = deckHandler.CurrentProfile.ProfileName; ;
                if (deckHandler.CurrentProfile.ProfileName != profileTextBox.Text)
                {
                    deckHandler.CurrentProfile.ProfileName = profileTextBox.Text;
                    deckHandler.CurrentProfile.nameChanged = true;
                }
                if (!ProfileManager.saveProfile(deckHandler.CurrentProfile))
                {
                    deckHandler.CurrentProfile.ProfileName = oldProfileName;
                    profileTextBox.Text = oldProfileName;
                }
                changes = false;
            }
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
            changes = true;
        }

        private void profileTextBox_Leave(object sender, EventArgs e)
        {
            if (changes)
            {
                string oldProfileName = deckHandler.CurrentProfile.ProfileName; ;
                if (deckHandler.CurrentProfile.ProfileName != profileTextBox.Text)
                {
                    deckHandler.CurrentProfile.ProfileName = profileTextBox.Text;
                    deckHandler.CurrentProfile.nameChanged = true;
                }
                if (!ProfileManager.saveProfile(deckHandler.CurrentProfile))
                {
                    deckHandler.CurrentProfile.ProfileName = oldProfileName;
                    profileTextBox.Text = oldProfileName;
                }
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

        private void titleTextBox_TextChanged(object sender, EventArgs e)
        {
            deckHandler.CurrentProfile.Pages[deckHandler.CurrentPage].Keys[selectedIndex].Label = titleTextBox.Text;
            changes = true;
        }

        private void titleTextBox_Leave(object sender, EventArgs e)
        {
            if (changes)
            {
                ProfileManager.saveProfile(deckHandler.CurrentProfile);
                changes = false;
            }
        }

        private void titlePosComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            deckHandler.CurrentProfile.Pages[deckHandler.CurrentPage].Keys[selectedIndex].LabelPos = (OpenStreamDeck.ProfileObjects.LabelPosition)titlePosComboBox.SelectedIndex;
            ProfileManager.saveProfile(deckHandler.CurrentProfile);
        }

        private void iconChangeButton_Click(object sender, EventArgs e)
        {
            if (selectedIndex == -1)
            {
                MessageBox.Show("No key selected");
                return;
            }
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG|All files (*.*)|*.*";
            fd.InitialDirectory = String.Format(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OpenStreamDeck/images/");
            fd.Multiselect = false;
            fd.ValidateNames = true;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Image img = Image.FromFile(fd.FileName);
                    if ((img.Height != 72) && (img.Width != 72))
                    {
                        MessageBox.Show("Images must be 72x72. Please resize your image to prevent potential issues!");
                        return;
                    }

                    var newPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\OpenStreamDeck\\images\\" + fd.SafeFileName;
                    if (File.Exists(newPath))
                    {
                        if (newPath == fd.FileName)
                        {

                        }
                        else if (MessageBox.Show("A file with this name already exists in the OpenStreamDeck image cache, would you like to overwrite it?", "Overwrite File", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            File.Delete(newPath);
                            File.Copy(fd.FileName, newPath);
                        }
                    }
                    else
                    {
                        File.Copy(fd.FileName, newPath);
                    }
                    deckHandler.CurrentProfile.Pages[deckHandler.CurrentPage].Keys[selectedIndex].setImage(newPath);
                    keyPictureBoxes[selectedIndex].Image = deckHandler.CurrentProfile.Pages[deckHandler.CurrentPage].Keys[selectedIndex].getImageForForm();
                    iconPreview.Image = deckHandler.CurrentProfile.Pages[deckHandler.CurrentPage].Keys[selectedIndex].getImageForForm();
                    deckHandler.renderPage();
                    ProfileManager.saveProfile(deckHandler.CurrentProfile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("Error: Could not open file. ({0})", ex.Message));
                }
            }
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Profile Files(*.JSON)|*.JSON|All files (*.*)|*.*";
            fd.InitialDirectory = String.Format(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\OpenStreamDeck\\profiles\\");
            fd.Multiselect = false;
            fd.ValidateNames = true;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    deckHandler.CurrentProfile = ProfileManager.loadProfile(fd.FileName);
                    deckHandler.CurrentPage = 0;
                    populateFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("Error: Could not open file. ({0})", ex.Message));
                }
            }
        }
    }
}
