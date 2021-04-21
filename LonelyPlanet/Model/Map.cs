using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LonelyPlanet.Model
{
    enum Direction
    {
        left,
        right
    }

    class Map
    {
        private readonly Dictionary<int, IBiome> biomes = new Dictionary<int, IBiome>();
        private int rightBiomeIndex = 0;
        private int leftBiomeIndex = 0;
        public int Width { get; private set; }
        public static readonly Random randomGenerator = new Random();
        public static readonly int chunkSize = 255;

        public Map()
        {
            biomes.Add(0, Biomes.GenerateStartBiome());
            Width = biomes[0].Length;
        }

        public IBiome GenerateNextBiome(Direction direction)
        {
            Chunk chunkReferance;
            int biomeIndex;
            if (direction == Direction.left) {
                chunkReferance = biomes[leftBiomeIndex][0];
                biomeIndex = leftBiomeIndex - 1;
                leftBiomeIndex--;
            }
            else {
                chunkReferance = biomes[rightBiomeIndex][biomes[rightBiomeIndex].Length - 1];
                biomeIndex = rightBiomeIndex + 1;
                rightBiomeIndex++;
            }
            var result = Biomes.GenerateRandomBiome(chunkReferance, biomeIndex);
            biomes.Add(biomeIndex, result);
            Width += result.Length;
            return result;
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
