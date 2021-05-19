using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace LonelyPlanet.Model
{
    public class Player : Entity
    {
        private const int manipulateMaxDistance = 7;

        public event EntityEventHandler OxygenChanged;

        public double Oxygen { get; private set; }
        public double MaxOxygen { get; }
        public double MaxHealth { get; }
        public Inventory Inventory { get; }
        public int CurrentHotBar { get; set; }

        public Player(string name, Coordinate position, double mass, Map map) : base(name, position, mass, map)
        {
            MaxHealth = Attributes.Player.Get("MaxHealth");
            Health = MaxHealth;
            MaxOxygen = Attributes.Player.Get("MaxOxygen");
            Oxygen = MaxOxygen;
            var hotBarMax = (int)Attributes.Player.Get("MaxHotBarAmount");
            var solidMax = (int)Attributes.Player.Get("MaxSolidAmount");
            var liquidMax = (int)Attributes.Player.Get("MaxLiquidAmount");
            Inventory = new Inventory(hotBarMax, solidMax, liquidMax);
            var propertyTimer = new Timer { Interval = 1000, AutoReset = true };
            propertyTimer.Elapsed += (e, a)=> {
                Oxygen -= 1;
                if(Oxygen <= 0)
                {
                    Oxygen = 0;
                    Health--;
                }
                OxygenChanged?.Invoke(this);
            };
            propertyTimer.Start();
        }

        public void ManipulateBlock(Point blockPosition)
        {
            if (blockPosition.Y >= Map.chunkSize)
                return;
            if (Math.Sqrt(Math.Pow(position.X - blockPosition.X, 2) + Math.Pow(position.Y - blockPosition.Y, 2)) > manipulateMaxDistance)
                return;
            var block = map.GetChunk(blockPosition.X)[blockPosition.Y];
            if (block.Name == "Air")
            {
                if (!Inventory.IsCellEmpty(CurrentHotBar))
                {
                    var newBlock = Inventory.GetBlockFromItem(CurrentHotBar, blockPosition.X, blockPosition.Y);
                    if (newBlock != null)
                    {
                        PlaceBlock(newBlock);
                        map.GetBiomeByX(blockPosition.X).NeedToRender = true;
                    }
                }
            }
            else
            {
                DestroyBlock(blockPosition);
                map.GetBiomeByX(blockPosition.X).NeedToRender = true;
            }
        }

        private void DestroyBlock(Point blockPosition)
        {
            var chunk = map.GetChunk(blockPosition.X);
            if(chunk[blockPosition.Y].Drop != null)
                Inventory.AddItem(chunk[blockPosition.Y].Drop);
            chunk.ChangeBlock(new Air(blockPosition.X, blockPosition.Y), blockPosition.Y);
        }

        private void PlaceBlock(IBlock block)
        {
            var chunk = map.GetChunk(block.Location.X);
            chunk.ChangeBlock(block, block.Location.Y);
        }
    }
}
