using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;

namespace Devilguard
{
    class Actor_Type
    {
        public Point Location;
        public Rectangle Hitbox;
        public float Speed;
        public Inventory inventory = new Inventory();
        public CraftCatalog CraftingBlueprints = new CraftCatalog();
        public bool UsedItem;

    }
}