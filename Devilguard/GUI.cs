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
        BackCrafting,
        ButtonCraft
    }

    enum SD_UI : int
    {
        None = -1,
        Back,
        Button,
        Resources,
        Items
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
            InventoryElements.Add((int)UIElement.BackInventory, new GUIElementType
            {
                Location = new Rectangle((int)S.X / 2 - 274, (int)S.Y / 2 - 212, 274, 424),
                Sprite = SD_UI.Back
            });

            InventoryElements.Add((int)UIElement.BackCrafting, new GUIElementType
            {
                Location = new Rectangle((int)S.X / 2, (int)S.Y / 2 - 212, 274, 424),
                Sprite = SD_UI.Back
            });
            
            //AddResourceButtons
            ID = 1000;
            p.X = (int)S.X / 2 - 274;
            p.Y = (int)S.Y / 2 - 212;

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

            //AddInventoryButtons
            
            ID = 2000;
            p.X = (int)S.X / 2;
            p.Y = (int)S.Y / 2 - 212;
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
                
        }

    }
}
