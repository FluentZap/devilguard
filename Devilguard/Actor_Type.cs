using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;

namespace Devilguard
{
    
    class Actor_Loadout
    {
        public InventoryEntry LeftHand;
        public InventoryEntry RightHand;
        public InventoryEntry Head;
        public InventoryEntry Armor;
        public InventoryEntry Accessory1;
        public InventoryEntry Accessory2;
        public SortedDictionary<int, InventoryEntry> UsableItems = new SortedDictionary<int, InventoryEntry>();
    }


    class Actor_Type
    {
        public Point Location;
        public Rectangle Hitbox;
        public float Speed;
        public Inventory inventory = new Inventory();
        public CraftCatalog CraftingBlueprints = new CraftCatalog();
        public bool UsedItem;
        public int ItemCooldown;        
        public Actor_Loadout Loadout = new Actor_Loadout();
        public int SelectedItem;
        public Rectangle Reach = new Rectangle(-2, -2, 5, 5);


        public Point getTile()
        {
            return (Hitbox.Center + Location) / new Point(32, 32);
        }


        public void UseItem()
        {
            if (!UsedItem)
            {
                UsedItem = true;
                ItemCooldown = 20;
            }            
        }

    }
}