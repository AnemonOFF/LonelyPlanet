using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace LonelyPlanet
{
    public enum Screen {
        MainMenu,
        Game,
        Multiplayer,
        Settings,
        Loading
    }

    public partial class LPForm : Form
    {
        public static readonly MediaPlayer musicPlayer = new MediaPlayer();
        private UserControl currentScreen;

        private static void PlayBackgroundMusic(Uri music)
        {
            musicPlayer.Open(music);
            musicPlayer.Play();
            musicPlayer.MediaEnded += (sender, e) => {
                musicPlayer.Open(music);
                musicPlayer.Play();
            };
        }

        public static void ChangeBackgroundMusicVolume(int volume)
        {
            if (volume < 0 || volume > 100)
                throw new ArgumentException("Volume have to be in 0 to 100 range");
            musicPlayer.Volume = volume;
        }

        private static void ClearBackgroundMusic()
        {
            musicPlayer.Close();
        }

        public LPForm()
        {
            InitializeComponent();
            //mainMenu.ChangeScreen += OnScreenChange;
            ShowMainMenu();
            PlayBackgroundMusic(new Uri(@"sounds\music\menu.wav", UriKind.Relative));
        }

        private void OnScreenChange(Screen screen)
        {
            Cursor = Cursors.Default;
            HideAllWindows();
            switch (screen)
            {
                case Screen.MainMenu:
                    ShowMainMenu();
                    break;
                case Screen.Loading:
                    ShowLoadingScreen();
                    break;
                case Screen.Game:
                    ShowGame();
                    break;
                default:
                    throw new ArgumentException("Unknown screen");
            }
        }

        private void ShowGame()
        {
            currentScreen = new View.GameScreen();
            ParameteriseAndShowScreen(currentScreen, name: "gameScreen");
            PlayBackgroundMusic(new Uri(@"sounds\music\meet-the-princess.wav", UriKind.Relative));
        }

        private void ShowMainMenu()
        {
            currentScreen = new View.MainMenu();
            ((View.MainMenu)currentScreen).ChangeScreen += OnScreenChange;
            ParameteriseAndShowScreen(currentScreen, bg: Properties.Resources.Nebula_Blue, name: "mainMenu");
            PlayBackgroundMusic(new Uri(@"sounds\music\menu.wav", UriKind.Relative));
        }

        private void ShowLoadingScreen()
        {
            currentScreen = new View.LoadingScreen();
            ParameteriseAndShowScreen(currentScreen, name: "loadingScreen");
            PlayBackgroundMusic(new Uri(@"sounds\music\loading.wav", UriKind.Relative));
        }

        private void ParameteriseAndShowScreen(UserControl screen, Image bg = null, string name = null)
        {
            screen.Dock = DockStyle.Fill;
            screen.BackColor = System.Drawing.Color.Transparent;
            screen.Location = new Point(0, 0);
            if (bg != null)
            {
                screen.BackgroundImage = bg;
                screen.BackgroundImageLayout = ImageLayout.None;
            }
            if (name != null)
                screen.Name = name;
            Controls.Add(screen);
        }

        private void HideAllWindows()
        {
            if (currentScreen != null)
                Controls.Remove(currentScreen);
            ClearBackgroundMusic();
        }
    }
}
