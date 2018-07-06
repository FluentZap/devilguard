using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic;

namespace Devilguard
{    


    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 

    public class Game1 : Game
    {
        static float DIAGONAL_SPEED_MODIFIER = 0.70710678118654757F;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont basicfont;


        Texture2D[] Background_Tiles = new Texture2D[10];
        Texture2D[] Actor_Sprites = new Texture2D[10];
        Texture2D[] Item_Tiles = new Texture2D[10];
        Texture2D[] UI_Textures = new Texture2D[10];




        Tile_Type[,] tilemap = new Tile_Type[1000, 1000];
        Point Screen_Scroll;
        float Screen_Zoom;
        public Point Screen_Size;
        Point MouseDragStart;
        bool MouseDraging;

        Point MouseDragScreenScrollStart;

        bool LeftMouseClicked;
        bool RightMouseClicked;

        bool InventoryOpen;
        bool InventoryKey;

        InventoryItem MouseHeldItem;
        Point SelectedTile;
        Point LastSelectedTile;

        bool TileInReach;
        Combat currentCombat = new Combat();

        PathFind pathfind;
        bool RecalculatePF = true;


        int TileCircleSize;
        HashSet<Point> TileCircle= new HashSet<Point>();

        //LinkedList<Point> path;
        List<Point> path;
        Dictionary<int, Actor_Type> ActorList = new Dictionary<int, Actor_Type>();


        public enum Listof_KeyStatus
        {            
            Down,
            Handled
        }



        public Dictionary<Keys, Listof_KeyStatus> PressedKeys = new Dictionary<Keys, Listof_KeyStatus>();


        public enum Listof_GameMode
        {
            Personal,
            Combat,
            Strategy
        }

        public Listof_GameMode GameMode;



        static Catalog catalog = new Catalog();

        static Dictionary<Listof_Structures, StructureDictionaryEntry> C_Structure = catalog.structure.Data;
        static Dictionary<Listof_InventoryItem, CraftingDictinaryEntry> C_Crafting = catalog.crafting.Data;
        static Dictionary<Listof_ResourceItem, ResourceDictionaryEntry> C_Resource = catalog.resource.Data;
        static Dictionary<Listof_InventoryItem, ItemDictionaryEntry> C_Item = catalog.item.Data;
        static Dictionary<Listof_Tile_Type, TileDictionaryEntry> C_Tile = catalog.tile.Data;

        GUI gui;

        Actor_Type Player = new Actor_Type();
        enum Direction { LEFT, RIGHT, UP, DOWN };
        


        void Move_Actor(Actor_Type A, Point2 Vector)
        {
            float X, Y;
            //Vector.X *= A.Speed;
            //Vector.Y *= A.Speed;

            while (Vector.X == 0 && Vector.Y == 0)
            {
                if (Vector.X > 0)
                    if (Vector.X > 1) { Vector.X--; X = 1; } else { X = Vector.X; Vector.X = 0; }
                else
                    if (Vector.X < -1) { Vector.X++; X = -1; } else { X = Vector.X; Vector.X = 0; }

                if (Vector.Y > 0)
                    if (Vector.Y > 1) { Vector.Y--; Y = 1; } else { Y = Vector.Y; Vector.Y = 0; }
                else
                    if (Vector.Y < -1) { Vector.Y++; Y = -1; } else { Y = Vector.Y; Vector.Y = 0; }

                if (X > 0) Move_Actor_Step(A, Direction.RIGHT, Math.Abs(X));
                if (X < 0) Move_Actor_Step(A, Direction.LEFT, Math.Abs(X));
                if (Y < 0) Move_Actor_Step(A, Direction.UP, Math.Abs(Y));
                if (Y > 0) Move_Actor_Step(A, Direction.DOWN, Math.Abs(Y));


            }
            
            
            //A.Hitbox.Bottom

            //Point pos.x = (Mouse.GetState().Position + Screen_Scroll) / new Point(64, 64);


        }

        void Move_Actor_Step(Actor_Type A, Direction D, float amount)
        {
            RectangleF Box = A.Hitbox;
            //Box.Offset(A.Location.ToVector2());
        }

        float GetATSF()
        {
            return 32 * Screen_Zoom;
        }

        int GetATSI()
        {
            return (int)(32 * Screen_Zoom);
        }



        public Game1()
        {
            Screen_Size.X = 1920;
            Screen_Size.Y = 1080;
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = Screen_Size.Y,
                PreferredBackBufferWidth = Screen_Size.X
            };
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Random R = new Random();
            for (int x = 0; x < 1000; x++)
                for (int y = 0; y < 1000; y++)
                {
                    tilemap[x, y] = new Tile_Type()
                    {                                                
                        ID = Listof_Tile_Type.Grass
                    };                    
                    if (R.Next(0, 10) == 0)
                        tilemap[x, y].Structure = new Structure_Type() { Type = Listof_Structures.Tree1, Durability = C_Structure[Listof_Structures.Tree1].Durability};
                }
                    

            for (int x = 10; x < 60; x++)
                for (int y = 10; y < 60; y++)
                {
                    tilemap[x, y].ID = Listof_Tile_Type.Dirt;
                    tilemap[x, y].Structure = new Structure_Type() { Type = Listof_Structures.Stone, Durability = C_Structure[Listof_Structures.Stone].Durability };
                }
                    

            Screen_Zoom = 2.0f;

            pathfind = new PathFind(tilemap, new Rectangle(0, 0, 999, 999), catalog);


            Player.Hitbox = new Rectangle(7, 2, 18, 29);
            Player.inventory.AddResource(Listof_ResourceItem.Wood, 500);
            Player.inventory.AddResource(Listof_ResourceItem.Stone, 500);            
            Player.Stats.MovementMax = 8;
            Player.Stats.Movement = 8;
            Player.PrimaryClass.Class = Listof_Classes.Medicus;
            //Player.Loadout.UsableItems[0] = new InventoryEntry(InventoryItem.WoodClub);
            //Player.Loadout.UsableItems[1] = new InventoryEntry(InventoryItem.WoodBow);
            //Player.Loadout.UsableItems[3] = new InventoryEntry(InventoryItem.WoodAxe);
            //Player.inventory.AddItem(Item.Iron, 30);

            //Player.inventory.AddItem(new InventoryEntry(InventoryItem.WoodHoe));


            ActorList.Add(0, Player);
            Player.Stats.Speed = 15;
            Player.Stats.HP = 52; Player.Stats.HPMax = 52;
            ActorList.Add(1, new Actor_Type());
            ActorList.Add(2, new Actor_Type());
            ActorList.Add(3, new Actor_Type());

            ActorList[1].Stats.Speed = 10; ActorList[1].Sprite = 1;
            ActorList[2].Stats.Speed = 8;  ActorList[2].Sprite = 2;
            ActorList[3].Stats.Speed = 12; ActorList[3].Sprite = 3;

            ActorList[1].Stats.HP = 40; ActorList[1].Stats.HPMax = 40;
            ActorList[2].Stats.HP = 40; ActorList[2].Stats.HPMax = 40;
            ActorList[3].Stats.HP = 40; ActorList[3].Stats.HPMax = 40;

            ActorList[1].PrimaryClass.Class = Listof_Classes.Trooper;
            ActorList[2].PrimaryClass.Class = Listof_Classes.Medicus;
            ActorList[3].PrimaryClass.Class = Listof_Classes.Lightfoot;

            ActorList[1].Location = new Point(10 * 32, 4 * 32);
            ActorList[2].Location = new Point(2 * 32, 2 * 32);
            ActorList[3].Location = new Point(7 * 32, 7 * 32);

            currentCombat.InitiativeList.Add(new Combat_Slot(ActorList[0], 0));
            currentCombat.InitiativeList.Add(new Combat_Slot(ActorList[1], 0));
            currentCombat.InitiativeList.Add(new Combat_Slot(ActorList[2], 0));
            currentCombat.InitiativeList.Add(new Combat_Slot(ActorList[3], 0));

            currentCombat.AdvanceToTurn();
            Combat_Block_Tiles();

