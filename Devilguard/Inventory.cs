using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devilguard
{

    enum SD_UI:int
    {
        Back,
        Button,
        Resources
    };


    enum Item
    {
        Wood,
        Stone,
        Iron,
        Dirt
    };



   
    class Inventory
    {
        public Dictionary<Item, int> Items = new Dictionary<Item, int>();


        public void AddItem(Item i, int amount)
        {
            if (Items.ContainsKey(i))
                Items[i] += amount;
            else
                Items.Add(i, amount);
        }

        public void RemoveItem(Item i, int amount)
        {
            if (Items.ContainsKey(i))
            {
                Items[i] -= amount;
                if (Items[i] <= 0)
                    Items.Remove(i);
            }            
        }

        public int ItemCount(Item i)
        {
            if (Items.ContainsKey(i))
                return Items[i];
            else
                return 0;

            
        }

        


    }
}
