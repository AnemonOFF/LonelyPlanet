using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace LonelyPlanet.Model
{
    public class Coordinate
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    public class Entity
    {
        protected const int updateInterval = 100;
        protected Timer updateTimer = new Timer { Interval = updateInterval};
        protected Coordinate position;
        protected Vector speed;
        protected Map map;
        protected List<EntityForce> forces;

        public string Name { get; }
        public double Health { get; protected set; } = 0;
        public double Armor { get; protected set; } = 0;
        public double Mass { get; protected set; }

        public Entity(string name, Coordinate position, double mass, Map map)
        {
            this.map = map;
            Mass = mass;
            this.position = new Coordinate { X = position.X, Y = position.Y };
            Name = name;

            speed = new Vector(0, 0);
            forces = new List<EntityForce>();
            forces.Add((m, p) => new Vector(0, -m * Game.g));

            updateTimer.Tick += (sender, args) => { UpdateSpeed(); Move(); };
            updateTimer.Start();
        }

        protected void UpdateSpeed()
        {
            var t = 1000 / updateInterval;
            var resultantForces = new Vector();
            foreach (var force in forces)
                resultantForces += force.Invoke(Mass, position);
            speed += resultantForces / Mass * t;
        }

        protected void Move()
        {
            lock (position)
            {
                var t = 1000 / updateInterval;
                if(map.GetChunk((int)(position.X + speed.X * t))[(int)position.Y].Name == "Air")
                    position.X += speed.X * t;
                if(map.GetChunk((int)position.X)[(int)(position.Y + speed.Y * t)].Name == "Air")
                    position.Y += speed.Y * t;
            }
        }

        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.left:
                    speed.X -= 1.7;
                    break;
                case Direction.right:
                    speed.X += 1.7;
                    break;
                default:
                    throw new ArgumentException("Wrong direction. Entity can move only left or right");
            }
        }

        public void AddForce(EntityForce force)
            => forces.Add(force);

        public void RemoveForce(EntityForce force)
            => forces.Remove(force);

        public Coordinate GetPosition()
            => new Coordinate { X = position.X, Y = position.Y };

        public void Jump()
        {
            speed.Y += 2;
        }
    }
}
