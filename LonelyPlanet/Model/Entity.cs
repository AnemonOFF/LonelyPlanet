using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Timers;
using System.Drawing;
using Size = System.Drawing.Size;

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
        public event EntityEventHandler HealthChangedEvent;
        public event EntityEventHandler FallEvent;
        public event EntityEventHandler JumpEvent;
        public event EntityEventHandler DamageEvent;

        public Bitmap Texture { get; protected set; }
        public Size Size { get; }
        public Direction MovingDirection { get; protected set; } = Direction.Right;
        public string Name { get; }
        public double Health { get; protected set; } = 100;
        public double Armor { get; protected set; } = 0;
        public double Mass { get; protected set; }
        public bool IsDead { get; protected set; } = false;
        public bool IsFlying { get; protected set; } = false;
        public bool IsFalling { get; protected set; } = false;
        public bool IsMoving { get; protected set; } = false;

        public Entity(string name, Coordinate position, double mass, Map map, Size size)
        {
            this.map = map;
            this.position = new Coordinate (position.X, position.Y);
            Mass = mass;
            Name = name;
            Size = size;

            speed = new Vector(0, 0);
            movementSpeed = new Vector(0, 0);
            forces = new List<EntityForce>
            {
                (m, p) => new Vector(0, -m * Game.g)
            };

            updateTimer.Elapsed += (sender, args) => {
                UpdateSpeed();
                Move();
            };
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
            HealthChangedEvent?.Invoke(this);
            DamageEvent?.Invoke(this);
        }

        public void Heal(double heal)
        {
            Health += heal;
            if (Health > 0)
                IsDead = false;
            HealthChangedEvent?.Invoke(this);
        }

        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    if (movementSpeed.X != -maxMovementSpeed)
                    {
                        speed.X -= maxMovementSpeed;
                        movementSpeed.X = -maxMovementSpeed;
                    }
                    break;
                case Direction.Right:
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
                case Direction.Horizontal:
                    speed.X -= movementSpeed.X;
                    movementSpeed.X = 0;
                    break;
                default:
                    throw new ArgumentException("Wrong direction. Reset can be applied only horizontal");
            }
        }

        public void AddForce(EntityForce force) => forces.Add(force);

        public void RemoveForce(EntityForce force) => forces.Remove(force);

        public Coordinate GetPosition() => new Coordinate(position.X, position.Y);

        public void SetPosition(Coordinate coordinate) => position = coordinate;

        public void Jump()
        {
            if(isOnGround)
                speed.Y += 5;
        }

        protected void UpdateSpeed()
        {
            var t = updateInterval / 1000.0;
            var resultantForces = new Vector();
            foreach (var force in forces)
                resultantForces += force.Invoke(Mass, position);
            speed += resultantForces / Mass * t;
        }

        private void Move()
        {
            lock (position)
            {
                var t = updateInterval / 1000.0;
                MoveX(t);
                if (speed.Y != 0)
                {
                    var nextY = speed.Y < 0 ? (int)Math.Floor(position.Y + speed.Y * t) : (int)Math.Ceiling(position.Y + speed.Y * t);
                    var sizeDelta = speed.Y < 0 ? 0 : Size.Height - 1;
                    var canGo = true;
                    for (var i = 0; i < Size.Width; i++)
                    {
                        var chunk = map.GetChunk((int)Math.Floor(position.X + i));
                        if (nextY >= 0 && nextY < Map.chunkSize && chunk[nextY].Name != "Air")
                            canGo = false;
                    }
                    if (nextY + sizeDelta >= 0 && nextY + sizeDelta < Map.chunkSize && map.GetChunk((int)Math.Ceiling(position.X + Size.Width - 1))[nextY + sizeDelta].Name != "Air")
                        canGo = false;
                    if (canGo)
                    {
                        position.Y += speed.Y * t;
                        isOnGround = false;
                        if (!IsFlying)
                            JumpEvent?.Invoke(this);
                        IsFlying = true;
                        IsFalling = speed.Y < 0;
                    }
                    else
                    {
                        position.Y = speed.Y < 0 ? nextY + 1 : nextY - 1;
                        speed.Y = 0;
                        movementSpeed.Y = 0;
                        isOnGround = true;
                        if(IsFlying)
                            FallEvent?.Invoke(this);
                        IsFlying = false;
                        IsFalling = false;
                    }
                }
            }
        }

        private void MoveX(double t)
        {
            if (speed.X != 0)
            {
                IsMoving = true;
                MovingDirection = speed.X < 0 ? Direction.Left : Direction.Right;
                var nextX = speed.X < 0 ? (int)Math.Floor(position.X + speed.X * t) : (int)Math.Ceiling(position.X + speed.X * t);
                var sizeDelta = speed.X < 0 ? 0 : Size.Width - 1;
                var chunk = map.GetChunk(nextX + sizeDelta);
                var canGo = true;
                for (var i = 0; i < Size.Height; i++)
                {
                    if (position.Y + i < Map.chunkSize && position.Y + i >= 0 && chunk[(int)Math.Floor(position.Y + i)].Name != "Air")
                        canGo = false;
                }
                if (canGo)
                    position.X += speed.X * t;
                else
                {
                    position.X = speed.X < 0 ? nextX + 1 : nextX - 1;
                    speed.X = 0;
                    movementSpeed.X = 0;
                    IsMoving = false;
                }
            }
            else
                IsMoving = false;
        }
    }
}
