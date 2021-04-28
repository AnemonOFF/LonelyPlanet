using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LonelyPlanet.Model
{
    public class Player : Entity
    {
        public double Oxygen { get; private set; }

        public Player(string name, Coordinate position, double mass, Map map) : base(name, position, mass, map)
        {
            Health = Attributes.Player.Get("MaxHealth");
            Oxygen = Attributes.Player.Get("MaxOxygen");
        }
    }
}
