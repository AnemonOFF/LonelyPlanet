using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LonelyPlanet.Model
{

    public class Map
    {
        private readonly Dictionary<int, IBiome> biomes = new Dictionary<int, IBiome>();
        private readonly Dictionary<int, int> xToBiomeID = new Dictionary<int, int>();
        private int rightBiomeIndex = 0;
        private int leftBiomeIndex = 0;
        public int Width { get; private set; }
        public static readonly Random randomGenerator = new Random();
        public static readonly int chunkSize = 255;

        public IBiome this[int x]
        {
            get
            {
                return biomes[x];
            }
        }

        public Map()
        {
            biomes.Add(0, Biome.GenerateStartBiome());
            for (int i = biomes[0].LeftX; i < biomes[0].Length + biomes[0].LeftX; i++)
                xToBiomeID[i] = 0;
            Width = biomes[0].Length;
        }

        public Chunk GetChunk(int x)
        {
            var biome = biomes[xToBiomeID[x]];
            return biome[x - biome.LeftX];
        }

        public IBiome GetBiomeByX(int x)
        {
            while (x < biomes[leftBiomeIndex].LeftX)
                GenerateNextBiome(Direction.left);
            while (x > biomes[rightBiomeIndex].LeftX + biomes[rightBiomeIndex].Length)
                GenerateNextBiome(Direction.right);
            return biomes[xToBiomeID[x]];
        }

        public void GenerateNextBiome(Direction direction)
        {
            Chunk chunkReferance;
            int biomeIndex;
            int x;
            if (direction == Direction.left)
            {
                chunkReferance = biomes[leftBiomeIndex][0];
                x = chunkReferance.X - 1;
                biomeIndex = leftBiomeIndex - 1;
                leftBiomeIndex--;
            }
            else if (direction == Direction.right)
            {
                chunkReferance = biomes[rightBiomeIndex][biomes[rightBiomeIndex].Length - 1];
                x = chunkReferance.X + 1;
                biomeIndex = rightBiomeIndex + 1;
                rightBiomeIndex++;
            }
            else
                throw new ArgumentException("direction can be only left or right");
            var result = Biome.GenerateRandomBiome(chunkReferance, x, direction);
            lock (biomes)
            {
                biomes.Add(biomeIndex, result);
            }
            Width += result.Length;
            lock (xToBiomeID)
            {
                for (int i = biomes[biomeIndex].LeftX; i < biomes[biomeIndex].Length + biomes[biomeIndex].LeftX; i++)
                    xToBiomeID[i] = biomeIndex;
            }
        }

        public void PrintMapInFile(string path)
        {
            var sw = new StreamWriter(path, false, Encoding.UTF8);
            var map = new char[Width, chunkSize];
            var line = 0;
            for(int i = leftBiomeIndex; i <= rightBiomeIndex; i++)
            {
                var currentBiome = biomes[i];
                for(int j = 0; j < currentBiome.Length; j++)
                {
                    var currentChunk = currentBiome[j];
                    for(int b = 0; b < chunkSize; b++)
                    {
                        var currentBlock = currentChunk[b];
                        switch (currentBlock.Name)
                        {
                            case "Air":
                                map[line, b] = ' ';
                                break;
                            case "Rock":
                                map[line, b] = '*';
                                break;
                            case "Gold":
                                map[line, b] = 'G';
                                break;
                            default:
                                throw new Exception();
                        }
                    }
                    line++;
                }
            }
            for(int i = map.GetLength(1) - 1; i >= 0; i--)
            {
                var sb = new StringBuilder();
                for (int j = 0; j < map.GetLength(0); j++)
                    sb.Append(map[j, i]);
                sw.WriteLine(sb.ToString());
            }
            sw.Close();
        }
    }
}
