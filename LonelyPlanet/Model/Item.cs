using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LonelyPlanet.Model
{
    public abstract class Item
    {
        public Bitmap Texture { get; }
        public string Name { get; }
        public abstract bool IsPlaceable { get; }

        public Item(Bitmap texture, string name)
        {
            Texture = texture;
            Name = name;
        }

        public abstract IBlock GetBlock(int x, int y);
    }

    public abstract class Spawner : Item
    {
        public override bool IsPlaceable { get; } = false;
        public abstract Type EntityType { get; }

        public Spawner(Bitmap texture, string name) : base (texture, name)
        {
        }

        public override IBlock GetBlock(int x, int y)
        {
            return null;
        }

        public void SpawnEntity(Map map, Player owner, Coordinate coordinate)
        {
            map.entities.Add((Entity)EntityType.
                GetConstructor(new Type[] { typeof(Player), typeof(string), typeof(Coordinate), typeof(double), typeof(Map)})
                .Invoke(new object[] { owner, "DevMiner", coordinate, 100, map}));
        }
    }

    public class MinerSpawnerItem : Spawner
    {
        public override Type EntityType { get; } = typeof(Miner);

        public MinerSpawnerItem() : base (GameSprites.Miner, "Miner")
        {
        }
    }

    public class RockItem : Item
    {
        public override bool IsPlaceable { get; } = true;

        public RockItem() : base(GameSprites.ItemRock, "Rock")
        {
        }

        public override IBlock GetBlock(int x, int y)
        {
            return new Rock(x, y);
        }
    }

    public class GoldItem : Item
    {
        public override bool IsPlaceable { get; } = false;

        public GoldItem() : base(GameSprites.ItemGold, "Gold")
        {
        }

        public override IBlock GetBlock(int x, int y)
        {
            return null;
        }
    }
    public class GoldOreItem : Item
    {
        public override bool IsPlaceable { get; } = true;

        public GoldOreItem() : base(GameSprites.ItemGoldOre, "Gold Ore")
        {
        }

        public override IBlock GetBlock(int x, int y)
        {
            return new Gold(x, y);
        }
    }
}
