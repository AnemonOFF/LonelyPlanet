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
        private readonly Size BlockSize;
        private readonly Size HotbarSize = new Size(400, 84);
        private readonly Size HotbarItemSize = new Size(31, 31);
        private readonly int HotbarPxInterval = 31;
        private readonly int HotbarPxPadding = 27;

        public Renderer(Size blockSize)
        {
            BlockSize = blockSize;
        }

        public void RenderHotbar(Inventory inventory)
        {
            inventory.HotbarRender = new Bitmap(HotbarSize.Width, HotbarSize.Height);
            lock (inventory.HotbarRender)
            {
                var graphics = Graphics.FromImage(inventory.HotbarRender);
                graphics.DrawImage(GameSprites.GuiHotbar, 0, 0, HotbarSize.Width, HotbarSize.Height);
                for(var i = 0; i < inventory.HotBarMaxAmount; i++)
                {
                    var cell = inventory.GetCell(i);
                    if (cell is null)
                        continue;
                    graphics.DrawImage(cell.Item.Texture,
                        HotbarPxPadding + HotbarItemSize.Width * i + HotbarPxInterval * i,
                        HotbarPxPadding,
                        HotbarItemSize.Width,
                        HotbarItemSize.Height);
                    graphics.DrawString(cell.Count.ToString(),
                        new Font("Arial", 12),
                        Brushes.Goldenrod,
                        new Point(HotbarPxPadding + HotbarItemSize.Width * i + HotbarPxInterval * i, HotbarPxPadding));
                }
            }
        }

        public void RenderBiome(IBiome biome)
        {
            biome.NeedToRender = false;
            if (biome.Render != null)
                biome.Render.Dispose();
            biome.Render = new Bitmap(biome.Length * BlockSize.Width, Map.chunkSize * BlockSize.Height);
            lock (biome.Render)
            {
                var graphics = Graphics.FromImage(biome.Render);
                for (int i = 0; i < biome.Length; i++)
                {
                    RenderChunk(graphics, biome[i], i);
                }
                graphics.Dispose();
            }
        }

        private void RenderChunk(Graphics graphics, Chunk chunk, int chunkNumber)
        {
            var x = chunkNumber * BlockSize.Width;
            for (var y = 0; y < Map.chunkSize; y++)
            {
                var block = chunk[y];
                if (block.Name == "Air")
                    continue;
                lock (block.Texture)
                {
                    RenderBlock(graphics, block.Texture, x, (Map.chunkSize - y) * BlockSize.Height);
                }
            }
        }

        private void RenderBlock(Graphics graphics, Image texture, int x, int y)
        {
            graphics.DrawImage(texture, x, y, BlockSize.Width, BlockSize.Height);
        }
    }
}
