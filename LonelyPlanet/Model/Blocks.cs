using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LonelyPlanet.Model
{
    static class Block
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

    interface IBlock
    {
        string Name { get; }
        Image Image { get; }
        Point Location { get; }
    }

    class Air : IBlock
    {
        public string Name { get; } = "Air";
        public Image Image { get; }
        public Point Location { get; }

        public Air(int x, int y)
        {
            Location = new Point(x, y);
        }
    }

    class Rock : IBlock
    {
        public string Name { get; } = "Rock";
        public Image Image { get; }
        public Point Location { get; }

        public Rock(int x, int y)
        {
            Location = new Point(x, y);
        }
    }

    class Gold : IBlock
    {
        public string Name { get; } = "Gold";
        public Image Image { get; }
        public Point Location { get; }

        public Gold(int x, int y)
        {
            Location = new Point(x, y);
        }
    }
}
