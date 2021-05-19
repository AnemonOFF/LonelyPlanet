using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LonelyPlanet.Model
{
    public class Chunk
    {
        private readonly IBlock[] blocks = new IBlock[Map.chunkSize];

        public int X { get; }
        public int Height { get; }

        public IBlock this[int index]
        {
            get
            {
                return blocks[index];
            }
        }

        public Chunk(int x, int height)
        {
            X = x;
            Height = height;
            for (int i = 0; i < Map.chunkSize; i++)
                blocks[i] = new Air(x, i);
        }

        public Chunk(int x, int height, IBlock[] blocks)
        {
            X = x;
            Height = height;
            if (blocks.Length == Map.chunkSize)
                this.blocks = blocks.ToArray();
            else
                for (int i = 0; i < Map.chunkSize; i++)
                {
                    if (i < blocks.Length)
                        this.blocks[i] = blocks[i];
                    else
                        this.blocks[i] = new Air(x, i);
                }
        }

        public void ChangeBlock(IBlock newBlock, int y)
        {
            blocks[y] = newBlock;
        }
    }
}
