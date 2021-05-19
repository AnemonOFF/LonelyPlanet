using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LonelyPlanet.Model
{
    public class GameSprites
    {
        private static readonly string spritesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Static");
        private static Size SpritesSize => BlockRock.Size;

        public static readonly Bitmap BlockRock = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "blocks", "rock.png")));
        public static readonly Bitmap BlockAir = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "blocks", "air.png")), SpritesSize);
        public static readonly Bitmap BlockGold = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "blocks", "gold.png")), SpritesSize);

        public static readonly Bitmap Player = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "entities", "astronaut", "default.png")));

        public static readonly Bitmap ItemRock = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "items", "rock.png")));
        public static readonly Bitmap ItemGoldOre = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "items", "gold_ore.png")));
        public static readonly Bitmap ItemGold = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "items", "gold.png")));

        public static readonly Bitmap GuiGlass = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "gui", "game", "Glass2.png")));
        public static readonly Bitmap GuiMetal = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "gui", "game", "Metal.png")));
        public static readonly Bitmap GuiGlassEffect = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "gui", "game", "GlassMDL.png")));
        public static readonly Bitmap GuiHealth = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "gui", "game", "Health2.png")));
        public static readonly Bitmap GuiOxygen = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "gui", "game", "Oxygen.png")));
        public static readonly Bitmap GuiHotbar = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "gui", "game", "Hotbar.png")));
    }
}
