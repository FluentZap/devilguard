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


    public class Actor_Class_Type
    {
        public Listof_Classes Class = Listof_Classes.Empty;        

    }


        public struct Actor_Stats_Type
    {
        //Stats
        public int Level, Vigor, Will, Strength, Mind, Speed;
        public int HP, HPMax, MP, MPMax, Attack, SpecialAttack;

        public int Movement, MovementMax;
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
        public Actor_Class_Type PrimaryClass = new Actor_Class_Type();
        public Actor_Class_Type SecondaryClass = new Actor_Class_Type();
        public int AttackRange;


        public Rectangle Hitbox;        
        public Inventory inventory = new Inventory();
        public CraftCatalog CraftingBlueprints = new CraftCatalog();
        public bool UsedItem;
        public int ItemCooldown;        
        public int SelectedItem;
        public Rectangle Reach = new Rectangle(-2, -2, 5, 5);

        //public Queue<Point> MoveQueue = new Queue<Point>();
        public List<Point> MoveQueue = new List<Point>();
        public bool UsedAction, UsedMove;


        public void NewTurnRefresh()
        {
            Stats.Movement = Stats.MovementMax;
            UsedAction = false;
            UsedMove = false;

        }

        
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