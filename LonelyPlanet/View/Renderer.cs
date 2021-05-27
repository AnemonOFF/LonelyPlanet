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
        private readonly Size blockSize;
        private readonly Size hotbarSize = new Size(400, 84);
        private readonly Size hotbarItemSize = new Size(31, 31);
        private readonly Size hotbarCellSize = new Size(58, 56);
        private readonly Size inventorySize = new Size(402, 394);
        private readonly Size inventoryItemSize = new Size(31, 31);
        private readonly int hotbarPxInterval = 31;
        private readonly int hotbarPxCellsInterval = 5;
        private readonly int hotbarPxPadding = 27;
        private readonly int hotbarPxCellsPadding = 13;
        private readonly int inventoryPxInterval = 31;
        private readonly int inventoryPxPaddingX = 27;
        private readonly int inventoryPxPaddingY = 75;

        public Renderer(Size blockSize)
        {
            this.blockSize = blockSize;
        }

        public void RenderInventory(Inventory inventory)
        {
            inventory.Render = new Bitmap(inventorySize.Width, inventorySize.Height);
            lock (inventory.Render)
            {
                var graphics = Graphics.FromImage(inventory.Render);
                graphics.DrawImage(GameSprites.GuiInventory, 0, 0, inventorySize.Width, inventorySize.Height);
                var row = 0;
                for (var i = 0; i < inventory.SolidMaxAmount; i++)
                {
                    var cell = inventory.GetCell(i);
                    if (cell is null)
                        continue;
                    var column = row == 0 ? i : i % row;
                    graphics.DrawImage(cell.Item.Texture,
                        inventoryPxPaddingX + inventoryItemSize.Width * column + inventoryPxInterval * column,
                        inventoryPxPaddingY + inventoryItemSize.Height * row + inventoryPxInterval * row,
                        inventoryItemSize.Width,
                        inventoryItemSize.Height);
                    graphics.DrawString(cell.Count.ToString(),
                        new Font("Aria", 12),
                        Brushes.Goldenrod,
                        new Point(
                            inventoryPxPaddingX + inventoryItemSize.Width * column + inventoryPxInterval * column,
                            inventoryPxPaddingY + inventoryItemSize.Height * row + inventoryPxInterval * row)
                        );
                }
            }
        }

        public void RenderHotbar(Inventory inventory, Player player)
        {
            inventory.HotbarRender = new Bitmap(hotbarSize.Width, hotbarSize.Height);
            lock (inventory.HotbarRender)
            {
                var graphics = Graphics.FromImage(inventory.HotbarRender);
                graphics.DrawImage(GameSprites.GuiHotbar, 0, 0, hotbarSize.Width, hotbarSize.Height);
                for(var i = 0; i < inventory.HotBarMaxAmount; i++)
                {
                    var cell = inventory.GetCell(i);
                    if(player.CurrentHotBar == i)
                    {
                        graphics.DrawImage(GameSprites.GuiHotbarCurrent,
                            hotbarPxCellsPadding + hotbarCellSize.Width * i + hotbarPxCellsInterval * i,
                            hotbarPxCellsPadding,
                            hotbarCellSize.Width,
                            hotbarCellSize.Height);
                    }
                    if (cell is null)
                        continue;
                    graphics.DrawImage(cell.Item.Texture,
                        hotbarPxPadding + hotbarItemSize.Width * i + hotbarPxInterval * i,
                        hotbarPxPadding,
                        hotbarItemSize.Width,
                        hotbarItemSize.Height);
                    graphics.DrawString(cell.Count.ToString(),
                        new Font("Arial", 12),
                        Brushes.Goldenrod,
                        new Point(hotbarPxPadding + hotbarItemSize.Width * i + hotbarPxInterval * i, hotbarPxPadding));
                }
            }
        }

        public void RenderBiome(IBiome biome)
        {
            biome.NeedToRender = false;
            if (biome.Render != null)
                biome.Render.Dispose();
            biome.Render = new Bitmap(biome.Length * blockSize.Width, Map.chunkSize * blockSize.Height);
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
            var x = chunkNumber * blockSize.Width;
            for (var y = 0; y < Map.chunkSize; y++)
            {
                var block = chunk[y];
                if (block.Name == "Air")
                    continue;
                lock (block.Texture)
                {
                    RenderBlock(graphics, block.Texture, x, (Map.chunkSize - y) * blockSize.Height);
                }
            }
        }

        private void RenderBlock(Graphics graphics, Image texture, int x, int y)
        {
            graphics.DrawImage(texture, x, y, blockSize.Width, blockSize.Height);
        }
    }
}
