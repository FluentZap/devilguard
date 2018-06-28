using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;

namespace Devilguard
{

    /// <summary>
    /// Tile Type
    /// </summary>
    class Tile_Type
    {
        public int ID;
        public int Sprite;
        public bool Walkalble;
        public int Item;
        public int Durability;
    }   

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 

    public class Game1 : Game
    {
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
        Point Screen_Size;
        Point MouseDragStart;
        bool MouseDraging;
        private Camera2D _camera;
        bool InventoryOpen;
        bool InventoryKey;

        CraftingDictionary craftingDictionary = new CraftingDictionary();
        ResourceDictionary resourceDictionary = new ResourceDictionary();

        Actor_Type Player = new Actor_Type();        
        enum Direction { LEFT, RIGHT, UP, DOWN };
        


        void Move_Actor(Actor_Type A, Point2 Vector)
        {
            float X, Y;
            Vector.X *= A.Speed;
            Vector.Y *= A.Speed;

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
            Box.Offset(A.Location.ToVector2());
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
                    tilemap[x, y] = new Tile_Type();
                    tilemap[x, y].Durability = 5;
                    if (R.Next(0, 20) == 0)
                        tilemap[x, y].Item = 1;
                }
                    

            for (int x = 10; x < 60; x++)
                for (int y = 10; y < 60; y++)
                {
                    tilemap[x, y].Sprite = 1;
                    tilemap[x, y].Item = 0;
                }
                    

            Screen_Zoom = 2.0f;
            Player.Hitbox = new Rectangle(13, 9, 6, 23);
            //Player.inventory.AddItem(Item.Wood, 30);
            //Player.inventory.AddItem(Item.Stone, 30);
            //Player.inventory.AddItem(Item.Iron, 30);


            for (int x = 0; x < Enum.GetNames(typeof(InventoryItems)).Length; x++)
                Player.CraftingBlueprints.AddBlueprint((InventoryItems)x);
            


            base.Initialize();
            Window.IsBorderless = true;
            this.IsMouseVisible = true;
            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, Screen_Size.X, Screen_Size.Y);            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.            
            Background_Tiles[0] = Content.Load<Texture2D>("BackTileMap");
            Actor_Sprites[0] = Content.Load<Texture2D>("King_0");
            Item_Tiles[0] = Content.Load<Texture2D>("Tree");

            UI_Textures[(int)SD_UI.Back] = Content.Load<Texture2D>("UI_Back");
            UI_Textures[(int)SD_UI.Button] = Content.Load<Texture2D>("UI_Button");
            UI_Textures[(int)SD_UI.Resources] = Content.Load<Texture2D>("UI_Resources");
            UI_Textures[(int)SD_UI.Items] = Content.Load<Texture2D>("UI_Items");

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
            float speed = 0.5f * gameTime.ElapsedGameTime.Milliseconds;


            if (Keyboard.GetState().IsKeyDown(Keys.W))
                Player.Location.Y -= 1;
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                Player.Location.Y += 1;
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                Player.Location.X -= 1;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                Player.Location.X += 1;

            /*
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                Player.Location += 5;
                Screen_Scroll.Y -= 10;
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                Screen_Scroll.Y += 10;
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                Screen_Scroll.X -= 10;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                Screen_Scroll.X += 10;
            */

            //if (Keyboard.GetState().IsKeyDown(Keys.Q))
            //Screen_Zoom -= 0.1f;

            //if (Keyboard.GetState().IsKeyDown(Keys.E))           
            //    Screen_Zoom += 0.1f;                

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

                


            // TODO: Add your update logic here

