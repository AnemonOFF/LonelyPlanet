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
            mainMenu.ChangeScreen += OnScreenChange;
            ShowMainMenu();
            PlayBackgroundMusic(new Uri(@"sounds\music\menu.wav", UriKind.Relative));
        }

        private void OnScreenChange(Screen screen)
        {
            Cursor = Cursors.Default;
            switch (screen)
            {
                case Screen.MainMenu:
                    ShowMainMenu();
                    ClearBackgroundMusic();
                    PlayBackgroundMusic(new Uri(@"sounds\music\menu.wav", UriKind.Relative));
                    break;
                case Screen.Loading:
                    ClearBackgroundMusic();
                    PlayBackgroundMusic(new Uri(@"sounds\music\loading.wav", UriKind.Relative));
                    ShowLoadingScreen();
                    break;
                case Screen.Game:
                    break;
            }
        }

        private void ShowMainMenu()
        {
            HideAllWindows();
            mainMenu.Show();
        }

        private void ShowLoadingScreen()
        {
            HideAllWindows();
            loadingScreen.Show();
        }

        private void HideAllWindows()
        {
            mainMenu.Hide();
            loadingScreen.Hide();
        }
    }
}
