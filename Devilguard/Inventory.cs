using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devilguard
{

    enum Listof_EquipmentType
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


    class InventoryItem
    {
        public Listof_InventoryItem item;
        public int Amount;

        public InventoryItem(Listof_InventoryItem i)
        {
            item = i;
        }
    }    

    class Inventory
    {
        public SortedDictionary<Listof_ResourceItem, int> Resources = new SortedDictionary<Listof_ResourceItem, int>();
        public SortedDictionary<int, InventoryItem> Items = new SortedDictionary<int, InventoryItem>();
        public int InventorySize = 24;

        //Resources
        public void AddResource(Listof_ResourceItem i, int amount)
        {
            if (Resources.ContainsKey(i))
                Resources[i] += amount;
            else
                Resources.Add(i, amount);
        }

        public void RemoveResource(Listof_ResourceItem i, int amount)
        {
            if (Resources.ContainsKey(i))
            {
                Resources[i] -= amount;
                if (Resources[i] <= 0)
                    Resources.Remove(i);
            }            
        }
        public int ResourceCount(Listof_ResourceItem i)
        {
            if (Resources.ContainsKey(i))
                return Resources[i];
            else
                return 0;            
        }




        //Items
        public bool AddItem(InventoryItem i)
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

        public InventoryItem GetItem(int i)
        {
            if (Items.ContainsKey(i))
                return Items[i];
            return null;
        }

    }







    class CraftCatalog
    {
        public SortedSet<Listof_InventoryItem> Blueprints = new SortedSet<Listof_InventoryItem>();

        public void AddBlueprint(Listof_InventoryItem i)
        {
            if (!Blueprints.Contains(i))
                Blueprints.Add(i);
        }

        public void RemoveBlueprint(Listof_InventoryItem i)
        {
            if (Blueprints.Contains(i))
                Blueprints.Remove(i);
        }

        public bool InCatalog(Listof_InventoryItem i)
        {
            if (Blueprints.Contains(i))
                return true;
            else
                return false;
        }
    }

}
