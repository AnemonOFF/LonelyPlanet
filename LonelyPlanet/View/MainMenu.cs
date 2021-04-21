using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Windows.Media;
using Color = System.Drawing.Color;
using System.Diagnostics;

namespace LonelyPlanet.View
{
    public partial class MainMenu : UserControl
    {
        //private readonly MediaPlayer musicPlayer = new MediaPlayer();
        public event Action<Screen> ChangeScreen;

        public MainMenu()
        {
            InitializeComponent();
            CreateMenuGUI();
            var map = new Model.Map();
            map.PrintMapInFile(@"map.txt");
        }

        //private void PlayBackgroundMusic()
        //{
        //    var music = new Uri(@"sounds\music\menu.wav", UriKind.Relative);
        //    musicPlayer.Open(music);
        //    musicPlayer.Play();
        //    musicPlayer.MediaEnded += (sender, e) => {
        //        musicPlayer.Open(music);
        //        musicPlayer.Play();
        //    };
        //}

        private void CreateMenuGUI()
        {
            var ico = new PictureBox
            {
                Name = "ico",
                BackgroundImage = Properties.Resources.logo,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                BackgroundImageLayout = ImageLayout.Zoom,
                Width = 100,
                Height = 100,
                Cursor = Cursors.Hand
            };
            ico.MouseClick += (sender, e) => Process.Start("http://github.com/anemonoff");
            var logo = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackgroundImage = Properties.Resources.lonelyplanet,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            var loadGameButton = CreateMenuButton("Load Game", Screen.Loading);
            var newGameButton = CreateMenuButton("New Game", Screen.Loading);
            var multiplayerButton = CreateMenuButton("Multiplayer", Screen.Multiplayer);
            var settingsButton = CreateMenuButton("Settings", Screen.Settings);
            table.Controls.Add(logo, 1, 1);
            table.Controls.Add(loadGameButton, 1, 2);
            table.Controls.Add(newGameButton, 1, 3);
            table.Controls.Add(multiplayerButton, 1, 4);
            table.Controls.Add(settingsButton, 1, 5);
            table.Controls.Add(ico, 2, 6);
        }

        private Button CreateMenuButton(string text, Screen screen)
        {
            var button = new Button
            {
                Text = text,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Staatliches", 16F, FontStyle.Regular),
                Dock = DockStyle.Fill,
                ForeColor = Color.FromArgb(255, 243, 137),
                //BackgroundImage = Properties.Resources.buttonbg,
                BackColor = Color.Transparent,
                BackgroundImageLayout = ImageLayout.Stretch,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Height = 45,
                Margin = new Padding(5)
            };
            button.MouseEnter += (sender, e) =>
            {
                button.ForeColor = Color.Black;
                button.BackColor = Color.FromArgb(255, 243, 137);
            };
            button.MouseLeave += (sender, e) => {
                button.ForeColor = Color.FromArgb(255, 243, 137);
                button.BackColor = Color.Transparent;
            };
            button.MouseClick += (sender, e) => {
                ChangeScreen?.Invoke(screen);
                };
            return button;
        }
    }
}