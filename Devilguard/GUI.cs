using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;

namespace Devilguard
{


    enum UIElement : int
    {
        BackInventory,
        BackLoadout,        
        BackCrafting,
        BackBottomBar,
        ButtonCraft
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
        



        public GUI(Point2 S)
        {
            Point p;
            int ID;
            InventoryElements.Add((int)UIElement.BackCrafting, new GUIElementType
            {
                Location = new Rectangle((int)S.X / 2 - 411, (int)S.Y / 2 - 212, 274, 424),
                Sprite = SD_UI.Back
            });

            InventoryElements.Add((int)UIElement.BackInventory, new GUIElementType
            {
                Location = new Rectangle((int)S.X / 2 - 137, (int)S.Y / 2 - 212, 274, 424),
                Sprite = SD_UI.Back
            });
            
            InventoryElements.Add((int)UIElement.BackLoadout, new GUIElementType
            {
                Location = new Rectangle((int)S.X / 2 + 137, (int)S.Y / 2 - 212, 274, 424),
                Sprite = SD_UI.BackLoadout
            });

            InventoryElements.Add((int)UIElement.BackBottomBar, new GUIElementType
            {
                Location = new Rectangle((int)S.X / 2 - 364, (int)S.Y - 88, 728, 88),
                Sprite = SD_UI.BottomBar
            });

            //AddResourceButtons
            ID = 1000;
            p.X = InventoryElements[(int)UIElement.BackInventory].Location.X;
            p.Y = InventoryElements[(int)UIElement.BackInventory].Location.Y;

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
            p.X = InventoryElements[(int)UIElement.BackCrafting].Location.X;
            p.Y = InventoryElements[(int)UIElement.BackCrafting].Location.Y;
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
            p.X = InventoryElements[(int)UIElement.BackLoadout].Location.X;
            p.Y = InventoryElements[(int)UIElement.BackLoadout].Location.Y;
            //head
            InventoryElements.Add(ID, new GUIElementType
            {
                Location = new Rectangle(InventoryElements[(int)UIElement.BackLoadout].Location.Center.X - 16, p.Y + 75, 32, 32),
                Sprite = SD_UI.Button
            });
            ID++;
            //armor
            InventoryElements.Add(ID, new GUIElementType
            {
                Location = new Rectangle(InventoryElements[(int)UIElement.BackLoadout].Location.Center.X - 16, p.Y + 200, 32, 32),
                Sprite = SD_UI.Button
            });
            ID++;
            //left acc
            InventoryElements.Add(ID, new GUIElementType
            {
                Location = new Rectangle(p.X + (int)(InventoryElements[(int)UIElement.BackLoadout].Location.Width * 0.25) - 16, p.Y + 150, 32, 32),
                Sprite = SD_UI.Button
            });
            ID++;
            //right acc
            InventoryElements.Add(ID, new GUIElementType
            {
                Location = new Rectangle(p.X + (int)(InventoryElements[(int)UIElement.BackLoadout].Location.Width * 0.75) - 16, p.Y + 150, 32, 32),
                Sprite = SD_UI.Button
            });
            ID++;
            //left hand
            InventoryElements.Add(ID, new GUIElementType
            {
                Location = new Rectangle(p.X + (int)(InventoryElements[(int)UIElement.BackLoadout].Location.Width * 0.25) - 16, p.Y + 250, 32, 32),
                Sprite = SD_UI.Button
            });
            ID++;
            //right hand
            InventoryElements.Add(ID, new GUIElementType
            {
                Location = new Rectangle(p.X + (int)(InventoryElements[(int)UIElement.BackLoadout].Location.Width * 0.75) - 16, p.Y + 250, 32, 32),
                Sprite = SD_UI.Button
            });



            //AddBarButtons
            ID = 4000;
            p.X = InventoryElements[(int)UIElement.BackBottomBar].Location.X;
            p.Y = InventoryElements[(int)UIElement.BackBottomBar].Location.Y;            
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
