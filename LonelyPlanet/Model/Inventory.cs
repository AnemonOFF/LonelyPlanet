using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LonelyPlanet.Model
{
    public class Cell<T>
    {
        public T Item { get; }
        public int Count { get; set; }

        public Cell (T item, int count)
        {
            Item = item;
            Count = count;
        }
    }

    public class Inventory
    {
        private readonly Dictionary<Liquid, int> liquids = new Dictionary<Liquid, int>();
        private readonly Cell<Item>[] items;

        public int SolidMaxAmount { get; }
        public int HotBarMaxAmount { get; }
        public int LiquidMaxAmount { get; }
        public int LiquidAmount { get; private set; } = 0;
        public Bitmap HotbarRender { get; set; }
        public Bitmap Render { get; set; }

        public Inventory(int hotBarAmount, int solidMax, int liquidMax)
        {
            if (hotBarAmount > solidMax)
                throw new ArgumentException("Hot bar items amount can`t be more than solid items amount");
            HotBarMaxAmount = hotBarAmount;
            SolidMaxAmount = solidMax;
            LiquidMaxAmount = liquidMax;
            items = new Cell<Item>[solidMax];
        }

        public bool IsCellEmpty(int index)
        {
            return items[index] is null;
        }

        public IBlock GetBlockFromItem(int cell, int x, int y)
        {
            if (items[cell] is null)
                return null;
            var item = items[cell].Item;
            if (!item.IsPlaceable)
                return null;
            items[cell].Count--;
            var block = item.GetBlock(x, y);
            if (items[cell].Count == 0)
                items[cell] = null;
            HotbarRender = null;
            Render = null;
            return block;
        }

        public Spawner GetSpawnerFromItem(int cell)
        {
            if (items[cell] is null)
                return null;
            var item = items[cell].Item;
            if (!(item is Spawner))
                return null;
            items[cell].Count--;
            if (items[cell].Count == 0)
                items[cell] = null;
            HotbarRender = null;
            Render = null;
            return item as Spawner;
        }

        public bool AddItem(Item item, int cell = -1)
        {
            if(cell == -1)
            {
                cell = FindSuitableCell(item.Name);
                if (cell == -1)
                    return false;
            }

            if (items[cell] != null && item.Name != items[cell].Item.Name)
                return false;

            if (items[cell] is null)
                items[cell] = new Cell<Item>(item, 1);
            else
                items[cell] = new Cell<Item>(item, items[cell].Count + 1);
            HotbarRender = null;
            Render = null;
            return true;
        }

        public IEnumerable<Tuple<Liquid, int>> GetLiquids()
        {
            foreach (var liquid in liquids)
                yield return Tuple.Create(liquid.Key, liquid.Value);
        }

        public IEnumerable<Cell<Item>> GetHotBarCells()
        {
            for(int i = 0; i < HotBarMaxAmount; i++)
                if(items[i] != null)
                    yield return items[i];
        }

        public Cell<Item> GetCell(int index)
        {
            return items[index];
        }

        public IEnumerable<Cell<Item>> GetCells()
        {
            foreach(var item in items)
                yield return item;
        }

        private int FindSuitableCell(string itemName)
        {
            var cell = -1;
            for (int i = 0; i < SolidMaxAmount; i++)
            {
                if (cell == -1 && items[i] is null)
                    cell = i;
                else if (items[i] != null && items[i].Item.Name == itemName)
                {
                    cell = i;
                    break;
                }
            }
            return cell;
        }
    }
}
