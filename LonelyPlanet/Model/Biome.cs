using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LonelyPlanet.Model
{
    public interface IBiome
    {
        string Name { get; }
        int Length { get; }
        int LeftX { get; }
        Chunk this[int index] { get; }
        Bitmap Render { get; set; }
    }

    static class Biome
    {
        private enum BiomeTypes
        {
            Plain,
            Mountain,
            Crater
        }

        private static readonly Tuple<double, BiomeTypes>[] BiomesProbability = new Tuple<double, BiomeTypes>[] {
            new Tuple<double, BiomeTypes>(0.5, BiomeTypes.Plain),
            new Tuple<double, BiomeTypes>(0.75, BiomeTypes.Mountain),
            new Tuple<double, BiomeTypes>(1.0, BiomeTypes.Crater)
        };

        public static IBiome GenerateRandomBiome(Chunk referance, int x, Direction direction)
        {
            var probability = Map.randomGenerator.NextDouble();
            //var probability = 0.2;
            //new Log("Probabiliy: "+probability, name: "prob.log").WriteLog(isAppend: true);
            int len;
            switch (
                BiomesProbability
                    .SkipWhile(type => type.Item1 < probability)
                    .First()
                    .Item2
                )
            {
                case BiomeTypes.Plain:
                    len = Map.randomGenerator.Next(200, 400);
                    if (direction == Direction.left)
                        x -= len - 1;
                    return new Plain(referance, x, len);
                case BiomeTypes.Mountain:
                    len = Map.randomGenerator.Next(50, 100);
                    if (direction == Direction.left)
                        x -= len - 1;
                    return new Mountain(referance, x, len);
                case BiomeTypes.Crater:
                    len = Map.randomGenerator.Next(50, 100);
                    if (direction == Direction.left)
                        x -= len - 1;
                    return new Crater(referance, x, len);
                default:
                    throw new ArgumentException("Wrong biome type, I have no idea how this exception can be thrown, probably that can happaned with future updates");
            }
        }

        public static IBiome GenerateStartBiome()
        {
            var firstBlocks = new IBlock[100];
            for (int i = 0; i < 100; i++)
                firstBlocks[i] = new Rock(0, i);
            return new Plain(new Chunk(0, 100, firstBlocks), 0, 200);
        }
    }
}
