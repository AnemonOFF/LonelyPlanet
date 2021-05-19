using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LonelyPlanet.Model;

namespace LonelyPlanet.View
{
    public partial class GameScreen : UserControl
    {
        private Bitmap healthBitmap;
        private Bitmap oxygenBitmap;

        private void DrawGUI(Graphics graphics)
        {
            DrawProperties(graphics);
            DrawHotbar(graphics);
        }

        private void DrawHotbar(Graphics graphics)
        {
            var inventory = game.player.Inventory;
            if (inventory.HotbarRender is null)
                renderer.RenderHotbar(inventory);
            lock (inventory.HotbarRender)
            {
                graphics.DrawImage(inventory.HotbarRender, 10, 10);
            }
        }

        private void DrawProperties(Graphics graphics)
        {
            if (healthBitmap is null)
            {
                healthBitmap = GetPropertyBitmap((int)game.player.Health, GameSprites.GuiHealth);
                oxygenBitmap = GetPropertyBitmap((int)game.player.Oxygen, GameSprites.GuiOxygen);
                game.player.OxygenChanged += (entity) => {
                    var player = (Player)entity;
                    healthBitmap = GetPropertyBitmap((int)(100 / player.MaxHealth * player.Health), GameSprites.GuiHealth);
                    oxygenBitmap = GetPropertyBitmap((int)(100 / player.MaxOxygen * player.Oxygen), GameSprites.GuiOxygen);
                };
            }
            graphics.DrawImage(healthBitmap, new Point(10, Height - 10 - 256));
            graphics.DrawImage(oxygenBitmap, new Point(50, Height - 10 - 256));
        }

        private Bitmap GetPropertyBitmap(int procent, Bitmap insideImage)
        {
            var bitmap = new Bitmap(32, 256);
            var graphics = Graphics.FromImage(bitmap);
            graphics.DrawImage(GameSprites.GuiGlass, 0, 0, 32, 256);
            var height = (int)(2.56 * procent);
            if(height > 0)
                graphics.DrawImage(insideImage.Clone(new Rectangle(0, 256 - height, 32, height), insideImage.PixelFormat), 0, 256 - height, 32, height);
            graphics.DrawImage(GameSprites.GuiGlassEffect, 0, 0, 32, 256);
            graphics.DrawImage(GameSprites.GuiMetal, 0, 0, 32, 256);
            return bitmap;
        }
    }
}