            for (int x = 0; x < Enum.GetNames(typeof(Listof_InventoryItem)).Length; x++)
                Player.CraftingBlueprints.AddBlueprint((Listof_InventoryItem)x);


            gui = new GUI(Screen_Size);
            base.Initialize();
            Window.IsBorderless = true;
            this.IsMouseVisible = true;
            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, Screen_Size.X, Screen_Size.Y);

            

        }

        public enum SD_UI : int
        {
            None = -1,
            Back,
            BackLoadout,
            Button,
            Resources,
            Items,
            BottomBar,
            SelectBox,
            //Combat
            Combat_ActionBack,
            Combat_ActionButton

        };

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.            
            Background_Tiles[0] = Content.Load<Texture2D>("BackTileMap");
            Actor_Sprites[0] = Content.Load<Texture2D>("ActorSprites");
            Item_Tiles[0] = Content.Load<Texture2D>("Tree");

            UI_Textures[(int)SD_UI.Back] = Content.Load<Texture2D>("UI_Back");
            UI_Textures[(int)SD_UI.BackLoadout] = Content.Load<Texture2D>("UI_Loadout");
            UI_Textures[(int)SD_UI.Button] = Content.Load<Texture2D>("UI_Button");
            UI_Textures[(int)SD_UI.Resources] = Content.Load<Texture2D>("UI_Resources");
            UI_Textures[(int)SD_UI.Items] = Content.Load<Texture2D>("UI_Items");
            UI_Textures[(int)SD_UI.BottomBar] = Content.Load<Texture2D>("UI_Bar");
            UI_Textures[(int)SD_UI.SelectBox] = Content.Load<Texture2D>("SelectBox");
            UI_Textures[(int)SD_UI.Combat_ActionBack] = Content.Load<Texture2D>("UI_CombatActionBack");

            UI_Textures[(int)SD_UI.Combat_ActionButton] = Content.Load<Texture2D>("UI_CombatActionButton");

            basicfont = Content.Load<SpriteFont>("BasicFont");

            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            SelectedTile = (Mouse.GetState().Position + Screen_Scroll) / new Point(GetATSI(), GetATSI());
            if (Mouse.GetState().Position.X + Screen_Scroll.X < 0) SelectedTile.X = -1;
            if (Mouse.GetState().Position.Y + Screen_Scroll.Y < 0) SelectedTile.Y = -1;

            if (SelectedTile.X > 1000) SelectedTile.X = -1;
            if (SelectedTile.Y > 1000) SelectedTile.Y = -1;

            for (int x = 0; x < Enum.GetNames(typeof(Keys)).Length; x++)
            {
                if (Keyboard.GetState().IsKeyDown((Keys)x))
                    if (!PressedKeys.ContainsKey((Keys)x))
                        PressedKeys.Add((Keys)x, Listof_KeyStatus.Down);

                if (Keyboard.GetState().IsKeyUp((Keys)x))
                    if (PressedKeys.ContainsKey((Keys)x) && PressedKeys[(Keys)x] == Listof_KeyStatus.Handled)
                        PressedKeys.Remove((Keys)x);
            }





            if (PressedKeys.ContainsKey(Keys.Tab) && PressedKeys[Keys.Tab] == Listof_KeyStatus.Down)
            {
                PressedKeys[Keys.Tab] = Listof_KeyStatus.Handled;
                if (GameMode == Listof_GameMode.Personal)
                    GameMode = Listof_GameMode.Combat;
                else
                    GameMode = Listof_GameMode.Personal;
            }


            if (PressedKeys.ContainsKey(Keys.P) && PressedKeys[Keys.P] == Listof_KeyStatus.Down)
            {
                PressedKeys[Keys.P] = Listof_KeyStatus.Handled;
                currentCombat.AdvanceToTurn();
                Combat_Block_Tiles();
                RecalculatePF = true;
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed) LeftMouseClicked = true;
            if (Mouse.GetState().RightButton == ButtonState.Pressed) RightMouseClicked = true;

            switch (GameMode)
            {
                case Listof_GameMode.Personal:
                    UI_Personal_Mode();
                    break;
                case Listof_GameMode.Combat:
                    UI_Combat_Mode();                                       
                    break;
                case Listof_GameMode.Strategy:
                    break;
            }

            if (Mouse.GetState().LeftButton == ButtonState.Released) LeftMouseClicked = false;
            if (Mouse.GetState().RightButton == ButtonState.Released) RightMouseClicked = false;

            RunScripts();

            UI_MouseDrag();                                              
            
            //Safeguards
            if (Screen_Zoom < 1.0f) Screen_Zoom = 1.0f;
            if (Screen_Zoom > 4.0f) Screen_Zoom = 4.0f;

            //if (Screen_Scroll.X < 0) Screen_Scroll.X = 0;
            //if (Screen_Scroll.X > GetATSI() * 1000 + Screen_Size.X) Screen_Scroll.X = GetATSI() * 1000 + Screen_Size.X;

            //if (Screen_Scroll.Y < 0) Screen_Scroll.Y = 0;
            //if (Screen_Scroll.Y > GetATSI() * 1000 + Screen_Size.Y) Screen_Scroll.Y = GetATSI() * 1000 + Screen_Size.Y;

            base.Update(gameTime);
        }


        void RunScripts()
        {
            bool advance = true;
            foreach(var a in ActorList)
            {
                if (a.Value.MoveQueue.Count > 0 )
                {
                    advance = false;
                    Point dest = a.Value.MoveQueue[0];
                    Point2 m = new Point2();
                    if (a.Value.Location.X < dest.X) m.X = 2f;
                    if (a.Value.Location.X > dest.X) m.X = -2f;
                    if (a.Value.Location.Y < dest.Y) m.Y = 2f;
                    if (a.Value.Location.Y > dest.Y) m.Y = -2f;

                    if (m.X != 0 && m.Y != 0)
                    {
                        m.X *= DIAGONAL_SPEED_MODIFIER;
                        m.Y *= DIAGONAL_SPEED_MODIFIER;
                    }

                    if (Math.Abs(a.Value.Location.X - dest.X) <= m.X)
                        a.Value.Location.X = dest.X;
                    else
                        a.Value.Location.X += m.X;

                    if (Math.Abs(a.Value.Location.Y - dest.Y) <= m.Y)
                        a.Value.Location.Y = dest.Y;
                    else
                        a.Value.Location.Y += m.Y;

                    if (a.Value.Location.X == dest.X && a.Value.Location.Y == dest.Y)
                        a.Value.MoveQueue.RemoveAt(0);
                    if (a.Value.MoveQueue.Count == 0)
                    {
                        if (currentCombat.CurrentTurn.Stats.Movement <= 0 )
                            currentCombat.MoveMode = Listof_MoveMode.Off;
                        else
                        {
                            currentCombat.MoveMode = Listof_MoveMode.Waitingforcommand;
                            path = null;
                        }
                            

                        //currentCombat.CurrentTurn.UsedMove = true;
                    }
                }
            }
        }









        void UI_Personal_Mode()
        {            
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                Player.Location.Y -= 1;
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                Player.Location.Y += 1;
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                Player.Location.X -= 1;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                Player.Location.X += 1;


            if (Keyboard.GetState().IsKeyUp(Keys.E))
                InventoryKey = false;

            if (Keyboard.GetState().IsKeyDown(Keys.E))
                if (!InventoryKey)
                {
                    if (InventoryOpen)
                        InventoryOpen = false;
                    else
                        InventoryOpen = true;
                    InventoryKey = true;
                }


            TileInReach = false;
            if (new Rectangle(Player.Reach.Location + Player.getTile(), Player.Reach.Size).Contains(SelectedTile))
                TileInReach = true;

            if (Player.ItemCooldown > 0)
                Player.ItemCooldown--;
            if (Player.ItemCooldown == 0)
                Player.UsedItem = false;


            if (!InventoryOpen)
                UI_PersonalHandleMouse();
            
            if (InventoryOpen)
                UI_CheckInventory();
            else
                UI_Bar();

        }


        void UI_Combat_Mode()
        {


            Point pos = Mouse.GetState().Position;

            if (Mouse.GetState().LeftButton == ButtonState.Released && LeftMouseClicked == true)
                foreach (var item in gui.CombatElements)
                    //Crafting Buttons
                    if (item.Value.Location.Contains(pos))
                    {
                        if (item.Key == (int)UICombatElement.Action_Move && !currentCombat.CurrentTurn.UsedMove)
                        {
                            currentCombat.MoveMode  = Listof_MoveMode.Waitingforcommand;
                            currentCombat.AttackMode = Listof_AttackMode.Off;
                            path = null;
                            LeftMouseClicked = false;
                        }

                        if (item.Key == (int)UICombatElement.Action_Attack && !currentCombat.CurrentTurn.UsedAction)
                        {
                            currentCombat.MoveMode = Listof_MoveMode.Off;
                            currentCombat.AttackMode = Listof_AttackMode.Waitingforcommand;
                            path = null;
                            LeftMouseClicked = false;
                        }
                    }

            
            if (currentCombat.MoveMode == Listof_MoveMode.Waitingforcommand)
            {                

                if (SelectedTile.X >= 0 && SelectedTile.Y >= 0 && SelectedTile.X <= 1000 && SelectedTile.Y <= 1000 && (SelectedTile != LastSelectedTile || RecalculatePF))
                {
                    Point p = currentCombat.CurrentTurn.getTile();
                    //pathfind.Standard_Pathfind(p, SelectedTile, currentCombat.CurrentTurn.Stats.Movement);
                    pathfind.Standard_Pathfind(p, SelectedTile, 20);
                    LastSelectedTile = SelectedTile;
                    RecalculatePF = false;
                    if (pathfind.Found_State == PFState.Found)
                    {
                        path = pathfind.Path;                        

                        int count = (path.Count - 1) - currentCombat.CurrentTurn.Stats.Movement;
                        if (count > 0)
                            for (int x = path.Count - 1; x > currentCombat.CurrentTurn.Stats.Movement; x--)
                                path.RemoveAt(x);
                    }
                    
                }                

                if (Mouse.GetState().LeftButton == ButtonState.Released && LeftMouseClicked == true && path != null && path.Count != 0 && SelectedTile == path[path.Count - 1])
                {
                    Point p = currentCombat.CurrentTurn.getTile();
                    tilemap[p.X, p.Y].Occupied = -1;

                    foreach (var item in path)
                        currentCombat.CurrentTurn.MoveQueue.Add(item * new Point(32, 32));
                    
                    currentCombat.CurrentTurn.Stats.Movement -= path.Count - 1;
                    if (currentCombat.CurrentTurn.Stats.Movement <= 0) currentCombat.CurrentTurn.UsedMove = true;
                    currentCombat.MoveMode = Listof_MoveMode.Moving;
                    //path = null;
                }               

                if (Mouse.GetState().RightButton == ButtonState.Released && RightMouseClicked == true)
                {                    
                    if (Math.Abs(MouseDragScreenScrollStart.X - Screen_Scroll.X) < 4  && Math.Abs(MouseDragScreenScrollStart.Y - Screen_Scroll.Y) < 4)
                    {
                        currentCombat.MoveMode = Listof_MoveMode.Off;                        
                        RightMouseClicked = false;
                    }                        
                }

            }

            if (currentCombat.AttackMode == Listof_AttackMode.Waitingforcommand)
            {

                if (Mouse.GetState().LeftButton == ButtonState.Released && LeftMouseClicked == true)
                {
                    Point p = currentCombat.CurrentTurn.getTile();
                    if (tilemap[SelectedTile.X, SelectedTile.Y].Occupied > -1)
                    {
                        ActorList[tilemap[SelectedTile.X, SelectedTile.Y].Occupied].Stats.HP -= 40;
                        if (ActorList[tilemap[SelectedTile.X, SelectedTile.Y].Occupied].Stats.HP <= 0 )
                        {
                            currentCombat.Remove_Actor(ActorList[tilemap[SelectedTile.X, SelectedTile.Y].Occupied]);
                            ActorList.Remove(tilemap[SelectedTile.X, SelectedTile.Y].Occupied);                            
                            tilemap[SelectedTile.X, SelectedTile.Y].Occupied = -1;
                        }
                        currentCombat.CurrentTurn.UsedAction = true;
                        currentCombat.AttackMode = Listof_AttackMode.Off;
                    }
                }


                if (Mouse.GetState().RightButton == ButtonState.Released && RightMouseClicked == true)
                {
                    if (Math.Abs(MouseDragScreenScrollStart.X - Screen_Scroll.X) < 4 && Math.Abs(MouseDragScreenScrollStart.Y - Screen_Scroll.Y) < 4)
                    {
                        currentCombat.AttackMode = Listof_AttackMode.Off;
                        RightMouseClicked = false;
                    }
                }

            }

        }


        void UI_MouseDrag()
        {
            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                if (!MouseDraging)
                {
                    MouseDraging = true;
                    MouseDragStart = Screen_Scroll + Mouse.GetState().Position;
                    MouseDragScreenScrollStart = Screen_Scroll;
                }
            }

            if (MouseDraging)
            {
                Screen_Scroll = (MouseDragStart - Mouse.GetState().Position);
                if (Mouse.GetState().RightButton != ButtonState.Pressed) MouseDraging = false;
            }
        }


        void UI_PersonalHandleMouse()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                
                Tile_Type t = tilemap[SelectedTile.X, SelectedTile.Y];
                if (SelectedTile.X >= 0 && SelectedTile.X <= 1000 && SelectedTile.Y >= 0 && SelectedTile.Y <= 1000 && TileInReach)
                {
                    if (Player.UsedItem == false)
                        if (t.Structure != null)
                            if (t.Structure.Type == Listof_Structures.Tree1)
                                UI_CheckTree(t);

                    if (Player.UsedItem == false)
                        if (t.Structure != null)
                            if (t.Structure.Type == Listof_Structures.Stone)
                                UI_CheckStone(t);


                    if (t.Structure == null)
                        UI_BuildItem(t);




                }
            }




        }

        void UI_BuildItem(Tile_Type t)
        {
            if (Player.Loadout.UsableItems.ContainsKey(Player.SelectedItem))
            {
                InventoryItem item = Player.Loadout.UsableItems[Player.SelectedItem];
                if (C_Item[item.item].BuildTile != Listof_Structures.None)
                {
                    t.Structure = new Structure_Type() { Type = C_Item[item.item].BuildTile, Durability = C_Structure[C_Item[item.item].BuildTile].Durability };

                    item.Amount--;
                    if (item.Amount <= 0)
                        Player.Loadout.UsableItems.Remove(Player.SelectedItem);
                }
            }
        }



        void UI_CheckTree(Tile_Type t)
        {
            
                bool UseFist = false;
                if (Player.Loadout.UsableItems.ContainsKey(Player.SelectedItem))
                {
                    InventoryItem item = Player.Loadout.UsableItems[Player.SelectedItem];
                    if (C_Item[item.item].HarvestWood)
                        if (C_Item[item.item].PowerTier >= (int)C_Structure[t.Structure.Type].Hardness)
                        {
                            t.Structure.Durability -= C_Item[item.item].Power;
                            Player.UseItem();
                            if (t.Structure.Durability <= 0)
                        {
                            if (C_Structure[t.Structure.Type].DropsResource)
                            {
                                Player.inventory.AddResource(C_Structure[t.Structure.Type].ResourceDrop.Value, C_Structure[t.Structure.Type].ResourceDrop.Key);
                                t.Structure = null;
                            }
                        }

                    }
                        else
                            UseFist = true;
                    else
                        UseFist = true;

                }
                else
                    UseFist = true;

                if (UseFist)
                    if (C_Structure[t.Structure.Type].Hardness <= 0)
                    {
                        t.Structure.Durability -= 1;
                        Player.UseItem();
                        if (t.Structure.Durability <= 0)
                        {
                        if (C_Structure[t.Structure.Type].DropsResource)
                        {
                            Player.inventory.AddResource(C_Structure[t.Structure.Type].ResourceDrop.Value, C_Structure[t.Structure.Type].ResourceDrop.Key);
                            t.Structure = null;
                        }
                    }
                    }            

        }

        void UI_CheckStone(Tile_Type t)
        {
            if (Player.Loadout.UsableItems.ContainsKey(Player.SelectedItem))
            {
                InventoryItem item = Player.Loadout.UsableItems[Player.SelectedItem];
                if (C_Item[item.item].HarvestStone)
                    if (C_Item[item.item].PowerTier >= (int)C_Structure[t.Structure.Type].Hardness)
                    {
                        t.Structure.Durability -= C_Item[item.item].Power;
                        Player.UseItem();
                        if (t.Structure.Durability <= 0)
                        {
                            if (C_Structure[t.Structure.Type].DropsResource)
                            {
                                Player.inventory.AddResource(C_Structure[t.Structure.Type].ResourceDrop.Value, C_Structure[t.Structure.Type].ResourceDrop.Key);
                                t.Structure = null;
                            }
                        }
                    }
            }
        }

        void UI_Bar()
        {

            for (int x = 0; x < 9; x++)
            {
                if (Keyboard.GetState().IsKeyDown((Keys)49 + x))
                    if (Player.Loadout.UsableItems.ContainsKey(x))
                        Player.SelectedItem = x;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D0))
                if (Player.Loadout.UsableItems.ContainsKey(9))
                    Player.SelectedItem = 9;


            Point pos = Mouse.GetState().Position;
            if (Mouse.GetState().LeftButton == ButtonState.Released && LeftMouseClicked == true)
            {
                foreach (var item in gui.InventoryElements)
                {
                    if (item.Value.Location.Contains(pos))
                    {
                        if (item.Key >= 4000 && item.Key < 5000)
                        {
                            if (Player.Loadout.UsableItems.ContainsKey(item.Key - 4000))
                                Player.SelectedItem = item.Key - 4000;
                        }
                    }
                }
            }
        }


        void UI_CheckInventory()
        {
            Point pos = Mouse.GetState().Position;

            if (Mouse.GetState().LeftButton == ButtonState.Released && LeftMouseClicked == true)
                foreach(var item in gui.InventoryElements)
                    //Crafting Buttons
                    if (item.Value.Location.Contains(pos))
                    { 
                        if (item.Key >= 2000 && item.Key < 3000)
                        {
                            int x = 0;
                            foreach (var bp in Player.CraftingBlueprints.Blueprints)
                            {                                                                
                                if (x == item.Key - 2000)
                                {
                                    bool CanCaft = true;
                                    foreach (var craftitem in C_Crafting[bp].ResourceCost)
                                        if (Player.inventory.ResourceCount(craftitem.Key) < craftitem.Value)
                                        { CanCaft = false; break; }

                                    if (CanCaft)
                                    {
                                        if (Player.inventory.AddItem(new InventoryItem(bp, 1)))
                                            foreach (var craftitem in C_Crafting[bp].ResourceCost)
                                                Player.inventory.RemoveResource(craftitem.Key, craftitem.Value);
                                        
                                    }
                                    break;
                                }
                                
                                x++;
                            }
                                
                        }

                        //Item Buttons
                        if (item.Key >= 1024 && item.Key < 2000)
                        {
                            //Pick up item with empty hand
                            if (MouseHeldItem == null)
                            {
                                if (Player.inventory.Items.ContainsKey(item.Key - 1024))
                                {
                                    MouseHeldItem = Player.inventory.Items[item.Key - 1024];
                                    Player.inventory.Items.Remove(item.Key - 1024);
                                }
                            }
                            else
                            {
                                //Drop item to empty slot
                                if (!Player.inventory.Items.ContainsKey(item.Key - 1024))
                                {
                                    Player.inventory.Items[item.Key - 1024] = MouseHeldItem;
                                    MouseHeldItem = null;                                    
                                }
                                else
                                //Switch with item in hand
                                {
                                    InventoryItem temp = Player.inventory.Items[item.Key - 1024];
                                    if (Player.inventory.Items[item.Key - 1024].item != MouseHeldItem.item)
                                    {
                                        Player.inventory.Items[item.Key - 1024] = MouseHeldItem;
                                        MouseHeldItem = temp;
                                    }
                                    else
                                    {
                                        Player.inventory.Items[item.Key - 1024].Amount += MouseHeldItem.Amount;
                                        MouseHeldItem = null;
                                    }
                                    
                                }
                            }

                        }

                        //Equipment Panel
                        if (item.Key >= 3000 && item.Key < 4000)
                        {

                            //Hand is Empty
                            if (MouseHeldItem == null)
                            {
                                //Space has item
                                if (Player.Loadout.Equipment[item.Key - 3000] != null)
                                {
                                    MouseHeldItem = Player.Loadout.Equipment[item.Key - 3000];
                                    Player.Loadout.Equipment[item.Key - 3000] = null;
                                }


                            }
                            else
                            //Hand has item
                            {
                                //Space is empty
                                if (Player.Loadout.Equipment[item.Key - 3000] == null)
                                {
                                    if ((item.Key - 3000 == 0 && C_Item[MouseHeldItem.item].ItemType == Listof_ItemTypes.Headpiece)
                                        | (item.Key - 3000 == 1 && C_Item[MouseHeldItem.item].ItemType == Listof_ItemTypes.Armor)
                                        | (item.Key - 3000 == 2 && C_Item[MouseHeldItem.item].ItemType == Listof_ItemTypes.Accessory)
                                        | (item.Key - 3000 == 3 && C_Item[MouseHeldItem.item].ItemType == Listof_ItemTypes.Accessory)
                                        | (item.Key - 3000 == 4 && C_Item[MouseHeldItem.item].ItemType == Listof_ItemTypes.WeaponOneHanded)
                                        | (item.Key - 3000 == 4 && C_Item[MouseHeldItem.item].ItemType == Listof_ItemTypes.WeaponTwoHanded)
                                        | (item.Key - 3000 == 4 && C_Item[MouseHeldItem.item].ItemType == Listof_ItemTypes.Shield)
                                        | (item.Key - 3000 == 5 && C_Item[MouseHeldItem.item].ItemType == Listof_ItemTypes.WeaponOneHanded)
                                        | (item.Key - 3000 == 5 && C_Item[MouseHeldItem.item].ItemType == Listof_ItemTypes.WeaponTwoHanded)
                                        | (item.Key - 3000 == 5 && C_Item[MouseHeldItem.item].ItemType == Listof_ItemTypes.Shield))
                                    {
                                        Player.Loadout.Equipment[item.Key - 3000] = new InventoryItem(MouseHeldItem.item, 1);
                                        MouseHeldItem.Amount--;
                                        if (MouseHeldItem.Amount <= 0) MouseHeldItem = null;
                                    }                                        
                                }
                                else
                                //Space has item
                                {
                                    if (MouseHeldItem.Amount == 1)
                                    {
                                        if ((item.Key - 3000 == 0 && C_Item[MouseHeldItem.item].ItemType == Listof_ItemTypes.Headpiece)
                                        | (item.Key - 3000 == 1 && C_Item[MouseHeldItem.item].ItemType == Listof_ItemTypes.Armor)
                                        | (item.Key - 3000 == 2 && C_Item[MouseHeldItem.item].ItemType == Listof_ItemTypes.Accessory)
                                        | (item.Key - 3000 == 3 && C_Item[MouseHeldItem.item].ItemType == Listof_ItemTypes.Accessory)
                                        | (item.Key - 3000 == 4 && C_Item[MouseHeldItem.item].ItemType == Listof_ItemTypes.WeaponOneHanded)
                                        | (item.Key - 3000 == 4 && C_Item[MouseHeldItem.item].ItemType == Listof_ItemTypes.WeaponTwoHanded)
                                        | (item.Key - 3000 == 4 && C_Item[MouseHeldItem.item].ItemType == Listof_ItemTypes.Shield)
                                        | (item.Key - 3000 == 5 && C_Item[MouseHeldItem.item].ItemType == Listof_ItemTypes.WeaponOneHanded)
                                        | (item.Key - 3000 == 5 && C_Item[MouseHeldItem.item].ItemType == Listof_ItemTypes.WeaponTwoHanded)
                                        | (item.Key - 3000 == 5 && C_Item[MouseHeldItem.item].ItemType == Listof_ItemTypes.Shield))
                                        {
                                            InventoryItem temp = Player.Loadout.Equipment[item.Key - 3000];
                                            Player.Loadout.Equipment[item.Key - 3000] = MouseHeldItem;
                                            MouseHeldItem = temp;
                                        }
                                    }                                    
                                }

                            }
                        }
                    

                        //Hotbar buttons
                        if (item.Key >= 4000 && item.Key < 5000)
                        {
                            //Hand is Empty
                            if (MouseHeldItem == null)
                            {
                                //Space has item
                                if (Player.Loadout.UsableItems.ContainsKey(item.Key - 4000))
                                {
                                    MouseHeldItem = Player.Loadout.UsableItems[item.Key - 4000];
                                    Player.Loadout.UsableItems.Remove(item.Key - 4000);
                                }                                


                            }                            
                            else
                            //Hand has item
                            {
                                //Space is empty
                                if (!Player.Loadout.UsableItems.ContainsKey(item.Key - 4000))
                                {
                                    Player.Loadout.UsableItems.Add(item.Key - 4000, MouseHeldItem);
                                    MouseHeldItem = null;                                    
                                }
                                else
                                //Space has item
                                {
                                    InventoryItem temp = Player.Loadout.UsableItems[item.Key - 4000];
                                    if (Player.Loadout.UsableItems[item.Key - 4000].item != MouseHeldItem.item)
                                    {
                                        Player.Loadout.UsableItems[item.Key - 4000] = MouseHeldItem;
                                        MouseHeldItem = temp;
                                    }
                                    else
                                    {
                                        Player.Loadout.UsableItems[item.Key - 4000].Amount += MouseHeldItem.Amount;
                                        MouseHeldItem = null;
                                    }                                    
                                }

                            }                            
                        }
                    }


            if (Mouse.GetState().RightButton == ButtonState.Released && RightMouseClicked == true)
                foreach (var item in gui.InventoryElements)
                    if (item.Value.Location.Contains(pos))
                    {
                        if (item.Key >= 2000 && item.Key < 3000)
                        {
                            for (int count = 0; count < 5; count++)
                            {
                                int x = 0;
                                foreach (var bp in Player.CraftingBlueprints.Blueprints)
                                {
                                    if (x == item.Key - 2000)
                                    {
                                        bool CanCaft = true;
                                        foreach (var craftitem in C_Crafting[bp].ResourceCost)
                                            if (Player.inventory.ResourceCount(craftitem.Key) < craftitem.Value)
                                            { CanCaft = false; break; }

                                        if (CanCaft)
                                        {
                                            if (Player.inventory.AddItem(new InventoryItem(bp, 1)))
                                                foreach (var craftitem in C_Crafting[bp].ResourceCost)
                                                    Player.inventory.RemoveResource(craftitem.Key, craftitem.Value);

                                        }
                                        break;
                                    }

                                    x++;
                                }
                            }
                        }


                        if (item.Key >= 1024 && item.Key < 2000)
                        {
                            //Pick up item with empty hand
                            if (MouseHeldItem == null)
                            {
                                if (Player.inventory.Items.ContainsKey(item.Key - 1024))
                                {
                                    int hand, inv;
                                    decimal start;
                                    start = Player.inventory.Items[item.Key - 1024].Amount;
                                    hand = (int)Math.Floor(start / 2.0m);
                                    inv = (int)Math.Ceiling(start / 2.0m);

                                    MouseHeldItem = new InventoryItem(Player.inventory.Items[item.Key - 1024].item, 0);
                                    MouseHeldItem.Amount = hand;
                                    Player.inventory.Items[item.Key - 1024].Amount = inv;

                                    if (MouseHeldItem.Amount <= 0) MouseHeldItem = null;

                                    if (Player.inventory.Items[item.Key - 1024].Amount <= 0)
                                        Player.inventory.Items.Remove(item.Key - 1024);
                                }
                            }
                            else
                            {
                                //Drop item to empty slot
                                if (!Player.inventory.Items.ContainsKey(item.Key - 1024))
                                {
                                    Player.inventory.Items[item.Key - 1024] = new InventoryItem(MouseHeldItem.item, 1);
                                    MouseHeldItem.Amount--;
                                    if (MouseHeldItem.Amount <= 0) MouseHeldItem = null;
                                }
                                else
                                //Switch with item in hand
                                {
                                    InventoryItem temp = Player.inventory.Items[item.Key - 1024];
                                    if (Player.inventory.Items[item.Key - 1024].item != MouseHeldItem.item)
                                    {
                                        Player.inventory.Items[item.Key - 1024] = MouseHeldItem;
                                        MouseHeldItem = temp;
                                    }
                                    else
                                    {
                                        Player.inventory.Items[item.Key - 1024].Amount++;
                                        MouseHeldItem.Amount--;
                                        if (MouseHeldItem.Amount <= 0) MouseHeldItem = null;                                        
                                    }
                                }
                            }

                        }

                        if (item.Key >= 4000 && item.Key < 5000)
                        {
                            //Hand is Empty
                            if (MouseHeldItem == null)
                            {
                                //Space has item
                                if (Player.Loadout.UsableItems.ContainsKey(item.Key - 4000))
                                {
                                    InventoryItem BItem = Player.Loadout.UsableItems[item.Key - 4000];
                                    int hand, inv;
                                    decimal start;
                                    start = BItem.Amount;
                                    hand = (int)Math.Floor(start / 2.0m);
                                    inv = (int)Math.Ceiling(start / 2.0m);

                                    MouseHeldItem = new InventoryItem(BItem.item, 0);
                                    MouseHeldItem.Amount = hand;
                                    BItem.Amount = inv;

                                    if (MouseHeldItem.Amount <= 0) MouseHeldItem = null;

                                    if (BItem.Amount <= 0)
                                        Player.Loadout.UsableItems.Remove(item.Key - 4000);                                                                  
                                }
                            }
                            else
                            //Hand has item
                            {
                                //Space is empty
                                if (!Player.Loadout.UsableItems.ContainsKey(item.Key - 4000))
                                {                                    
                                    Player.Loadout.UsableItems.Add(item.Key - 4000, new InventoryItem(MouseHeldItem.item, 1));
                                    MouseHeldItem.Amount--;
                                    if (MouseHeldItem.Amount <= 0) MouseHeldItem = null;                                    
                                }
                                else
                                //Space has item
                                {
                                    InventoryItem temp = Player.Loadout.UsableItems[item.Key - 4000];
                                    if (Player.Loadout.UsableItems[item.Key - 4000].item != MouseHeldItem.item)
                                    {
                                        Player.Loadout.UsableItems[item.Key - 4000] = MouseHeldItem;
                                        MouseHeldItem = temp;
                                    }
                                    else
                                    {
                                        Player.Loadout.UsableItems[item.Key - 4000].Amount++;
                                        MouseHeldItem.Amount--;
                                        if (MouseHeldItem.Amount <= 0) MouseHeldItem = null;
                                    }                                 
                                }

                            }
                        }
                    }


        }

        //Limit to once
        void Combat_Block_Tiles()
        {
            foreach(var a in ActorList)
            {
                Point pos = a.Value.getTile();
                tilemap[pos.X, pos.Y].Occupied = a.Key;
            }
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            
            Point pos, sel_pos;
            Rectangle ScreenR;
            sel_pos = (Mouse.GetState().Position + Screen_Scroll) / new Point(GetATSI(), GetATSI());

            ScreenR.X = Screen_Scroll.X / GetATSI();
            ScreenR.Y = Screen_Scroll.Y / GetATSI();
            ScreenR.Width = (int)Math.Ceiling((double)Screen_Size.X / GetATSI());
            ScreenR.Height = (int)Math.Ceiling((double)Screen_Size.Y / GetATSI());
            ScreenR.Inflate(1, 1);
            for (int x = ScreenR.Left; x < ScreenR.Right; x++)
                for (int y = ScreenR.Top; y < ScreenR.Bottom; y++)
                {
                    pos.X = x * GetATSI() - Screen_Scroll.X;
                    pos.Y = y * GetATSI() - Screen_Scroll.Y;
                    if (x >= 0 && x < 1000 && y >= 0 && y < 1000)
                    {
                        //if (sel_pos == new Point(x, y))
                        //spriteBatch.Draw(Background_Tiles[tilemap[x, y].Sprite], new Rectangle(pos.X, pos.Y, GetATSI(), GetATSI()), new Color(80, 80, 180));
                        //else
                        //{

                        byte dur = 255;
                        spriteBatch.Draw(Background_Tiles[0], new Rectangle(pos.X, pos.Y, GetATSI(), GetATSI()), new Rectangle(C_Tile[tilemap[x, y].ID].Sprite * 32, 0, 32, 32), new Color(dur, dur, dur));

                        if (GameMode == Listof_GameMode.Combat)
                        {
                            /*
                            if (currentCombat.CurrentTurn != null)
                            {
                                Point actorP = currentCombat.CurrentTurn.getTile();
                                if (actorP.X == x && actorP.Y == y)
                                    spriteBatch.Draw(Background_Tiles[0], new Rectangle(pos.X, pos.Y, GetATSI(), GetATSI()), new Rectangle(C_Tile[tilemap[x, y].ID].Sprite * 32, 0, 32, 32), new Color(150, 150, 255));
                            }
                            */
                            if (path != null && currentCombat.MoveMode == Listof_MoveMode.Waitingforcommand)
                            {
                                if (path.Contains(new Point(x, y)))
                                {
                                    spriteBatch.Draw(Background_Tiles[0], new Rectangle(pos.X, pos.Y, GetATSI(), GetATSI()), new Rectangle(C_Tile[tilemap[x, y].ID].Sprite * 32, 0, 32, 32), new Color(150, 255, 150));
                                    spriteBatch.DrawString(basicfont, path.IndexOf(new Point(x, y)).ToString(), new Rectangle(pos.X, pos.Y, GetATSI(), GetATSI()).Location.ToVector2() + new Vector2(8, 8), Color.White);
                                }                                    
                            }
                            
                            if (currentCombat.MoveMode == Listof_MoveMode.Moving)
                            {
                                if (currentCombat.CurrentTurn.MoveQueue.Contains(new Point(x * 32, y * 32)))
                                {
                                    spriteBatch.Draw(Background_Tiles[0], new Rectangle(pos.X, pos.Y, GetATSI(), GetATSI()), new Rectangle(C_Tile[tilemap[x, y].ID].Sprite * 32, 0, 32, 32), new Color(150, 255, 150));
                                    spriteBatch.DrawString(basicfont, path.IndexOf(new Point(x, y)).ToString(), new Rectangle(pos.X, pos.Y, GetATSI(), GetATSI()).Location.ToVector2() + new Vector2(8, 8), Color.White);
                                    //spriteBatch.DrawString(basicfont, (currentCombat.CurrentTurn.MoveQueue.Count - currentCombat.CurrentTurn.MoveQueue.IndexOf(new Point(x * 32, y * 32))).ToString() , new Rectangle(pos.X, pos.Y, GetATSI(), GetATSI()).Location.ToVector2() + new Vector2(8, 8), Color.White);
                                }
                                    
                            }

                            if (currentCombat.AttackMode == Listof_AttackMode.Waitingforcommand)
                            {
                                //Rectangle apos = new Rectangle(currentCombat.CurrentTurn.getTile(), new Point(1, 1));
                                //apos.Inflate(1, 1);
                                Point p = currentCombat.CurrentTurn.getTile();

                                CreateCircle(p.X, p.Y, 1);
                                CreateCircle(p.X, p.Y, 2);
                                CreateCircle(p.X, p.Y, 3);
                                CreateCircle(p.X, p.Y, 4);
                                CreateCircle(p.X, p.Y, 5);
                                CreateCircle(p.X, p.Y, 6);
                                CreateCircle(p.X, p.Y, 7);
                                CreateCircle(p.X, p.Y, 8);

                                if (TileCircle.Contains(new Point(x, y)))
                                { 
                                    spriteBatch.Draw(Background_Tiles[0], new Rectangle(pos.X, pos.Y, GetATSI(), GetATSI()), new Rectangle(C_Tile[tilemap[x, y].ID].Sprite * 32, 0, 32, 32), new Color(255, 150, 150));
                                }
                            }



                            //spriteBatch.Draw(Background_Tiles[0], new Rectangle(pos.X, pos.Y, GetATSI(), GetATSI()), new Color(dur, dur, dur));
                        }
                        if (tilemap[x, y].Structure != null)
                        {
                            dur = (byte)((255 / C_Structure[tilemap[x, y].Structure.Type].Durability) * tilemap[x, y].Structure.Durability);

                            if (tilemap[x, y].Structure.Type == Listof_Structures.Tree1)
                                spriteBatch.Draw(Item_Tiles[0], new Rectangle(pos.X, pos.Y - GetATSI() * 2, GetATSI(), GetATSI() * 3), new Color(dur, dur, dur));

                            if (tilemap[x, y].Structure.Type == Listof_Structures.Stone)
                                spriteBatch.Draw(Background_Tiles[0], new Rectangle(pos.X, pos.Y, GetATSI(), GetATSI()), new Rectangle(1 * 32, 0, 32, 32), new Color(dur, dur, dur));

                            if (tilemap[x, y].Structure.Type == Listof_Structures.StoneThrone)
                                spriteBatch.Draw(UI_Textures[(int)SD_UI.Items], new Rectangle(pos.X, pos.Y, GetATSI(), GetATSI()), new Rectangle((int)Listof_InventoryItem.StoneThrone * 32, 0, 32, 32), new Color(dur, dur, dur));


                        }
                            
                    }
                    
                    //if (tilemap[x, y] != 0)
                    //spriteBatch.Draw(Background_Tiles[tilemap[x, y]], new Rectangle(pos.X, pos.Y, 64, 64), Color.White);
                }

            //Draw Personal SelectBox
            if (GameMode == Listof_GameMode.Personal)
            {
                Rectangle sbox = new Rectangle(Player.Reach.Location + Player.getTile(), Player.Reach.Size);
                if (sbox.Contains(SelectedTile) && SelectedTile.X >= 0 && SelectedTile.Y >= 0 && SelectedTile.X <= 1000 && SelectedTile.Y <= 1000)
                {                    
                        pos.X = SelectedTile.X * GetATSI() - Screen_Scroll.X;
                        pos.Y = SelectedTile.Y * GetATSI() - Screen_Scroll.Y;
                        //spriteBatch.Draw(Item_Tiles[0], new Rectangle(pos.X, pos.Y - GetATSI() * 2, GetATSI(), GetATSI() * 3), new Color(dur, dur, dur));
                        spriteBatch.Draw(UI_Textures[(int)SD_UI.SelectBox], new Rectangle(pos.X, pos.Y, GetATSI(), GetATSI()), Color.White);                 
                }
            }

            if (GameMode == Listof_GameMode.Combat)
            {
                if (SelectedTile.X >= 0 && SelectedTile.Y >= 0 && SelectedTile.X <= 1000 && SelectedTile.Y <= 1000)
                {
                    pos.X = SelectedTile.X * GetATSI() - Screen_Scroll.X;
                    pos.Y = SelectedTile.Y * GetATSI() - Screen_Scroll.Y;
                    //spriteBatch.Draw(Item_Tiles[0], new Rectangle(pos.X, pos.Y - GetATSI() * 2, GetATSI(), GetATSI() * 3), new Color(dur, dur, dur));
                    spriteBatch.Draw(UI_Textures[(int)SD_UI.SelectBox], new Rectangle(pos.X, pos.Y, GetATSI(), GetATSI()), Color.White);
                }
            }

            //spriteBatch.Draw(Actor_Sprites[0], new Rectangle((int)(Player.Location.X * Screen_Zoom - Screen_Scroll.X), (int)(Player.Location.Y * Screen_Zoom - Screen_Scroll.Y), GetATSI(), GetATSI()), Color.White);
            DrawActors();

            if (currentCombat.CurrentTurn != null)
            {
                spriteBatch.Draw(UI_Textures[(int)SD_UI.SelectBox], new Rectangle(
                                    (int)((currentCombat.CurrentTurn.Location.X + currentCombat.CurrentTurn.Hitbox.X) * Screen_Zoom - Screen_Scroll.X),
                                    (int)((currentCombat.CurrentTurn.Location.Y + currentCombat.CurrentTurn.Hitbox.Y) * Screen_Zoom - Screen_Scroll.Y),
                                    (int)(currentCombat.CurrentTurn.Hitbox.Width * Screen_Zoom),
                                    (int)(currentCombat.CurrentTurn.Hitbox.Height * Screen_Zoom))
                                    , Color.White);
            }

            if (GameMode == Listof_GameMode.Personal)
            {
                if (InventoryOpen)
                    DrawInventory();
                else
                    DrawBar();
            }


            if (GameMode == Listof_GameMode.Combat)
            {

                DrawCombat();


            }

                if (MouseHeldItem != null)
            {
                Point p;
                p.X = Mouse.GetState().Position.X - 16;
                p.Y = Mouse.GetState().Position.Y - 16;
                spriteBatch.Draw(UI_Textures[(int)SD_UI.Items], new Rectangle(p.X, p.Y, 32, 32), new Rectangle((int)MouseHeldItem.item * 32, 0, 32, 32), Color.White);
                spriteBatch.DrawString(basicfont, MouseHeldItem.Amount.ToString(), new Vector2(p.X, p.Y), Color.White);
            }
            

            spriteBatch.End();
            base.Draw(gameTime);
        }


        void DrawActors()
        {
            foreach (var a in ActorList)
            {
                spriteBatch.Draw(Actor_Sprites[0],
                    new Rectangle((int)(a.Value.Location.X * Screen_Zoom - Screen_Scroll.X), (int)(a.Value.Location.Y * Screen_Zoom - Screen_Scroll.Y), GetATSI(), GetATSI()),
                    new Rectangle(a.Value.Sprite * 64, 0, 64, 64),
                    Color.White);
            }
        }


        void DrawBar()
        {
            GUIElementType bar = gui.InventoryElements[(int)UIInventoryElement.BackBottomBar];

            if (!InventoryOpen)
            {
                spriteBatch.Draw(UI_Textures[(int)bar.Sprite], bar.Location, bar.color);
                for (int x = 4000; x < 4010; x++)
                {
                    if (gui.InventoryElements[x].Sprite != SD_UI.None)
                        spriteBatch.Draw(UI_Textures[(int)gui.InventoryElements[x].Sprite], gui.InventoryElements[x].Location, gui.InventoryElements[x].color);                        
                }
            }
                

            int xpos = 0;
            Point p;
            p.X = bar.Location.X;
            p.Y = bar.Location.Y;

            for (int x = 0; x < 10; x++)
            {
                if (Player.Loadout.UsableItems.ContainsKey(x))
                {
                    InventoryItem item = Player.Loadout.UsableItems[x];
                    Color c = Color.White;                
                    if (Player.SelectedItem == x)
                        c = new Color(160, 160, 255);
                    else
                        c = Color.White;

                    spriteBatch.Draw(UI_Textures[(int)SD_UI.Button], new Rectangle(p.X + 6 + 72 * xpos, p.Y + 12, 64, 64), c);
                    spriteBatch.Draw(UI_Textures[(int)SD_UI.Items], new Rectangle(p.X + 6 + 72 * xpos, p.Y + 12, 64, 64), new Rectangle((int)item.item * 32, 0, 32, 32), c);
                    spriteBatch.DrawString(basicfont, item.Amount.ToString(), new Vector2(p.X + 6 + 72 * xpos, p.Y + 12), Color.White);
                }
                xpos++;
            }

        }


        void DrawInventory()
        {
            Point p;
            p.X = gui.InventoryElements[(int)UIInventoryElement.BackInventory].Location.X;
            p.Y = gui.InventoryElements[(int)UIInventoryElement.BackInventory].Location.Y;

            foreach (var item in gui.InventoryElements)
                if (item.Value.Enabled)
                    if (item.Value.Sprite != SD_UI.None)
                        spriteBatch.Draw(UI_Textures[(int)item.Value.Sprite], item.Value.Location, item.Value.color);

            int width = 6;
            int xpos = 0;
            int ypos = 0;
            //Draw Resources
            foreach (var item in Player.inventory.Resources)
            {
                spriteBatch.Draw(UI_Textures[(int)SD_UI.Button], new Rectangle(p.X + 27 + 38 * xpos, p.Y + 66 + 36 * ypos, 32, 32), Color.White);
                spriteBatch.Draw(UI_Textures[(int)SD_UI.Resources], new Rectangle(p.X + 27 + 38 * xpos, p.Y + 66 + 36 * ypos, 32, 32), new Rectangle((int)item.Key * 16, 0, 16, 16), Color.White);
                spriteBatch.DrawString(basicfont, item.Value.ToString(), new Vector2(p.X + 27 + 38 * xpos, p.Y + 66 + 36 * ypos), Color.White);
                
                //spriteBatch.DrawString(basicfont, (resourceDictionary.Data[item.Key].Value * item.Value).ToString() + "gp", new Vector2(p.X + 55 + 42 * xpos, p.Y + 114 + 42 * ypos), Color.White);
                //spriteBatch.DrawString(basicfont, (resourceDictionary.Data[item.Key].Weight * item.Value).ToString() + "wt", new Vector2(p.X + 55 + 42 * xpos, p.Y + 146 + 42 * ypos), Color.White);
                xpos++;
                if (xpos >= width)
                { xpos = 0; ypos++; }
            }

            xpos = 0;
            ypos = 4;
            //Draw Items
            for (int x = 0; x < Player.inventory.InventorySize; x++)
            {
                if (Player.inventory.Items.ContainsKey(x))
                {
                    var item = Player.inventory.Items[x];

                    spriteBatch.Draw(UI_Textures[(int)SD_UI.Button], new Rectangle(p.X + 27 + 38 * xpos, p.Y + 66 + 36 * ypos, 32, 32), Color.White);
                    spriteBatch.Draw(UI_Textures[(int)SD_UI.Items], new Rectangle(p.X + 27 + 38 * xpos, p.Y + 66 + 36 * ypos, 32, 32), new Rectangle((int)item.item * 32, 0, 32, 32), Color.White);
                    spriteBatch.DrawString(basicfont, item.Amount.ToString(), new Vector2(p.X + 27 + 38 * xpos, p.Y + 66 + 36 * ypos), Color.White);                

                    //spriteBatch.DrawString(basicfont, (resourceDictionary.Data[item.Key].Value * item.Value).ToString() + "gp", new Vector2(p.X + 55 + 42 * xpos, p.Y + 114 + 42 * ypos), Color.White);
                    //spriteBatch.DrawString(basicfont, (resourceDictionary.Data[item.Key].Weight * item.Value).ToString() + "wt", new Vector2(p.X + 55 + 42 * xpos, p.Y + 146 + 42 * ypos), Color.White);
                }
                xpos++;
                if (xpos >= width)
                { xpos = 0; ypos++; }
            }

            DrawInventoryEquipment();

            width = 6;
            xpos = 0;
            ypos = 0;

            p.X = gui.InventoryElements[(int)UIInventoryElement.BackCrafting].Location.X;
            p.Y = gui.InventoryElements[(int)UIInventoryElement.BackCrafting].Location.Y;
            //Draw Crafting
            foreach (var item in Player.CraftingBlueprints.Blueprints)
            {                
                bool CanCaft = true;
                foreach (var craftitem in C_Crafting[item].ResourceCost)
                    if (Player.inventory.ResourceCount(craftitem.Key) < craftitem.Value)
                    { CanCaft = false; break;}

                if (CanCaft)
                {
                    spriteBatch.Draw(UI_Textures[(int)SD_UI.Button], new Rectangle(p.X + 27 + 38 * xpos, p.Y + 66 + 36 * ypos, 32, 32), Color.White);
                    spriteBatch.Draw(UI_Textures[(int)SD_UI.Items], new Rectangle(p.X + 27 + 38 * xpos, p.Y + 66 + 36 * ypos, 32, 32), new Rectangle((int)item * 32, 0, 32, 32), Color.White);
                }
                else
                {
                    spriteBatch.Draw(UI_Textures[(int)SD_UI.Button], new Rectangle(p.X + 27 + 38 * xpos, p.Y + 66 + 36 * ypos, 32, 32), Color.Gray);
                    spriteBatch.Draw(UI_Textures[(int)SD_UI.Items], new Rectangle(p.X + 27 + 38 * xpos, p.Y + 66 + 36 * ypos, 32, 32), new Rectangle((int)item * 32, 0, 32, 32), Color.Gray);
                }
                
                //spriteBatch.DrawString(basicfont, item.Value.ToString(), new Vector2(p.X + 55 + 42 * xpos, p.Y + 82 + 42 * ypos), Color.White);
                xpos++;
                if (xpos >= width)
                { xpos = 0; ypos++; }
            }

            DrawBar();

        }
        
        void DrawCombat()
        {
            Color c;
            if (currentCombat.CurrentTurn.UsedAction)
                c = new Color(100, 100, 100);
            else
                c = Color.White;
            gui.CombatElements[(int)UICombatElement.Action_Attack].color = c;
            gui.CombatElements[(int)UICombatElement.Action_Skill1].color = c;
            gui.CombatElements[(int)UICombatElement.Action_Skill2].color = c;

            c = Color.White;
            if (currentCombat.CurrentTurn.UsedMove) c = new Color(100, 100, 100);
            if (currentCombat.MoveMode == Listof_MoveMode.Waitingforcommand) c = new Color(100, 255, 100);            
            gui.CombatElements[(int)UICombatElement.Action_Move].color = c;

            c = Color.White;
            if (currentCombat.CurrentTurn.UsedAction) c = new Color(100, 100, 100);
            if (currentCombat.AttackMode == Listof_AttackMode.Waitingforcommand) c = new Color(255, 100, 100);
            gui.CombatElements[(int)UICombatElement.Action_Attack].color = c;




            foreach (var item in gui.CombatElements)
                if (item.Value.Enabled)
                    if (item.Value.Sprite != SD_UI.None)
                        spriteBatch.Draw(UI_Textures[(int)item.Value.Sprite], item.Value.Location, item.Value.color);


            
            spriteBatch.DrawString(basicfont, "Attack", gui.CombatElements[(int)UICombatElement.Action_Attack].Location.Location.ToVector2() + new Vector2(32, 32), Color.White);

            if (currentCombat.CurrentTurn.PrimaryClass.Class == Listof_Classes.Empty)
                spriteBatch.DrawString(basicfont, "None", gui.CombatElements[(int)UICombatElement.Action_Skill1].Location.Location.ToVector2() + new Vector2(32, 32), Color.Gray);
            else
                spriteBatch.DrawString(basicfont, catalog.classes.Data[currentCombat.CurrentTurn.PrimaryClass.Class].Skills, gui.CombatElements[(int)UICombatElement.Action_Skill1].Location.Location.ToVector2() + new Vector2(32, 32), Color.White);


            if (currentCombat.CurrentTurn.SecondaryClass.Class == Listof_Classes.Empty)
                spriteBatch.DrawString(basicfont, "None", gui.CombatElements[(int)UICombatElement.Action_Skill2].Location.Location.ToVector2() + new Vector2(32, 32), Color.Gray);
            else
                spriteBatch.DrawString(basicfont, catalog.classes.Data[currentCombat.CurrentTurn.SecondaryClass.Class].Skills, gui.CombatElements[(int)UICombatElement.Action_Skill2].Location.Location.ToVector2() + new Vector2(32, 32), Color.White);

            spriteBatch.DrawString(basicfont, "Move", gui.CombatElements[(int)UICombatElement.Action_Move].Location.Location.ToVector2() + new Vector2(32, 32), Color.White);

            spriteBatch.DrawString(basicfont, currentCombat.CurrentTurn.Stats.Movement.ToString() + "/" + currentCombat.CurrentTurn.Stats.MovementMax.ToString(), gui.CombatElements[(int)UICombatElement.Action_Move].Location.Location.ToVector2() + new Vector2(96, 32), Color.White);

            if (currentCombat.CurrentTurn != null)
            {
                spriteBatch.Draw(Actor_Sprites[0], new Rectangle(64, Screen_Size.Y - 128, 64, 64), new Rectangle(currentCombat.CurrentTurn.Sprite * 64, 0, 64, 64), Color.White);
                spriteBatch.DrawString(basicfont, currentCombat.CurrentTurn.Stats.Speed.ToString(), new Vector2(90, Screen_Size.Y - 150), Color.White);
                spriteBatch.DrawString(basicfont, currentCombat.CurrentTurn.Stats.HP.ToString() + "/" + currentCombat.CurrentTurn.Stats.HPMax.ToString(), new Vector2(90, Screen_Size.Y - 180), Color.White);
            }
            
            

            List<Actor_Type> inilist = currentCombat.GetCombatOrder(20);
            for (int x = 0; x < inilist.Count; x++)
            {
                spriteBatch.Draw(Actor_Sprites[0], new Rectangle((x + 2) * 64, Screen_Size.Y - 128, 64, 64), new Rectangle(inilist[x].Sprite * 64, 0, 64, 64), Color.White);
                spriteBatch.DrawString(basicfont, inilist[x].Stats.Speed.ToString(), new Vector2(26 + (x + 2) * 64, Screen_Size.Y - 150), Color.White);

                spriteBatch.DrawString(basicfont, inilist[x].Stats.HP.ToString() + "/" + inilist[x].Stats.HPMax.ToString(), new Vector2(26 + (x + 2) * 64, Screen_Size.Y - 180), Color.White);                
            }
            



        }


        void DrawInventoryEquipment()
        {
            for (int x = 0; x < 6; x++)
            {

                if (Player.Loadout.Equipment[x] != null)
                {
                    {
                        var item = Player.Loadout.Equipment[x];
                        //spriteBatch.Draw(UI_Textures[(int)SD_UI.Button], new Rectangle(p.X + 27 + 38 * xpos, p.Y + 66 + 36 * ypos, 32, 32), Color.White);
                        spriteBatch.Draw(UI_Textures[(int)SD_UI.Items], gui.InventoryElements[3000 + x].Location, new Rectangle((int)item.item * 32, 0, 32, 32), Color.White);
                        //spriteBatch.DrawString(basicfont, item.Amount.ToString(), new Vector2(p.X + 27 + 38 * xpos, p.Y + 66 + 36 * ypos), Color.White);                    
                    }
                }


            }            
        }







        void CreateCircle(int x0, int y0, int radius)
        {
            int x = radius - 1;
            int y = 0;
            int dx = 1;
            int dy = 1;
            int err = dx - (radius << 1);

            HashSet<Point> Tiles = new HashSet<Point>();


            while (x >= y)
            {
                Tiles.Add(new Point(x0 + x, y0 + y));
                Tiles.Add(new Point(x0 + y, y0 + x));

                Tiles.Add(new Point(x0 - y, y0 + x));
                Tiles.Add(new Point(x0 - x, y0 + y));

                Tiles.Add(new Point(x0 - x, y0 - y));
                Tiles.Add(new Point(x0 - y, y0 - x));

                Tiles.Add(new Point(x0 + y, y0 - x));
                Tiles.Add(new Point(x0 + x, y0 - y));

                if (err <= 0)
                {
                    y++;
                    err += dy;
                    dy += 2;
                }

                if (err > 0)
                {
                    x--;                    
                    dx += 2;
                    err += dx - (radius << 1);
                }

            }

            foreach (var item in Tiles)
            {
                if (!TileCircle.Contains(item))
                {
                    TileCircle.Add(item);

                }
            }            
            TileCircleSize = radius;
        }
    }
}
