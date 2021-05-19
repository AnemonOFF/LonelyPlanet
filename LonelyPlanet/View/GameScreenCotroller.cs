using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using LonelyPlanet.Model;
using Point = System.Drawing.Point;

namespace LonelyPlanet.View
{
    public partial class GameScreen : UserControl
    {
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            var inGamePoint = ConvertInGamePoint(new Coordinate(e.X, e.Y));
            game.player.ManipulateBlock(inGamePoint);
        }

        private Point ConvertInGamePoint(Coordinate coordinate)
        {
            var playerPosition = game.player.GetPosition();
            var playerMonitorPosition = new Coordinate((Width - PlayerSize.Width) / 2, Height / 2);
            var deltaMonitor = new Vector(coordinate.X - playerMonitorPosition.X, coordinate.Y - playerMonitorPosition.Y);
            return new Point((int)playerPosition.X + (int)(deltaMonitor.X / BlockSize.Width), (int)playerPosition.Y - (int)(deltaMonitor.Y / BlockSize.Height));
        }

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
                case Keys.F3:
                    DevInfoFlag = !DevInfoFlag;
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
