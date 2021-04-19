using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LonelyPlanet
{
    public partial class LPForm : Form
    {
        public LPForm()
        {
            InitializeComponent();
            ShowMainMenu();
        }

        public void ShowMainMenu()
        {
            HideAllWindows();
            mainMenu.Show();
        }

        public void ShowLoadingScreen()
        {
            HideAllWindows();
        }

        private void HideAllWindows()
        {
            mainMenu.Hide();
        }
    }
}
