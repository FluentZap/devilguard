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
        Resources,
        Items
    };


    enum ResourceItem
    {
        Wood,
        Stone,
        Iron,
        Dirt
    };

    enum InventoryItems:int
    {
        WoodChest,
        WoodPickAxe,
        WoodAxe,
        WoodShovel,
        WoodHoe,
        WoodSickle,
        WoodClub,
        WoodBow
    };


    enum MineHardness : byte
    {
        Stone,
        Iron,
    };


    class TileDictionary
    {        
        public int Sprite;
        public bool Walkalble;        
        public int Durability;        
        public int Drops;
        public MineHardness Hardness;
    }


    class ResourceDictionaryEntry
    {
        public string Name;
        public int Value;
        public int Weight;

        public ResourceDictionaryEntry(String name, int value, int weight)
        {
            Name = name;
            Value = value;
            Weight = weight;
        }

    }

    class ResourceDictionary
    {
        public Dictionary<ResourceItem, ResourceDictionaryEntry> Data = new Dictionary<ResourceItem, ResourceDictionaryEntry>();
        public ResourceDictionary()
        {
            Data.Add(ResourceItem.Wood, new ResourceDictionaryEntry("Wood", 3, 2));
            Data.Add(ResourceItem.Stone, new ResourceDictionaryEntry("Stone", 5, 5));
            Data.Add(ResourceItem.Iron, new ResourceDictionaryEntry("Iron", 20, 10));
            Data.Add(ResourceItem.Dirt, new ResourceDictionaryEntry("Dirt", 0, 5));
        }
    }



    class CraftingDictinaryEntry
    {
        public Dictionary<ResourceItem, int> ResourceCost = new Dictionary<ResourceItem, int>();
        public string Name;
        public int Value;
        public int Weight;
        
        public CraftingDictinaryEntry(String name, int value, int weight, ResourceItem r1, int c1)
        {            
            Name = name;
            Value = value;
            Weight = weight;
            ResourceCost.Add(r1, c1);
        }

        public CraftingDictinaryEntry(String name, int value, int weight, ResourceItem r1, int c1, ResourceItem r2, int c2)
        {
            Name = name;
            Value = value;
            Weight = weight;
            ResourceCost.Add(r1, c1);
            ResourceCost.Add(r2, c2);
        }

        public CraftingDictinaryEntry(String name, int value, int weight, ResourceItem r1, int c1, ResourceItem r2, int c2, ResourceItem r3, int c3)
        {
            Name = name;
            Value = value;
            Weight = weight;
            ResourceCost.Add(r1, c1);
            ResourceCost.Add(r2, c2);
            ResourceCost.Add(r3, c3);
        }



    }
    
    class CraftingDictionary
    {
        public Dictionary<InventoryItems, CraftingDictinaryEntry> Data = new Dictionary<InventoryItems, CraftingDictinaryEntry>();
        public CraftingDictionary()
        {
            Data.Add(InventoryItems.WoodChest, new CraftingDictinaryEntry("Wooden Chest", 10, 50, ResourceItem.Wood, 40));
            Data.Add(InventoryItems.WoodPickAxe, new CraftingDictinaryEntry("Wooden Axe", 10, 10, ResourceItem.Wood, 10, ResourceItem.Stone, 5));
            Data.Add(InventoryItems.WoodAxe, new CraftingDictinaryEntry("Wooden Axe", 10, 10, ResourceItem.Wood, 10, ResourceItem.Stone, 5));
            Data.Add(InventoryItems.WoodShovel, new CraftingDictinaryEntry("Wooden Shovel", 10, 10, ResourceItem.Wood, 10, ResourceItem.Stone, 5));
            Data.Add(InventoryItems.WoodHoe, new CraftingDictinaryEntry("Wooden Hoe", 10, 10, ResourceItem.Wood, 40, ResourceItem.Stone, 5));
            Data.Add(InventoryItems.WoodSickle, new CraftingDictinaryEntry("Wooden Sickel", 10, 10, ResourceItem.Wood, 40, ResourceItem.Stone, 5));
            Data.Add(InventoryItems.WoodClub, new CraftingDictinaryEntry("Wooden Club", 15, 15, ResourceItem.Wood, 40));
            Data.Add(InventoryItems.WoodBow, new CraftingDictinaryEntry("Wooden Bow", 20, 10, ResourceItem.Wood, 60));            
        }
    }




    class Inventory
    {
        public Dictionary<ResourceItem, int> Resources = new Dictionary<ResourceItem, int>();
        public Dictionary<InventoryItems, int> Items = new Dictionary<InventoryItems, int>();


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
        public void AddItem(InventoryItems i, int amount)
        {
            if (Items.ContainsKey(i))
                Items[i] += amount;
            else
                Items.Add(i, amount);
        }

        public void RemoveItem(InventoryItems i, int amount)
        {
            if (Items.ContainsKey(i))
            {
                Items[i] -= amount;
                if (Items[i] <= 0)
                    Items.Remove(i);
            }
        }
        public int ItemCount(InventoryItems i)
        {
            if (Items.ContainsKey(i))
                return Items[i];
            else
                return 0;
        }

    }







    class CraftCatalog
    {
        public HashSet<InventoryItems> Blueprints = new HashSet<InventoryItems>();


        public void AddBlueprint(InventoryItems i)
        {
            if (!Blueprints.Contains(i))                
                Blueprints.Add(i);
        }

        public void RemoveBlueprint(InventoryItems i)
        {
            if (Blueprints.Contains(i))
                Blueprints.Remove(i);            
        }

        public bool InCatalog(InventoryItems i)
        {
            if (Blueprints.Contains(i))
                return true;
            else
                return false;
        }
    }

}
