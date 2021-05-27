using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace LonelyPlanet.Model
{
    enum MovementAction
    {
        Left,
        Right,
        Jump,
        Fall
    }

    public class Miner : Entity
    {
        private readonly Player owner;
        private List<Point> currentPath;
        private Timer pathFindTimer;
        private bool isFinding = false;

        public int GoldLeft = 4;

        public Miner(Player owner, string name, Coordinate position, double mass, Map map) : base(name, position, mass, map, new Size(1, 1))
        {
            this.owner = owner;
            Texture = GameSprites.Miner;
            pathFindTimer = new Timer
            {
                Interval = 10000,
                AutoReset = true
            };
            pathFindTimer.Elapsed += (s, e) => {
                if (currentPath == null && !isFinding && GoldLeft > 0)
                {
                    isFinding = true;
                    SetCurrentPath();
                    isFinding = false;
                    var a = GoToGold();
                }
            };
            pathFindTimer.Start();
        }

        private void SetCurrentPath()
        {
            lock (position)
            {
                currentPath = map.GetPath(new Point((int)position.X, (int)position.Y), (block)=>block.Name == "Gold");
            }
        }

        private async Task GoToGold()
        {
            if (currentPath is null)
                return;
            foreach (var point in currentPath)
                await GoToPoint(point);
            owner.Inventory.AddItem(new GoldItem());
            currentPath = null;
            GoldLeft--;
            if (GoldLeft == 0)
                map.RemoveEntity(this);
        }

        private Task GoToPoint(Point point)
        {
            var task = new Task(()=>
            {
                var wasHorizontalMovement = false;
                if (map.GetChunk(point.X)[point.Y].Name != "Air")
                    DestroyBlock(point);
                var currentPoint = new Point((int)position.X, (int)position.Y);
                if (currentPoint.X < point.X)
                {
                    Move(Direction.Right);
                    wasHorizontalMovement = true;
                }
                else if (currentPoint.X > point.X)
                {
                    Move(Direction.Left);
                    wasHorizontalMovement = true;
                }
                else if (currentPoint.Y < point.Y)
                    Jump();
                while ((int)position.X != point.X || (int)position.Y != point.Y)
                    Task.Delay(100);
                if (wasHorizontalMovement)
                    ResetMovementSpeed(Direction.Horizontal);
            });
            task.Start();
            return task;
        }

        private void DestroyBlock(Point blockPosition)
        {
            var chunk = map.GetChunk(blockPosition.X);
            chunk.ChangeBlock(new Air(blockPosition.X, blockPosition.Y), blockPosition.Y);
            map.GetBiomeByX(blockPosition.X).NeedToRender = true;
        }
    }
}
