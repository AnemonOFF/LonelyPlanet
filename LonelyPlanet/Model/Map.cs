using System;
using System.Collections.Generic;
using System.Drawing;
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

        public List<Entity> entities;
        public static readonly Random randomGenerator = new Random();
        public static readonly int chunkSize = 255;

        public int Width { get; private set; }

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
            entities = new List<Entity>();
        }

        public void RemoveEntity(Entity entity)
        {
            entities.Remove(entity);
            entity = null;
        }

        public Chunk GetChunk(int x)
        {
            var biome = biomes[xToBiomeID[x]];
            return biome[x - biome.LeftX];
        }

        public IBiome GetBiomeByX(int x)
        {
            while (x < biomes[leftBiomeIndex].LeftX)
                GenerateNextBiome(Direction.Left);
            while (x > biomes[rightBiomeIndex].LeftX + biomes[rightBiomeIndex].Length)
                GenerateNextBiome(Direction.Right);
            return biomes[xToBiomeID[x]];
        }

        public void GenerateNextBiome(Direction direction)
        {
            Chunk chunkReferance;
            int biomeIndex;
            int x;
            if (direction == Direction.Left)
            {
                chunkReferance = biomes[leftBiomeIndex][0];
                x = chunkReferance.X - 1;
                biomeIndex = leftBiomeIndex - 1;
                leftBiomeIndex--;
            }
            else if (direction == Direction.Right)
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

        public delegate bool PathReturnAction(IBlock block);

        public List<Point> GetPath(Point startPoint, PathReturnAction returnAction)
        {
            var queue = new Queue<List<Point>>();
            var visited = new HashSet<Point>();
            queue.Enqueue(new List<Point> { startPoint});
            visited.Add(startPoint);
            while(queue.Count != 0)
            {
                var currentList = queue.Dequeue();
                var currentPoint = currentList.Last();
                if (returnAction.Invoke(GetChunk(currentPoint.X)[currentPoint.Y]))
                    return currentList;
                for(var dy = -1; dy <= 1; dy++)
                {
                    for(var dx = -1; dx <= 1; dx++)
                    {
                        if (dx != 0 && dy != 0)
                            continue;
                        var nextPoint = new Point(currentPoint.X + dx, currentPoint.Y + dy);
                        if (!IsPointCorrect(nextPoint, visited))
                            continue;
                        visited.Add(nextPoint);
                        var nextList = new List<Point>(currentList)
                        {
                            nextPoint
                        };
                        queue.Enqueue(nextList);
                    }
                }
            }
            return null;
        }

        private bool IsPointCorrect(Point point, HashSet<Point> visited)
        {
            if (point.X < biomes[leftBiomeIndex].LeftX ||
                point.X >= biomes[rightBiomeIndex].LeftX + biomes[rightBiomeIndex].Length)
                return false;
            if (point.Y < 0 || point.Y >= chunkSize)
                return false;
            if (visited.Contains(point))
                return false;
            var chunk = GetChunk(point.X);
            return chunk[point.Y].IsBreakable ||
                (point.Y > 1 && chunk[point.Y].Name == "Air" && chunk[point.Y - 1].Name != "Air");
        }
    }
}
