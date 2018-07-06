using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devilguard
{
    enum Listof_ResourceItem
    {
        Wood,
        Stone,
        Iron,
        Dirt
    };

    //this is the master list of items
    enum Listof_InventoryItem : int
    {
        WoodChest,
        WoodPickAxe,
        WoodAxe,
        WoodShovel,
        WoodHoe,
        WoodSickle,
        WoodClub,
        WoodBow,
        StoneThrone,
        StoneHelmet,
        StoneArmor,
        StoneShield,
        StoneRing,

    };


    enum Listof_MineHardness : byte
    {
        Stone,
        Iron,
    };

    
    class Tile_Type
    {
        public Listof_Tile_Type ID;
        public int Occupied = -1;
        public Structure_Type Structure;

        public bool Walkable(Catalog c)
        {
            if (Occupied == -1 && c.tile.Data[ID].Walkalble)
                if (Structure == null || c.structure.Data[Structure.Type].Walkalble)
                    return true;
            return false;
        }


    }



    enum Listof_Tile_Type
    {
        Grass,
        Dirt
    }


    class Structure_Type
    {
        public Listof_Structures Type;
        public int Durability;        
    }

    enum Listof_Structures
    {
        None = -1,
        Tree1,
        Stone,
        WoodWall,
        StoneThrone
    }

    enum Listof_ItemTypes
    {
        None = -1,
        WeaponOneHanded,
        WeaponTwoHanded,
        Shield,
        Headpiece,
        Armor,
        Accessory,
        Tool,
        Wand
    }



    class Catalog
    {
        public StructureDictionary structure = new StructureDictionary();
        public CraftingDictionary crafting = new CraftingDictionary();
        public ResourceDictionary resource = new ResourceDictionary();
        public ItemDictionary item = new ItemDictionary();
        public TileDictionary tile = new TileDictionary();
    }        




    //Item Dictionary
    class StructureDictionaryEntry
    {
        public string Name;
        public int Value;
        public int Weight;
        public int Durability;        
        public int Sprite;
        public bool Walkalble;
        public Listof_MineHardness Hardness;

        public bool DropsItem;
        public bool DropsResource;

        public KeyValuePair<int, Listof_ResourceItem> ResourceDrop;
        public KeyValuePair<int, Listof_InventoryItem> ItemDrop;

        //public Listof_ResourceItem DropsResource;
        //public int DropsResourceAmount;
        //public Listof_InventoryItem DropsItem;
        //public int DropsItemAmount;
        
    }

    



    class TileDictionaryEntry
    {
        public int Sprite;
        public bool Walkalble;
        public int Durability;
        public int Drops;
        public Listof_MineHardness Hardness;
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
        public Listof_Structures BuildTile = Listof_Structures.None;
        public Listof_ItemTypes ItemType = Listof_ItemTypes.None;


        public bool HarvestStone;
        public bool HarvestWood;
        public bool HarvestDirt;

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

    


    //Crafting Dictionary
    class CraftingDictinaryEntry
    {
        public Dictionary<Listof_ResourceItem, int> ResourceCost = new Dictionary<Listof_ResourceItem, int>();
        public string Name;
        public int Value;
        public int Weight;

        public CraftingDictinaryEntry(String name, int value, int weight, Listof_ResourceItem r1, int c1)
        {
            Name = name;
            Value = value;
            Weight = weight;
            ResourceCost.Add(r1, c1);
        }

        public CraftingDictinaryEntry(String name, int value, int weight, Listof_ResourceItem r1, int c1, Listof_ResourceItem r2, int c2)
        {
            Name = name;
            Value = value;
            Weight = weight;
            ResourceCost.Add(r1, c1);
            ResourceCost.Add(r2, c2);
        }

        public CraftingDictinaryEntry(String name, int value, int weight, Listof_ResourceItem r1, int c1, Listof_ResourceItem r2, int c2, Listof_ResourceItem r3, int c3)
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
        public Dictionary<Listof_InventoryItem, CraftingDictinaryEntry> Data = new Dictionary<Listof_InventoryItem, CraftingDictinaryEntry>();
        public CraftingDictionary()
        {
            Data.Add(Listof_InventoryItem.StoneThrone, new CraftingDictinaryEntry("Stone Throne", 100, 50, Listof_ResourceItem.Stone, 50));

            Data.Add(Listof_InventoryItem.WoodChest, new CraftingDictinaryEntry("Wooden Chest", 10, 50, Listof_ResourceItem.Wood, 40));
            Data.Add(Listof_InventoryItem.WoodPickAxe, new CraftingDictinaryEntry("Wooden PickAxe", 10, 10, Listof_ResourceItem.Wood, 20));
            Data.Add(Listof_InventoryItem.WoodAxe, new CraftingDictinaryEntry("Wooden Axe", 10, 10, Listof_ResourceItem.Wood, 15, Listof_ResourceItem.Stone, 5));
            Data.Add(Listof_InventoryItem.WoodShovel, new CraftingDictinaryEntry("Wooden Shovel", 10, 10, Listof_ResourceItem.Wood, 15, Listof_ResourceItem.Stone, 5));
            Data.Add(Listof_InventoryItem.WoodHoe, new CraftingDictinaryEntry("Wooden Hoe", 10, 10, Listof_ResourceItem.Wood, 50, Listof_ResourceItem.Stone, 5));
            Data.Add(Listof_InventoryItem.WoodSickle, new CraftingDictinaryEntry("Wooden Sickel", 10, 10, Listof_ResourceItem.Wood, 50, Listof_ResourceItem.Stone, 5));
            Data.Add(Listof_InventoryItem.WoodClub, new CraftingDictinaryEntry("Wooden Club", 15, 15, Listof_ResourceItem.Wood, 40));
            Data.Add(Listof_InventoryItem.WoodBow, new CraftingDictinaryEntry("Wooden Bow", 20, 10, Listof_ResourceItem.Wood, 60));


            Data.Add(Listof_InventoryItem.StoneHelmet, new CraftingDictinaryEntry("Wooden Bow", 20, 10, Listof_ResourceItem.Stone, 20));
            Data.Add(Listof_InventoryItem.StoneArmor, new CraftingDictinaryEntry("Wooden Bow", 20, 10, Listof_ResourceItem.Stone, 20));
            Data.Add(Listof_InventoryItem.StoneShield, new CraftingDictinaryEntry("Wooden Bow", 20, 10, Listof_ResourceItem.Stone, 20));
            Data.Add(Listof_InventoryItem.StoneRing, new CraftingDictinaryEntry("Wooden Bow", 20, 10, Listof_ResourceItem.Stone, 20));
            

        }
    }

    class ItemDictionary
    {
        public Dictionary<Listof_InventoryItem, ItemDictionaryEntry> Data = new Dictionary<Listof_InventoryItem, ItemDictionaryEntry>();
        public ItemDictionary()
        {
            //Buildables
            Data.Add(Listof_InventoryItem.WoodChest, new ItemDictionaryEntry() { Name = "Wooden Chest", Value = 10, Weight = 10, DurabilityMax = 100, Durability = 100 });
            Data.Add(Listof_InventoryItem.StoneThrone, new ItemDictionaryEntry() { Name = "Stone Throne", Value = 100, Weight = 50, DurabilityMax = 100, Durability = 100, BuildTile = Listof_Structures.StoneThrone });

            //Tools
            Data.Add(Listof_InventoryItem.WoodPickAxe, new ItemDictionaryEntry() { Name = "Wooden PickAxe", Value = 10, Weight = 10, DurabilityMax = 100, Durability = 100, Cooldown = 3, PowerTier = 1, Power = 3, HarvestStone = true });
            Data.Add(Listof_InventoryItem.WoodAxe, new ItemDictionaryEntry() { Name = "Wooden Axe", Value = 10, Weight = 10, DurabilityMax = 100, Durability = 100, Cooldown = 3, PowerTier = 1, Power = 3, HarvestWood = true });
            Data.Add(Listof_InventoryItem.WoodShovel, new ItemDictionaryEntry() { Name = "Wooden Shovel", Value = 10, Weight = 10, DurabilityMax = 100, Durability = 100, Cooldown = 3, PowerTier = 1, Power = 3, HarvestDirt = true });
            Data.Add(Listof_InventoryItem.WoodHoe, new ItemDictionaryEntry() { Name = "Wooden Hoe", Value = 10, Weight = 10, DurabilityMax = 100, Durability = 100, Cooldown = 3, PowerTier = 1, Power = 3 });
            Data.Add(Listof_InventoryItem.WoodSickle, new ItemDictionaryEntry() { Name = "Wooden Sickle", Value = 10, Weight = 10, DurabilityMax = 100, Durability = 100, Cooldown = 3, PowerTier = 1, Power = 3 });

            //Weapons
            Data.Add(Listof_InventoryItem.WoodClub, new ItemDictionaryEntry() { Name = "Wooden Club", Value = 10, Weight = 10, DurabilityMax = 100, Durability = 100, PowerTier = 1, Power = 1, ItemType = Listof_ItemTypes.WeaponOneHanded });
            Data.Add(Listof_InventoryItem.WoodBow, new ItemDictionaryEntry() { Name = "Wooden Bow", Value = 10, Weight = 10, DurabilityMax = 100, Durability = 100, PowerTier = 1, Power = 1, ItemType = Listof_ItemTypes.WeaponTwoHanded });


            //Armor
            Data.Add(Listof_InventoryItem.StoneHelmet, new ItemDictionaryEntry() { Name = "Wooden Club", Value = 10, Weight = 10, DurabilityMax = 100, Durability = 100, Power = 1, ItemType = Listof_ItemTypes.Headpiece });
            Data.Add(Listof_InventoryItem.StoneArmor, new ItemDictionaryEntry() { Name = "Wooden Bow", Value = 10, Weight = 10, DurabilityMax = 100, Durability = 100, Power = 1, ItemType = Listof_ItemTypes.Armor });
            Data.Add(Listof_InventoryItem.StoneShield, new ItemDictionaryEntry() { Name = "Wooden Club", Value = 10, Weight = 10, DurabilityMax = 100, Durability = 100, Power = 1, ItemType = Listof_ItemTypes.Shield });
            Data.Add(Listof_InventoryItem.StoneRing, new ItemDictionaryEntry() { Name = "Wooden Bow", Value = 10, Weight = 10, DurabilityMax = 100, Durability = 100, Power = 1, ItemType = Listof_ItemTypes.Accessory });
        }
    }

    class ResourceDictionary
    {
        public Dictionary<Listof_ResourceItem, ResourceDictionaryEntry> Data = new Dictionary<Listof_ResourceItem, ResourceDictionaryEntry>();
        public ResourceDictionary()
        {
            Data.Add(Listof_ResourceItem.Wood, new ResourceDictionaryEntry("Wood", 3, 2));
            Data.Add(Listof_ResourceItem.Stone, new ResourceDictionaryEntry("Stone", 5, 5));
            Data.Add(Listof_ResourceItem.Iron, new ResourceDictionaryEntry("Iron", 20, 10));
            Data.Add(Listof_ResourceItem.Dirt, new ResourceDictionaryEntry("Dirt", 0, 5));
        }
    }


    class StructureDictionary
    {
        public Dictionary<Listof_Structures, StructureDictionaryEntry> Data = new Dictionary<Listof_Structures, StructureDictionaryEntry>();
        public StructureDictionary()
        {
            //Buildables
            Data.Add(Listof_Structures.Tree1, new StructureDictionaryEntry() { Name = "Tree", Value = 0, Weight = 0, Durability = 12, Walkalble = false, DropsResource = true, ResourceDrop = new KeyValuePair<int, Listof_ResourceItem>(5, Listof_ResourceItem.Wood) });
            Data.Add(Listof_Structures.Stone, new StructureDictionaryEntry() { Name = "Stone", Value = 0, Weight = 0, Durability = 20, Walkalble = false, DropsResource = true, ResourceDrop = new KeyValuePair<int, Listof_ResourceItem>(2, Listof_ResourceItem.Stone) });

            Data.Add(Listof_Structures.StoneThrone, new StructureDictionaryEntry() { Name = "Stone Throne", Value = 100, Weight = 50, Durability = 50 });
            //Data.Add(Listof_Structures.StoneThrone, new StructureDictionaryEntry() { Name = "Stone Throne", Value = 50, Weight = 50, DurabilityMax = 100, Durability = 100 });

        }
    }

    class TileDictionary
    {
        public Dictionary<Listof_Tile_Type, TileDictionaryEntry> Data = new Dictionary<Listof_Tile_Type, TileDictionaryEntry>();
        public TileDictionary()
        {
            Data.Add(Listof_Tile_Type.Grass, new TileDictionaryEntry() { Sprite = 0, Walkalble = true, Hardness = Listof_MineHardness.Stone });
            Data.Add(Listof_Tile_Type.Dirt, new TileDictionaryEntry() { Sprite = 2, Walkalble = true, Hardness = Listof_MineHardness.Stone });
        }
    }
}
