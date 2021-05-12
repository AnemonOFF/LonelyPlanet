using LonelyPlanet.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LonelyPlanet.View
{
    class Renderer
    {
        public Size BlockSize { get; }

        public Renderer(Size blockSize)
        {
            BlockSize = blockSize;
        }

        public void RenderBiome(IBiome biome)
        {
            if (biome.Render is null)
                biome.Render = new Bitmap(biome.Length * BlockSize.Width, Map.chunkSize * BlockSize.Height);
            var graphics = Graphics.FromImage(biome.Render);
            for (int i = 0; i < biome.Length; i++)
            {
                RenderChunk(graphics, biome[i], i);
            }
            graphics.Dispose();
        }

        private void RenderChunk(Graphics graphics, Chunk chunk, int chunkNumber)
        {
            var x = chunkNumber * BlockSize.Width;
            for (var y = 0; y < Map.chunkSize; y++)
            {
                var block = chunk[y];
                RenderBlock(graphics, block.Texture, x, (Map.chunkSize - y) * BlockSize.Height);
            }
        }

        private void RenderBlock(Graphics graphics, Image texture, int x, int y)
        {
            graphics.DrawImage(texture, x, y, BlockSize.Width, BlockSize.Height);
        }
    }
}
