using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;

namespace Devilguard
{


    enum UIInventoryElement : int
    {
        BackInventory,
        BackLoadout,
        BackCrafting,
        BackBottomBar,
        ButtonCraft
    }

    enum UICombatElement : int
    {
        BackInitiative,
        BackActions,
        Action_Skill1,
        Action_Skill2,        
        Action_Move        
    }

    enum SD_UI : int
    {
        None = -1,
        Back,
        BackLoadout,
        Button,
        Resources,
        Items,
        BottomBar,
        SelectBox
    };



    class GUIElementType
    {
        public Rectangle Location;
        public byte Depth;
        public SD_UI Sprite;
        public Color color = Color.White;
        public bool Enabled = true;

    }    


    class GUI
    {
        public Dictionary<int, GUIElementType> InventoryElements = new Dictionary<int, GUIElementType>();
        public Dictionary<int, GUIElementType> CombatElements = new Dictionary<int, GUIElementType>();
        Point2 S;


        public GUI(Point2 s)
        {
            S = s;
            AddInventoryItems();
            AddCombatItems();
        }

        public void AddCombatItems()
        {
            Point p;
            int ID;
            CombatElements.Add((int)UICombatElement.BackInitiative, new GUIElementType
            {
                Location = new Rectangle(0, (int)S.Y - 200, (int)S.X, 200),
                Sprite = SD_UI.Back
            });
            
            CombatElements.Add((int)UICombatElement.BackActions, new GUIElementType
            {
                Location = new Rectangle(0, (int)S.Y / 2 - 200, 200, 400),
                Sprite = SD_UI.Back
            });

            CombatElements.Add((int)UICombatElement.Action_Skill1, new GUIElementType
            {
                Location = new Rectangle(0, (int)S.Y / 2 - 180, 200, 60),
                Sprite = SD_UI.Button
            });

            CombatElements.Add((int)UICombatElement.Action_Skill2, new GUIElementType
            {
                Location = new Rectangle(0, (int)S.Y / 2 - 100, 200, 60),
                Sprite = SD_UI.Button
            });

            CombatElements.Add((int)UICombatElement.Action_Move, new GUIElementType
            {
                Location = new Rectangle(0, (int)S.Y / 2 - 20, 200, 60),
                Sprite = SD_UI.Button
            });
        }



        public void AddInventoryItems()
        {
            Point p;
            int ID;
            InventoryElements.Add((int)UIInventoryElement.BackCrafting, new GUIElementType
            {
                Location = new Rectangle((int)S.X / 2 - 411, (int)S.Y / 2 - 212, 274, 424),
                Sprite = SD_UI.Back
            });

            InventoryElements.Add((int)UIInventoryElement.BackInventory, new GUIElementType
            {
                Location = new Rectangle((int)S.X / 2 - 137, (int)S.Y / 2 - 212, 274, 424),
                Sprite = SD_UI.Back
            });

            InventoryElements.Add((int)UIInventoryElement.BackLoadout, new GUIElementType
            {
                Location = new Rectangle((int)S.X / 2 + 137, (int)S.Y / 2 - 212, 274, 424),
                Sprite = SD_UI.BackLoadout
            });

            InventoryElements.Add((int)UIInventoryElement.BackBottomBar, new GUIElementType
            {
                Location = new Rectangle((int)S.X / 2 - 364, (int)S.Y - 88, 728, 88),
                Sprite = SD_UI.BottomBar
            });

            //AddResourceButtons
            ID = 1000;
            p.X = InventoryElements[(int)UIInventoryElement.BackInventory].Location.X;
            p.Y = InventoryElements[(int)UIInventoryElement.BackInventory].Location.Y;

            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 6; x++)
                {
                    InventoryElements.Add(ID, new GUIElementType
                    {
                        Location = new Rectangle(p.X + 27 + 38 * x, p.Y + 66 + 36 * y, 32, 32),
                        Sprite = SD_UI.None
                    });
                    ID++;
                }


            //AddCraftingButtons            
            ID = 2000;
            p.X = InventoryElements[(int)UIInventoryElement.BackCrafting].Location.X;
            p.Y = InventoryElements[(int)UIInventoryElement.BackCrafting].Location.Y;
            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 6; x++)
                {
                    InventoryElements.Add(ID, new GUIElementType
                    {
                        Location = new Rectangle(p.X + 27 + 38 * x, p.Y + 66 + 36 * y, 32, 32),
                        Sprite = SD_UI.Button
                    });
                    ID++;
                }

            //AddLoadoutButtons
            ID = 3000;
            p.X = InventoryElements[(int)UIInventoryElement.BackLoadout].Location.X;
            p.Y = InventoryElements[(int)UIInventoryElement.BackLoadout].Location.Y;
            //head
            InventoryElements.Add(ID, new GUIElementType
            {
                Location = new Rectangle(InventoryElements[(int)UIInventoryElement.BackLoadout].Location.Center.X - 16, p.Y + 75, 32, 32),
                Sprite = SD_UI.Button
            });
            ID++;
            //armor
            InventoryElements.Add(ID, new GUIElementType
            {
                Location = new Rectangle(InventoryElements[(int)UIInventoryElement.BackLoadout].Location.Center.X - 16, p.Y + 200, 32, 32),
                Sprite = SD_UI.Button
            });
            ID++;
            //left acc
            InventoryElements.Add(ID, new GUIElementType
            {
                Location = new Rectangle(p.X + (int)(InventoryElements[(int)UIInventoryElement.BackLoadout].Location.Width * 0.25) - 16, p.Y + 150, 32, 32),
                Sprite = SD_UI.Button
            });
            ID++;
            //right acc
            InventoryElements.Add(ID, new GUIElementType
            {
                Location = new Rectangle(p.X + (int)(InventoryElements[(int)UIInventoryElement.BackLoadout].Location.Width * 0.75) - 16, p.Y + 150, 32, 32),
                Sprite = SD_UI.Button
            });
            ID++;
            //left hand
            InventoryElements.Add(ID, new GUIElementType
            {
                Location = new Rectangle(p.X + (int)(InventoryElements[(int)UIInventoryElement.BackLoadout].Location.Width * 0.25) - 16, p.Y + 250, 32, 32),
                Sprite = SD_UI.Button
            });
            ID++;
            //right hand
            InventoryElements.Add(ID, new GUIElementType
            {
                Location = new Rectangle(p.X + (int)(InventoryElements[(int)UIInventoryElement.BackLoadout].Location.Width * 0.75) - 16, p.Y + 250, 32, 32),
                Sprite = SD_UI.Button
            });



            //AddBarButtons
            ID = 4000;
            p.X = InventoryElements[(int)UIInventoryElement.BackBottomBar].Location.X;
            p.Y = InventoryElements[(int)UIInventoryElement.BackBottomBar].Location.Y;
            for (int x = 0; x < 10; x++)
            {
                InventoryElements.Add(ID, new GUIElementType
                {
                    Location = new Rectangle(p.X + 6 + 72 * x, p.Y + 12, 64, 64),
                    Sprite = SD_UI.Button
                });
                ID++;
            }



        }


    }
}
