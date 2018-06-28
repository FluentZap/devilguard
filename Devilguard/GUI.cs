using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;

namespace Devilguard
{


    class GUIElementType
    {
        public Rectangle Location;
        public byte Depth;
    }    


    class GUI
    {
        public HashSet<GUIElementType> Elements = new HashSet<GUIElementType>();
        




    }
}
