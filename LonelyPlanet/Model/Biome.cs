﻿using System;
using System.Collections.Generic;
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

        public static IBiome GenerateRandomBiome(Chunk referance, int x)
        {
            var probability = Map.randomGenerator.NextDouble();
            switch (
                BiomesProbability
                    .SkipWhile(type => type.Item1 < probability)
                    .First()
                    .Item2
                )
            {
                case BiomeTypes.Plain:
                    return new Plain(referance, x, Map.randomGenerator.Next(200, 400));
                case BiomeTypes.Mountain:
                    return new Mountain(referance, x, Map.randomGenerator.Next(50, 100));
                case BiomeTypes.Crater:
                    return new Crater(referance, x, Map.randomGenerator.Next(50, 100));
                default:
                    throw new ArgumentException("Wrong biome type, I have no idea how this exception can be thrown, probably that can happaned with future updates");
            }
        }

        public static IBiome GenerateStartBiome()
        {
            var firstBlocks = new IBlock[100];
            for (int i = 0; i < 100; i++)
                firstBlocks[i] = new Rock(0, i);
            return new Plain(new Chunk(0, 100, firstBlocks), -100, 200);
        }
    }
}