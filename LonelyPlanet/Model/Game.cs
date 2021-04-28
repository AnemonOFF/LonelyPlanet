using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LonelyPlanet.Model
{    
    public class Game
    {
        public const double g = 3.7;
        public readonly Map map;

        public Game()
        {
            map = new Map();
        }
    }
}
