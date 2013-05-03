using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using Microsoft.Xna.Framework.Input.Touch;
using GunbondLibrary;

namespace GameEngine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GunbondGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        // Represents the player
        List<Player> Players;
        // Keyboard states used to determine key presses
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        

        // Gamepad states used to determine button presses
        GamePadState currentGamePadState;
        GamePadState previousGamePadState;

        // A movement speed for the player
        float playerMoveSpeed;

        // Image used to display the static background
        Texture2D mainBackground;

        // Parallaxing Layers
        ParallaxingBackground bgLayer1;
        ParallaxingBackground bgLayer2;

        // Gambar Untuk Player
        public Texture2D kiribawah;
        public Texture2D kiriatas;
        public Texture2D kananbawah;
        public Texture2D kananatas;
        public Texture2D kiribawah2;
        public Texture2D kiriatas2;
        public Texture2D kananbawah2;
        public Texture2D kananatas2;
        private int horizontal = 0; // 0 kanan, 1 kiri
        private int vertikal = 0; // 0 bawah2, 1 bawah, 2 atas, 3 atas2
        private int PlayerNo;

        Texture2D projectileTexture;
        List<Projectile> projectiles;

        //Network
        IPeer peer;
        IPAddress localIP;
        List<IPAddress> listTeam1;
        List<IPAddress> listTeam2;
        Dictionary<IPAddress, int> listPlayers;

        // The rate of fire of the player laser
        TimeSpan fireTime;
        TimeSpan previousFireTime;

        public GunbondGame(IPeer inpeer)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            peer = inpeer;
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
            // Initialize the player class
            Players = new List<Player>();
            listPlayers = new Dictionary<IPAddress, int>();
            localIP = peer.GetSelfIP();
            listTeam1 = peer.GetListTeam1();
            listTeam2 = peer.GetListTeam2();
            int count = 0;
            for (int i = 0; i < listTeam1.Count; i++)
            {
                listPlayers.Add(listTeam1[i], count);
                count++;
            }
            for (int i = 0; i < listTeam2.Count; i++)
            {
                listPlayers.Add(listTeam2[i], count);
                count++;
            }

            // Set a constant player move speed
            playerMoveSpeed = 8.0f;

            //Enable the FreeDrag gesture.
            TouchPanel.EnabledGestures = GestureType.FreeDrag;

            bgLayer1 = new ParallaxingBackground();
            bgLayer2 = new ParallaxingBackground();

            projectiles = new List<Projectile>();

            // Set the laser to fire every quarter second
            fireTime = TimeSpan.FromSeconds(.15f);

            base.Initialize();
        }

        private void AddPlayer(Texture2D image,Vector2 position){
            Player player = new Player();
            player.Initialize(image, position);
            Players.Add(player);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            // Load the player resources 
            Vector2 playerPosition = new Vector2(130, GraphicsDevice.Viewport.Height - 119);
            kiriatas = Content.Load<Texture2D>("kiriatas");
            kiribawah = Content.Load<Texture2D>("kiribawah");
            kananatas = Content.Load<Texture2D>("kananatas");
            kananbawah = Content.Load<Texture2D>("kananbawah");
            kiriatas2 = Content.Load<Texture2D>("kiriatas2");
            kiribawah2 = Content.Load<Texture2D>("kiribawah2");
            kananatas2 = Content.Load<Texture2D>("kananatas2");
            kananbawah2 = Content.Load<Texture2D>("kananbawah2");

            for (int i = 0; i < listTeam1.Count; i++)
            {
                if (listTeam1[i].Equals(localIP))
                {
                    AddPlayer(kananbawah, new Vector2(10f + 50*i, playerPosition.Y));
                    PlayerNo = Players.Count - 1;
                }
                else
                {
                    AddPlayer(Content.Load<Texture2D>("enemy"), new Vector2(10f + 50 * i, playerPosition.Y));
                }
            }
            for (int i = 0; i < listTeam2.Count; i++)
            {
                if (listTeam2[i].Equals(localIP))
                {
                    AddPlayer(kananbawah, new Vector2(GraphicsDevice.Viewport.Width - 10f + 50 * i, playerPosition.Y));
                    PlayerNo = Players.Count - 1;
                }
                else
                {
                    AddPlayer(Content.Load<Texture2D>("enemy"), new Vector2(GraphicsDevice.Viewport.Width -  10f + 50 * i, playerPosition.Y));
                }
            }
            //AddPlayer(kananbawah, playerPosition);

            //AddPlayer(Content.Load<Texture2D>("enemy"), new Vector2(10f , GraphicsDevice.Viewport.Height - Players[0].Height));
            
            // Load the parallaxing background
            bgLayer1.Initialize(Content, "bgLayer1", GraphicsDevice.Viewport.Width, -1);
            bgLayer2.Initialize(Content, "bgLayer2", GraphicsDevice.Viewport.Width, -2);

            projectileTexture = Content.Load<Texture2D>("laser");

            mainBackground = Content.Load<Texture2D>("mainbackground");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            // Save the previous state of the keyboard and game pad so we can determinesingle key/button presses
            previousGamePadState = currentGamePadState;
            previousKeyboardState = currentKeyboardState;

            // Read the current state of the keyboard and gamepad and store it
            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);


            //Update the player
            UpdatePlayer(gameTime);

            // Update the parallaxing background
            bgLayer1.Update();
            bgLayer2.Update();

            // Update the projectiles
            UpdateProjectiles();

            base.Update(gameTime);
        }

        private void NewSprite(int Player,int horizontal,int vertical)
        {
            if (horizontal == 0) // Kanan
            {
                switch (vertikal)
                {
                    case 0: // bawah 2
                        Players[Player].PlayerTexture = kananbawah2;
                        break;
                    case 1: // bawah
                        Players[Player].PlayerTexture = kananbawah;
                        break;
                    case 2: // atas
                        Players[Player].PlayerTexture = kananatas;
                        break;
                    default: // atas 2
                        Players[Player].PlayerTexture = kananatas2;
                        break;
                }
            }
            else
            {
                switch (vertikal)
                {
                    case 0: // bawah 2
                        Players[Player].PlayerTexture = kiribawah2;
                        break;
                    case 1: // bawah
                        Players[Player].PlayerTexture = kiribawah;
                        break;
                    case 2: // atas
                        Players[Player].PlayerTexture = kiriatas;
                        break;
                    default: // atas 2
                        Players[Player].PlayerTexture = kiriatas2;
                        break;
                }
            }
        }

        private void Move(Player player, int direction /* 0 kanan, 1 kiri */)
        {
            if (direction == 0) // kanan
            {
                player.Position.X += playerMoveSpeed;
            }
            else // kiri
            {
                player.Position.X -= playerMoveSpeed;
            }
        }

        public void UpdateOtherPlayer(IPAddress otherIP, int state)
        {
            HandleOtherPlayerMovement(listPlayers[otherIP], state);
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            for (int i = 0; i < Players.Count; i++)
            {
                Players[i].Update(gameTime);
            }
            HandlePlayerMovement();
        }

        private void HandlePlayerMovement()
        {
            // Space
            if ((currentKeyboardState.IsKeyDown(Keys.Space)) && (previousKeyboardState.IsKeyUp(Keys.Space)))
            {
                AddProjectile(Players[PlayerNo], horizontal, vertikal);
                peer.SendPosition(0);
            }

            // Use the Keyboard / Dpad
            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                if (horizontal == 0) horizontal = 1;
                Move(Players[PlayerNo], 1);
                NewSprite(PlayerNo, horizontal, vertikal);
                peer.SendPosition(-1);
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                if (horizontal == 1) horizontal = 0;
                Move(Players[PlayerNo], 0);
                NewSprite(PlayerNo, horizontal, vertikal);
                peer.SendPosition(1);
            }
            if (currentKeyboardState.IsKeyDown(Keys.Up) && previousKeyboardState.IsKeyUp(Keys.Up))
            {
                if (vertikal < 3) vertikal++;
                NewSprite(PlayerNo, horizontal, vertikal);
                peer.SendPosition(3);
            }
            if (currentKeyboardState.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down))
            {
                if (vertikal > 0) vertikal--;
                NewSprite(PlayerNo, horizontal, vertikal);
                peer.SendPosition(4);
            }
            // Make sure that the player does not go out of bounds
            Players[PlayerNo].Position.X = MathHelper.Clamp(Players[PlayerNo].Position.X, 0, GraphicsDevice.Viewport.Width + -Players[PlayerNo].Width);
            Players[PlayerNo].Position.Y = MathHelper.Clamp(Players[PlayerNo].Position.Y, 0, GraphicsDevice.Viewport.Height + -Players[PlayerNo].Height);
        }

        private void HandleOtherPlayerMovement(int playerNo, int state)
        {
            // Space
            if (state == 0)
            {
                AddProjectile(Players[playerNo], horizontal, vertikal);
            }

            // Use the Keyboard / Dpad
            if (state == -1)
            {
                if (horizontal == 0) horizontal = 1;
                Move(Players[playerNo], 1);
                NewSprite(playerNo, horizontal, vertikal);
            }
            if (state == 1)
            {
                if (horizontal == 1) horizontal = 0;
                Move(Players[playerNo], 0);
                NewSprite(playerNo, horizontal, vertikal);
            }
            if (state == 3)
            {
                if (vertikal < 3) vertikal++;
                NewSprite(playerNo, horizontal, vertikal);
            }
            if (state == 4)
            {
                if (vertikal > 0) vertikal--;
                NewSprite(playerNo, horizontal, vertikal);
            }
            // Make sure that the player does not go out of bounds
            Players[playerNo].Position.X = MathHelper.Clamp(Players[playerNo].Position.X, 0, GraphicsDevice.Viewport.Width + -Players[playerNo].Width);
            Players[playerNo].Position.Y = MathHelper.Clamp(Players[playerNo].Position.Y, 0, GraphicsDevice.Viewport.Height + -Players[playerNo].Height);
        }

        private void UpdateProjectiles()
        {
            // Update the Projectiles
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                projectiles[i].Update();

                if (projectiles[i].Active == false)
                {
                    projectiles.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            // Start drawing
            spriteBatch.Begin();

            spriteBatch.Draw(mainBackground, Vector2.Zero, Color.White);

            // Draw the moving background
            bgLayer1.Draw(spriteBatch);
            bgLayer2.Draw(spriteBatch);

            // Draw the Player
            for (int i = 0; i < Players.Count; i++)
            {
                Players[i].Draw(spriteBatch);
            }

            // Draw the Projectiles
            for (int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Draw(spriteBatch);
            }

            // Stop drawing
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void AddProjectile(Player P,int horizontal, int vertikal)
        {
            Vector2 position = P.Position + new Vector2(P.Width,0);
            Projectile projectile = new Projectile();
            projectile.Initialize(GraphicsDevice.Viewport, projectileTexture, position);
            projectiles.Add(projectile);
        }
    }
}
