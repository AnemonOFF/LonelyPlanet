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
        private static readonly string spritesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Static");
        private static Size SpritesSize => BlockRock.Size;

        public static readonly Bitmap BlockRock = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "blocks", "rock.png")));
        public static readonly Bitmap BlockAir = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "blocks", "air.png")), SpritesSize);
        public static readonly Bitmap BlockGold = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "blocks", "gold.png")), SpritesSize);

        public static readonly Bitmap Player = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "entities", "astronaut", "default.png")), SpritesSize);
    }
}
