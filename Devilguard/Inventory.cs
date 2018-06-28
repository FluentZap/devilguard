using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devilguard
{

    enum EquipmentType
    {
        WeaponLH,
        WeaponRH,
        WeaponDW,
        Shield,
        Headpiece,
        Armor,
        Accessory1,
        Accessory2,
        Tool,
        Wand
    }


    class InventoryEntry
    {
        public InventoryItem item;
        public int Amount;

        public InventoryEntry(InventoryItem i)
        {
            item = i;
        }
    }    

    class Inventory
    {
        public SortedDictionary<ResourceItem, int> Resources = new SortedDictionary<ResourceItem, int>();
        public SortedDictionary<int, InventoryEntry> Items = new SortedDictionary<int, InventoryEntry>();
        public int InventorySize = 24;

        //Resources
        public void AddResource(ResourceItem i, int amount)
        {
            if (Resources.ContainsKey(i))
                Resources[i] += amount;
            else
                Resources.Add(i, amount);
        }

        public void RemoveResource(ResourceItem i, int amount)
        {
            if (Resources.ContainsKey(i))
            {
                Resources[i] -= amount;
                if (Resources[i] <= 0)
                    Resources.Remove(i);
            }            
        }
        public int ResourceCount(ResourceItem i)
        {
            if (Resources.ContainsKey(i))
                return Resources[i];
            else
                return 0;            
        }




        //Items
        public bool AddItem(InventoryEntry i)
        {
            for (int x = 0; x < InventorySize; x++)
            {
                if (!Items.ContainsKey(x))
                {
                    Items.Add(x, i);
                    return true;                    
                }
            }
            return false;
        }

        public void RemoveItem(int i)
        {
            if (Items.ContainsKey(i))
                Items.Remove(i);
        }

        public InventoryEntry GetItem(int i)
        {
            if (Items.ContainsKey(i))
                return Items[i];
            return null;
        }

    }







    class CraftCatalog
    {
        public SortedSet<InventoryItem> Blueprints = new SortedSet<InventoryItem>();

        public void AddBlueprint(InventoryItem i)
        {
            if (!Blueprints.Contains(i))
                Blueprints.Add(i);
        }

        public void RemoveBlueprint(InventoryItem i)
        {
            if (Blueprints.Contains(i))
                Blueprints.Remove(i);
        }

        public bool InCatalog(InventoryItem i)
        {
            if (Blueprints.Contains(i))
                return true;
            else
                return false;
        }
    }

}