            if (Screen_Zoom < 1.0f) Screen_Zoom = 1.0f;
            if (Screen_Zoom > 4.0f) Screen_Zoom = 4.0f;

            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                if (!MouseDraging)
                {
                    MouseDraging = true;
                    MouseDragStart = Screen_Scroll + Mouse.GetState().Position;
                }           
            }
            if (MouseDraging)
            {
                Screen_Scroll = (MouseDragStart - Mouse.GetState().Position);
                if (Mouse.GetState().RightButton != ButtonState.Pressed) MouseDraging = false;
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Point sel_pos = (Mouse.GetState().Position + Screen_Scroll) / new Point(GetATSI(), GetATSI());
                Tile_Type t = tilemap[sel_pos.X, sel_pos.Y];
                if (sel_pos.X >= 0 && sel_pos.X <= 1000 && sel_pos.Y >= 0 && sel_pos.Y <= 1000)
                    if (Player.UsedItem == false)
                    {
                        if (t.Item == 1)
                        {
                            Player.inventory.AddResource(ResourceItem.Wood, 5);
                            t.Item = 0;
                        }

                        if (t.Sprite == 1)
                        {
                            t.Durability--;
                            if (t.Durability <= 0)
                            {
                                Player.inventory.AddResource(ResourceItem.Stone, 5);
                                t.Durability = 5;
                                t.Sprite = 2;
                            }                            
                        }
                        Player.UsedItem = true;
                    }
            }


            if (Mouse.GetState().LeftButton == ButtonState.Released)
                Player.UsedItem = false;

            if (Screen_Scroll.X < 0) Screen_Scroll.X = 0;
            if (Screen_Scroll.X > GetATSI() * 1000 + Screen_Size.X) Screen_Scroll.X = GetATSI() * 1000 + Screen_Size.X;

            if (Screen_Scroll.Y < 0) Screen_Scroll.Y = 0;
            if (Screen_Scroll.Y > GetATSI() * 1000 + Screen_Size.Y) Screen_Scroll.Y = GetATSI() * 1000 + Screen_Size.Y;
            //if (Screen_Scroll.Y * 1000 > Screen_Size.X) Screen_Scroll.Y = Screen_Size.Y;

            base.Update(gameTime);
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
                            byte dur = (byte)(tilemap[x, y].Durability * 51);
                        
                            spriteBatch.Draw(Background_Tiles[0], new Rectangle(pos.X, pos.Y, GetATSI(), GetATSI()), new Rectangle(tilemap[x, y].Sprite * 32, 0, 32, 32), new Color(dur, dur, dur));
                        //spriteBatch.Draw(Background_Tiles[0], new Rectangle(pos.X, pos.Y, GetATSI(), GetATSI()), new Color(dur, dur, dur));
                        //}

                        if (tilemap[x, y].Item == 1)
                            spriteBatch.Draw(Item_Tiles[0], new Rectangle(pos.X, pos.Y - GetATSI() * 2, GetATSI(), GetATSI() * 3), Color.White);
                    }
                    
                    //if (tilemap[x, y] != 0)
                    //spriteBatch.Draw(Background_Tiles[tilemap[x, y]], new Rectangle(pos.X, pos.Y, 64, 64), Color.White);
                }

            spriteBatch.Draw(Actor_Sprites[0], new Rectangle((int)(Player.Location.X * Screen_Zoom - Screen_Scroll.X), (int)(Player.Location.Y * Screen_Zoom - Screen_Scroll.Y), GetATSI(), GetATSI()), Color.White);

            if (InventoryOpen)
                DrawInventory();

            spriteBatch.End();
            base.Draw(gameTime);
        }





        void DrawInventory()
        {
            Point p = new Point(Screen_Size.X / 2 - 392, Screen_Size.Y / 2 - 196);
            spriteBatch.Draw(UI_Textures[0], new Rectangle(p.X, p.Y, 392, 392), Color.White);
            spriteBatch.Draw(UI_Textures[0], new Rectangle(p.X + 392, p.Y, 392, 392), Color.White);

            int width = 6;
            int xpos = 0;
            int ypos = 0;
            
            foreach (var item in Player.inventory.Resources)
            {                
                spriteBatch.Draw(UI_Textures[(int)SD_UI.Button], new Rectangle(p.X + 50 + 42 * xpos, p.Y + 50 + 42 * ypos, 32, 32), Color.White);
                spriteBatch.Draw(UI_Textures[(int)SD_UI.Resources], new Rectangle(p.X + 50 + 42 * xpos, p.Y + 50 + 42 * ypos, 32, 32), new Rectangle((int)item.Key * 16, 0, 16, 16), Color.White);

                spriteBatch.DrawString(basicfont, item.Value.ToString(), new Vector2(p.X + 55 + 42 * xpos, p.Y + 82 + 42 * ypos), Color.White);                

                spriteBatch.DrawString(basicfont, (resourceDictionary.Data[item.Key].Value * item.Value).ToString() + "gp", new Vector2(p.X + 55 + 42 * xpos, p.Y + 114 + 42 * ypos), Color.White);

                spriteBatch.DrawString(basicfont, (resourceDictionary.Data[item.Key].Weight * item.Value).ToString() + "wt", new Vector2(p.X + 55 + 42 * xpos, p.Y + 146 + 42 * ypos), Color.White);

                xpos++;
                if (xpos >= width)
                { xpos = 0; ypos++; }                
            }

            width = 6;
            xpos = 0;
            ypos = 0;                        

            
            //Draw Crafting Area
            foreach (var item in Player.CraftingBlueprints.Blueprints)
            {                
                bool CanCaft = true;
                foreach (var craftitem in craftingDictionary.Data[item].ResourceCost)
                    if (Player.inventory.ResourceCount(craftitem.Key) < craftitem.Value)
                    { CanCaft = false; break;}

                if (CanCaft)
                {
                    spriteBatch.Draw(UI_Textures[(int)SD_UI.Button], new Rectangle(p.X + 392 + 50 + 42 * xpos, p.Y + 50 + 42 * ypos, 32, 32), Color.White);
                    spriteBatch.Draw(UI_Textures[(int)SD_UI.Items], new Rectangle(p.X + 392 + 50 + 42 * xpos, p.Y + 50 + 42 * ypos, 32, 32), new Rectangle((int)item * 16, 0, 16, 16), Color.White);
                }
                else
                {
                    spriteBatch.Draw(UI_Textures[(int)SD_UI.Button], new Rectangle(p.X + 392 + 50 + 42 * xpos, p.Y + 50 + 42 * ypos, 32, 32), Color.Gray);
                    spriteBatch.Draw(UI_Textures[(int)SD_UI.Items], new Rectangle(p.X + 392 + 50 + 42 * xpos, p.Y + 50 + 42 * ypos, 32, 32), new Rectangle((int)item * 16, 0, 16, 16), Color.Gray);
                }
                
                //spriteBatch.DrawString(basicfont, item.Value.ToString(), new Vector2(p.X + 55 + 42 * xpos, p.Y + 82 + 42 * ypos), Color.White);
                xpos++;
                if (xpos >= width)
                { xpos = 0; ypos++; }
            }            


        }
        


    }
}
