using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LonelyPlanet.Model
{    
    public class Game
    {
        public const double g = 3.7;
        public readonly Map map;
        public readonly Player player;

        public Game()
        {
            map = new Map();
            player = new Player("Dev", new Coordinate (0, 101), 80, map);
            SetUpGame();
        }

        private void SetUpGame()
        {
            player.Inventory.AddItem(new MinerSpawnerItem());
        }
    }
}
