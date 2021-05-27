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
        private bool isInventoryOpen = false;
        private bool isModalWindowOpen = false;

        protected override void OnMouseClick(MouseEventArgs e)
        {
            DoIfNotModalOpen(() =>
            {
                if (e.Button != MouseButtons.Left)
                    return;
                var inGamePoint = ConvertInGamePoint(new Coordinate(e.X, e.Y));
                game.player.ManipulateBlock(inGamePoint);
            });
        }

        private Point ConvertInGamePoint(Coordinate coordinate)
        {
            var playerPosition = game.player.GetPosition();
            var playerMonitorPosition = new Coordinate((Width - PlayerSize.Width) / 2, (Height + PlayerSize.Height) / 2);
            var deltaMonitor = new Vector(coordinate.X - playerMonitorPosition.X, coordinate.Y - playerMonitorPosition.Y);
            return new Point((int)Math.Floor(playerPosition.X + (deltaMonitor.X / BlockSize.Width)), (int)Math.Floor(playerPosition.Y - (deltaMonitor.Y / BlockSize.Height)));
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            var moveFlag = false;
            switch (e.KeyCode)
            {
                case Keys.D:
                    DoIfNotModalOpen(()=> {
                        game.player.Move(Direction.Right);
                        moveFlag = true;
                    });
                    break;
                case Keys.A:
                    DoIfNotModalOpen(() =>
                    {
                        game.player.Move(Direction.Left);
                        moveFlag = true;
                    });
                    break;
                case Keys.Space:
                    DoIfNotModalOpen(() =>
                    {
                        game.player.Jump();
                    });
                    break;
                case Keys.E:
                    isInventoryOpen = !isInventoryOpen;
                    UpdateModalWindowState();
                    break;
                case Keys.F3:
                    DoIfNotModalOpen(() =>
                    {
                        devInfoFlag = !devInfoFlag;
                    });
                    break;
                case Keys.D1:
                    DoIfNotModalOpen(() => SetCurrentHotbar(0));
                    break;
                case Keys.D2:
                    DoIfNotModalOpen(() => SetCurrentHotbar(1));
                    break;
                case Keys.D3:
                    DoIfNotModalOpen(() => SetCurrentHotbar(2));
                    break;
                case Keys.D4:
                    DoIfNotModalOpen(() => SetCurrentHotbar(3));
                    break;
                case Keys.D5:
                    DoIfNotModalOpen(() => SetCurrentHotbar(4));
                    break;
                case Keys.D6:
                    DoIfNotModalOpen(() => SetCurrentHotbar(5));
                    break;
            }

            if (moveFlag)
                CheckForBiomesRelevance();
        }

        private void SetCurrentHotbar(int index)
        {
            game.player.CurrentHotBar = index;
            game.player.Inventory.HotbarRender = null;
        }

        private delegate void ControllerAction();

        private void DoIfNotModalOpen(ControllerAction action)
        {
            if (!isModalWindowOpen)
                action.Invoke();
        }

        private void UpdateModalWindowState()
        {
            isModalWindowOpen = isInventoryOpen;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D:
                    game.player.ResetMovementSpeed(Direction.Horizontal);
                    break;
                case Keys.A:
                    game.player.ResetMovementSpeed(Direction.Horizontal);
                    break;
            }
        }
    }
}
