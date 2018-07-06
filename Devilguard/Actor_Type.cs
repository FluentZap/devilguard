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
        public InventoryItem[] Equipment = new InventoryItem[6];      
        public SortedDictionary<int, InventoryItem> UsableItems = new SortedDictionary<int, InventoryItem>();
    }


    public struct Actor_Stats_Type
    {
        //Stats
        public int Level, Vigor, Will, Strength, Mind, Speed;
        public int HP, HPMax, MP, MPMax, Attack, SpecialAttack;
        
    }

    enum Listof_Allegiance
    {
        Controlled,
        Ally,
        Enemy,
        Neutral
    }



    class Actor_Type
    {        
        public int ID;
        public Point2 Location;
        public int Sprite;
        public Listof_Allegiance Allegiance;

        public Actor_Stats_Type Stats;
        public Actor_Loadout Loadout = new Actor_Loadout();



        public Rectangle Hitbox;
        public float Speed;
        public Inventory inventory = new Inventory();
        public CraftCatalog CraftingBlueprints = new CraftCatalog();
        public bool UsedItem;
        public int ItemCooldown;        
        public int SelectedItem;
        public Rectangle Reach = new Rectangle(-2, -2, 5, 5);

        public Queue<Point> MoveQueue = new Queue<Point>();
        


        public Point getTile()
        {
            return (new Point(Hitbox.Center.X, Hitbox.Bottom) + new Point((int)Location.X, (int)Location.Y)) / new Point(32, 32);
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