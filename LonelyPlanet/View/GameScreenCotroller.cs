using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LonelyPlanet.Model;

namespace LonelyPlanet.View
{
    public partial class GameScreen : UserControl
    {
        protected override void OnKeyDown(KeyEventArgs e)
        {
            var moveFlag = false;
            switch (e.KeyCode)
            {
                case Keys.D:
                    game.player.Move(Direction.right);
                    moveFlag = true;
                    break;
                case Keys.A:
                    game.player.Move(Direction.left);
                    moveFlag = true;
                    break;
                case Keys.Space:
                    game.player.Jump();
                    break;
            }

            if (moveFlag)
                CheckForBiomesRelevance();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D:
                    game.player.ResetMovementSpeed(Direction.horizontal);
                    break;
                case Keys.A:
                    game.player.ResetMovementSpeed(Direction.horizontal);
                    break;
            }
        }
    }
}
