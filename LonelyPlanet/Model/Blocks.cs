using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LonelyPlanet.Model
{
    public static class Block
    {
        private enum BlockTypes
        {
            Air,
            Rock,
            Gold
        }

        private static readonly Tuple<double, BlockTypes>[] undergroundBlocksProbability = new Tuple<double, BlockTypes>[] {
            new Tuple<double, BlockTypes>(0.9, BlockTypes.Rock),
            new Tuple<double, BlockTypes>(1, BlockTypes.Gold)
        };

        public static IBlock GetRandomUndergroundBlock(int x, int y)
        {
            var probability = Map.randomGenerator.NextDouble();
            switch(
                undergroundBlocksProbability
                    .SkipWhile(type => type.Item1 < probability)
                    .First()
                    .Item2
                )
            {
                case BlockTypes.Rock:
                    return new Rock(x, y);
                case BlockTypes.Gold:
                    return new Gold(x, y);
                default:
                    throw new ArgumentException("Wrong block type for underground");
            }
        }
    }

    public interface IBlock
    {
        Item Drop { get; }
        string Name { get; }
        Point Location { get; }
        Bitmap Texture { get; }
        bool IsBreakable { get; }
    }

    public class Air : IBlock
    {
        public Item Drop { get; }
        public string Name { get; } = "Air";
        public Point Location { get; }
        public Bitmap Texture { get; } = GameSprites.BlockAir;
        public bool IsBreakable { get; } = false;

        public Air(int x, int y)
        {
            Location = new Point(x, y);
        }
    }

    public class Rock : IBlock
    {
        public Item Drop { get; } = new RockItem();
        public string Name { get; } = "Rock";
        public Point Location { get; }
        public Bitmap Texture { get; } = GameSprites.BlockRock;
        public bool IsBreakable { get; } = true;

        public Rock(int x, int y)
        {
            Location = new Point(x, y);
        }
    }

    public class Gold : IBlock
    {
        public Item Drop { get; } = new GoldOreItem();
        public string Name { get; } = "Gold";
        public Point Location { get; }
        public Bitmap Texture { get; } = GameSprites.BlockGold;
        public bool IsBreakable { get; } = true;

        public Gold(int x, int y)
        {
            Location = new Point(x, y);
        }
    }
}
