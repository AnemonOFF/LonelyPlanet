using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LonelyPlanet.Model
{
    class Mountain : IBiome
    {
        private readonly Chunk[] chunks;
        private readonly int startedHeight;
        private readonly int topHeight;
        private const int MaxHeightDifference = 4;

        public string Name { get; } = "Mountain";
        public int Length { get; }
        public int LeftX { get; }
        public Bitmap Render { get; set; }
        public bool NeedToRender { get; set; } = false;

        public Chunk this[int index]
        {
            get
            {
                return chunks[index];
            }
        }

        public Mountain(Chunk referance, int x, int length)
        {
            LeftX = x;
            Length = length;
            startedHeight = referance.Height;
            topHeight = startedHeight + Map.randomGenerator.Next(startedHeight + 50, startedHeight + 150);
            if (topHeight > Map.chunkSize)
                topHeight = Map.chunkSize;
            chunks = new Chunk[Length];
            var heights = GenerateChunksHeight();
            for (var i = 0; i < Length; i++)
                chunks[i] = GenerateChunk(LeftX + i, heights[i]);
        }

        private int[] GenerateChunksHeight()
        {
            var halfWidth = Length / 2 - Map.randomGenerator.Next(1, 5);
            var chunks = new int[Length];
            var firstSlope = GenerateSlope(halfWidth, startedHeight, topHeight);
            for (var i = 0; i < halfWidth; i++)
                chunks[i] = firstSlope[i];
            for (var i = halfWidth; i < Length - halfWidth; i++)
                chunks[i] = chunks[i - 1];
            var secondSlope = GenerateSlope(halfWidth, topHeight, startedHeight);
            for (var i = 0; i < halfWidth; i++)
                chunks[i + Length - halfWidth] = secondSlope[i];
            return chunks;
        }

        private int[] GenerateSlope(int length, int start, int end)
        {
            var needToReverse = false;
            if (end < start)
            {
                (start, end) = (end, start);
                needToReverse = true;
            }
            var chunks = new int[length];
            chunks[length - 1] = end;
            for (var i = Length - 2; i >= 0; i--)
            {
                chunks[i] = chunks[i - 1] - MaxHeightDifference;
                if (chunks[i] < start)
                    chunks[i] = start;
            }
            int maxRandomLength = Length - 1;
            for(var i = start + 1; i < end; i++)
            {
                var randomLength = Map.randomGenerator.Next(1, maxRandomLength);
                for(var h = Length - 1; h > Length - 1 - randomLength && h > 1; h--)
                {
                    if(chunks[h] < i && chunks[h] - 4 < chunks[h - 1])
                        chunks[h] = i;
                }
            }
            return needToReverse ? chunks.Reverse().ToArray() : chunks;
        }

        private Chunk GenerateChunk(int x, int height)
        {
            var blocks = new IBlock[height];
            for (var i = 0; i < height - 10; i++)
                blocks[i] = Block.GetRandomUndergroundBlock(x, i);
            for (var i = height - 10; i < height; i++)
                blocks[i] = new Rock(x, i);
            return new Chunk(x, height, blocks);
        }
    }
}