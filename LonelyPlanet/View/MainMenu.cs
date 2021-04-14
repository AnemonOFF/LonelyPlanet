using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LonelyPlanet.View
{
    public partial class MainMenu : UserControl
    {
        public MainMenu()
        {
            InitializeComponent();
            CreateMenuGUI();
        }

        private void CreateMenuGUI()
        {
            var logo = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackgroundImage = Properties.Resources.lonelyplanet,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            var loadGameButton = CreateMenuButton("Load Game");
            var newGameButton = CreateMenuButton("New Game");
            var multiplayerButton = CreateMenuButton("Multiplayer");
            var settingsButton = CreateMenuButton("Settings");
            table.Controls.Add(logo, 1, 1);
            table.Controls.Add(loadGameButton, 1, 2);
            table.Controls.Add(newGameButton, 1, 3);
            table.Controls.Add(multiplayerButton, 1, 4);
            table.Controls.Add(settingsButton, 1, 5);
        }

        private Button CreateMenuButton(string text)
        {
            return new Button
            {
                Text = text,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Staatliches", 15F, FontStyle.Regular),
                Dock = DockStyle.Fill,
                ForeColor = Color.FromArgb(255, 243, 137),
                BackgroundImage = Properties.Resources.buttonbg,
                BackgroundImageLayout = ImageLayout.Stretch,
                FlatStyle = FlatStyle.Flat,
                Height = 40
            };
        }
    }
}
