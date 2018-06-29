using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devilguard
{
        enum ResourceItem
        {
            Wood,
            Stone,
            Iron,
            Dirt
        };

        enum InventoryItem : int
        {
            WoodChest,
            WoodPickAxe,
            WoodAxe,
            WoodShovel,
            WoodHoe,
            WoodSickle,
            WoodClub,
            WoodBow,
            StoneThrone
        };


        enum MineHardness : byte
        {
            Stone,
            Iron,
        };

    class Structure_Type
    {
        public Structures Type;
        public int StructureDurabilityMax = 1;
        public int StructureDurability;
        public int Hardness;
    }    

    enum Structures
    {
        None = -1,
        Tree1,
        Stone,
        WoodWall,
        StoneThrone
    }




    //Item Dictionary
    class StructureDictionaryEntry
    {
        public string Name;
        public int Value;
        public int Weight;
        public int DurabilityMax;
        public int Durability;
        public int Sprite;
        public bool Walkalble;        
        public int Drops;
        public MineHardness Hardness;
    }

    class StructureDictionary
    {
        public Dictionary<Structures, StructureDictionaryEntry> Data = new Dictionary<Structures, StructureDictionaryEntry>();
        public StructureDictionary()
        {
            //Buildables
            Data.Add(Structures.StoneThrone, new StructureDictionaryEntry() { Name = "Stone Throne", Value = 50, Weight = 50, DurabilityMax = 100, Durability = 100 });
            
        }
    }



    class TileDictionary
    {
        public int Sprite;
        public bool Walkalble;
        public int Durability;
        public int Drops;
        public MineHardness Hardness;
    }

    //Item Dictionary
    class ItemDictionaryEntry
    {
        public string Name;
        public int Value;
        public int Weight;
        public byte PowerTier;
        public int Power;
        public int Cooldown;
        public int DurabilityMax;
        public int Durability;
        public Structures BuildTile = Structures.None;


        public bool HarvestStone;
        public bool HarvestWood;
        public bool HarvestDirt;

    }

    class ItemDictionary
    {
        public Dictionary<InventoryItem, ItemDictionaryEntry> Data = new Dictionary<InventoryItem, ItemDictionaryEntry>();
        public ItemDictionary()
        {
            //Buildables
            Data.Add(InventoryItem.WoodChest, new ItemDictionaryEntry() { Name = "Wooden Chest", Value = 10, Weight = 10, DurabilityMax = 100, Durability = 100 });
            Data.Add(InventoryItem.StoneThrone, new ItemDictionaryEntry() { Name = "Stone Throne", Value = 100, Weight = 50, DurabilityMax = 100, Durability = 100, BuildTile = Structures.StoneThrone });

            //Equipment
            Data.Add(InventoryItem.WoodPickAxe, new ItemDictionaryEntry() { Name = "Wooden PickAxe", Value = 10, Weight = 10, DurabilityMax = 100, Durability = 100, Cooldown = 3, PowerTier = 1, Power = 3, HarvestStone = true });
            Data.Add(InventoryItem.WoodAxe, new ItemDictionaryEntry() { Name = "Wooden Axe", Value = 10, Weight = 10, DurabilityMax = 100, Durability = 100, Cooldown = 3, PowerTier = 1, Power = 3, HarvestWood = true });
            Data.Add(InventoryItem.WoodShovel, new ItemDictionaryEntry() { Name = "Wooden Shovel", Value = 10, Weight = 10, DurabilityMax = 100, Durability = 100, Cooldown = 3, PowerTier = 1, Power = 3, HarvestDirt = true });
            Data.Add(InventoryItem.WoodHoe, new ItemDictionaryEntry() { Name = "Wooden Hoe", Value = 10, Weight = 10, DurabilityMax = 100, Durability = 100, Cooldown = 3, PowerTier = 1, Power = 3 });
            Data.Add(InventoryItem.WoodSickle, new ItemDictionaryEntry() { Name = "Wooden Sickle", Value = 10, Weight = 10, DurabilityMax = 100, Durability = 100, Cooldown = 3, PowerTier = 1, Power = 3 });

            //Weapons
            Data.Add(InventoryItem.WoodClub, new ItemDictionaryEntry() { Name = "Wooden Club", Value = 10, Weight = 10, DurabilityMax = 100, Durability = 100, PowerTier = 1, Power = 1 });
            Data.Add(InventoryItem.WoodBow, new ItemDictionaryEntry() { Name = "Wooden Bow", Value = 10, Weight = 10, DurabilityMax = 100, Durability = 100, PowerTier = 1, Power = 1 });
        }
    }

    //Resource Dictionary
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


    //Crafting Dictionary
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
        public Dictionary<InventoryItem, CraftingDictinaryEntry> Data = new Dictionary<InventoryItem, CraftingDictinaryEntry>();
        public CraftingDictionary()
        {
            Data.Add(InventoryItem.StoneThrone, new CraftingDictinaryEntry("Stone Throne", 100, 50, ResourceItem.Stone, 50));

            Data.Add(InventoryItem.WoodChest, new CraftingDictinaryEntry("Wooden Chest", 10, 50, ResourceItem.Wood, 40));
            Data.Add(InventoryItem.WoodPickAxe, new CraftingDictinaryEntry("Wooden PickAxe", 10, 10, ResourceItem.Wood, 20));
            Data.Add(InventoryItem.WoodAxe, new CraftingDictinaryEntry("Wooden Axe", 10, 10, ResourceItem.Wood, 15, ResourceItem.Stone, 5));
            Data.Add(InventoryItem.WoodShovel, new CraftingDictinaryEntry("Wooden Shovel", 10, 10, ResourceItem.Wood, 15, ResourceItem.Stone, 5));
            Data.Add(InventoryItem.WoodHoe, new CraftingDictinaryEntry("Wooden Hoe", 10, 10, ResourceItem.Wood, 50, ResourceItem.Stone, 5));
            Data.Add(InventoryItem.WoodSickle, new CraftingDictinaryEntry("Wooden Sickel", 10, 10, ResourceItem.Wood, 50, ResourceItem.Stone, 5));
            Data.Add(InventoryItem.WoodClub, new CraftingDictinaryEntry("Wooden Club", 15, 15, ResourceItem.Wood, 40));
            Data.Add(InventoryItem.WoodBow, new CraftingDictinaryEntry("Wooden Bow", 20, 10, ResourceItem.Wood, 60));
        }
    }
}
