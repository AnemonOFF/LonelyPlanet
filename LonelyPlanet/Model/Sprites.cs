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


        public static readonly Bitmap[] PlayerStaying = new Bitmap[] 
        {
            new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "entities", "astronaut", "stay1.png"))),
            new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "entities", "astronaut", "stay1.png"))),
            new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "entities", "astronaut", "stay2.png"))),
            new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "entities", "astronaut", "stay2.png"))),
            new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "entities", "astronaut", "stay3.png"))),
            new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "entities", "astronaut", "stay3.png")))
        };
        public static readonly Bitmap[] PlayerMoving = new Bitmap[]
        {
            new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "entities", "astronaut", "move1.png"))),
            new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "entities", "astronaut", "move2.png"))),
            new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "entities", "astronaut", "move3.png"))),
            new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "entities", "astronaut", "move4.png"))),
            new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "entities", "astronaut", "move5.png"))),
            new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "entities", "astronaut", "move6.png"))),
            new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "entities", "astronaut", "move7.png"))),
            new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "entities", "astronaut", "move8.png"))),
            new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "entities", "astronaut", "move9.png")))
        };
        public static readonly Bitmap[] PlayerFlying = new Bitmap[]
        {
            new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "entities", "astronaut", "fly.png")))
        };
        public static readonly Bitmap[] PlayerFalling = new Bitmap[]
        {
            new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "entities", "astronaut", "fall.png")))
    };
        public static readonly Bitmap PlayerStayingEasterEgg = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "entities", "astronaut", "stayEasterEgg.png")));

        public static readonly Bitmap Miner = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "entities", "miner", "default.png")));

        public static readonly Bitmap ItemRock = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "items", "rock.png")));
        public static readonly Bitmap ItemGoldOre = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "items", "gold_ore.png")));
        public static readonly Bitmap ItemGold = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "items", "gold.png")));

        public static readonly Bitmap GuiGlass = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "gui", "game", "Glass2.png")));
        public static readonly Bitmap GuiMetal = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "gui", "game", "Metal.png")));
        public static readonly Bitmap GuiGlassEffect = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "gui", "game", "GlassMDL.png")));
        public static readonly Bitmap GuiHealth = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "gui", "game", "Health2.png")));
        public static readonly Bitmap GuiHealthIco = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "gui", "game", "health.png")));
        public static readonly Bitmap GuiOxygenIco = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "gui", "game", "o2.png")));
        public static readonly Bitmap GuiOxygen = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "gui", "game", "Oxygen.png")));
        public static readonly Bitmap GuiHotbar = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "gui", "game", "Hotbar.png")));
        public static readonly Bitmap GuiHotbarCurrent = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "gui", "game", "currentHotbar.png")));
        public static readonly Bitmap GuiInventory = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "gui", "game", "inventory.png")));
    }
}
