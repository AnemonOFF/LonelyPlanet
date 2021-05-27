using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LonelyPlanet.View
{
    public partial class Settings : UserControl
    {
        private int bgVolume = 50;
        private int soundsVolume = 50;

        public delegate void volumeChange(int bg, int sounds);
        public event Action<Screen> ChangeScreen;
        public event volumeChange ChangeVolume;

        public Settings()
        {
            InitializeComponent();
            CreateMenuGUI();
        }

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
            var labelMusic = CreateLabel("Background music volume");
            var trackMusic = CreateTrackBar();
            trackMusic.ValueChanged += (sender, args) =>
            {
                bgVolume = trackMusic.Value;
                ChangeVolume?.Invoke(bgVolume, soundsVolume);
            };
            var labelSounds = CreateLabel("SFX volume");
            var trackSounds = CreateTrackBar();
            trackMusic.ValueChanged += (sender, args) =>
            {
                soundsVolume = trackSounds.Value;
                ChangeVolume?.Invoke(bgVolume, soundsVolume);
            };
            var loadGameButton = CreateMenuButton("Go back", Screen.MainMenu);
            table.Controls.Add(labelMusic, 1, 1);
            table.Controls.Add(trackMusic, 1, 2);
            table.Controls.Add(labelSounds, 1, 3);
            table.Controls.Add(trackSounds, 1, 4);
            table.Controls.Add(loadGameButton, 1, 5);
        }

        private TrackBar CreateTrackBar()
        {
            var trackBar = new TrackBar
            {
                Dock = DockStyle.Fill,
                Height = 45,
                Margin = new Padding(5),
                TickStyle = TickStyle.None,
                TickFrequency = 1,
                Minimum = 0,
                Maximum = 100,
                Value = 50
            };
            return trackBar;
        }

        private Label CreateLabel(string text)
        {
            return new Label
            {
                Text = text,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Staatliches", 16F, FontStyle.Regular),
                Dock = DockStyle.Fill,
                ForeColor = Color.FromArgb(255, 243, 137),
                BackColor = Color.Transparent,
                BackgroundImageLayout = ImageLayout.Stretch,
                FlatStyle = FlatStyle.Flat,
                Height = 45,
                Margin = new Padding(5)
            };
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
