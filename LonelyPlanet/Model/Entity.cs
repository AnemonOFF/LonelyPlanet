using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Timers;

namespace LonelyPlanet.Model
{
    public class Entity
    {
        protected const int updateInterval = 16;
        protected Vector movementSpeed;
        protected double maxMovementSpeed = 5;
        protected Timer updateTimer = new Timer { Interval = updateInterval, AutoReset = true};
        protected Coordinate position;
        protected Vector speed;
        protected Map map;
        protected List<EntityForce> forces;
        protected bool isOnGround = false;

        public delegate void EntityEventHandler(Entity entity);
        public event EntityEventHandler HealthChanged;

        public string Name { get; }
        public double Health { get; protected set; } = 100;
        public double Armor { get; protected set; } = 0;
        public double Mass { get; protected set; }
        public bool IsDead { get; protected set; } = false;

        public Entity(string name, Coordinate position, double mass, Map map)
        {
            this.map = map;
            this.position = new Coordinate (position.X, position.Y);
            Mass = mass;
            Name = name;

            speed = new Vector(0, 0);
            movementSpeed = new Vector(0, 0);
            forces = new List<EntityForce>
            {
                (m, p) => new Vector(0, -m * Game.g)
            };

            updateTimer.Elapsed += (sender, args) => { UpdateSpeed(); Move(); };
            updateTimer.Start();
        }

        public void Hit(double damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Health = 0;
                IsDead = true;
            }
            HealthChanged?.Invoke(this);
        }

        public void Heal(double heal)
        {
            Health += heal;
            if (Health > 0)
                IsDead = false;
            HealthChanged?.Invoke(this);
        }

        protected void UpdateSpeed()
        {
            var t = updateInterval / 1000.0;
            var resultantForces = new Vector();
            foreach (var force in forces)
                resultantForces += force.Invoke(Mass, position);
            speed += resultantForces / Mass * t;
        }

        protected void Move()
        {
            lock (position)
            {
                var t = updateInterval / 1000.0;
                int nextX;
                int nextY;
                if (speed.X > 0)
                    nextX = (int)Math.Ceiling(position.X + speed.X * t);
                else
                    nextX = (int)Math.Floor(position.X + speed.X * t);
                if (speed.Y > 0)
                    nextY = (int)Math.Ceiling(position.Y + speed.Y * t);
                else 
                    nextY = (int)Math.Floor(position.Y + speed.Y * t);
                var xChunk = map.GetChunk(nextX);
                var yChunk = map.GetChunk((int)position.X);
                //new Log("nextX: "+nextX+"; nextY: "+nextY+"; posX: "+position.X+"; posY: "+position.Y, name: "pos.log").WriteLog(isAppend: true);
                if (speed.X != 0 &&
                    xChunk[(int)position.Y].Name == "Air" &&
                    ((int)position.Y + 1 >= Map.chunkSize || xChunk[(int)position.Y + 1].Name == "Air"))
                {
                    position.X += speed.X * t;
                } else
                {
                    if (speed.X > 0)
                        position.X = nextX - 1;
                    if (speed.X < 0)
                        position.X = nextX + 1;
                    speed.X = 0;
                    movementSpeed.X = 0;
                }
                if (nextY < 0 || nextY > Map.chunkSize || speed.Y != 0 && yChunk[nextY].Name == "Air")
                {
                    position.Y += speed.Y * t;
                    isOnGround = false;
                } else
                {
                    if (speed.Y > 0)
                        position.Y = nextY - 1;
                    if (speed.Y < 0)
                        position.Y = nextY + 1;
                    speed.Y = 0;
                    isOnGround = true;
                    movementSpeed.Y = 0;
                }
            }
        }

        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.left:
                    if (movementSpeed.X != -maxMovementSpeed)
                    {
                        speed.X -= maxMovementSpeed;
                        movementSpeed.X = -maxMovementSpeed;
                    }
                    break;
                case Direction.right:
                    if (movementSpeed.X != maxMovementSpeed)
                    {
                        speed.X += maxMovementSpeed;
                        movementSpeed.X = maxMovementSpeed;
                    }
                    break;
                default:
                    throw new ArgumentException("Wrong direction. Entity can move only left or right");
            }
        }

        public void ResetMovementSpeed(Direction direction)
        {
            switch (direction)
            {
                case Direction.horizontal:
                    speed.X -= movementSpeed.X;
                    movementSpeed.X = 0;
                    break;
                default:
                    throw new ArgumentException("Wrong direction. Reset can be applied only horizontal");
            }
        }

        public void AddForce(EntityForce force)
            => forces.Add(force);

        public void RemoveForce(EntityForce force)
            => forces.Remove(force);

        public Coordinate GetPosition()
            => new Coordinate(position.X, position.Y);

        public void Jump()
        {
            if(isOnGround)
                speed.Y += 5;
        }
    }
}
