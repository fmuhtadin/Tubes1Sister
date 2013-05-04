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
using System.Threading;

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
        public Texture2D healthtxr;

        //Network
        IPeer peer;
        IPAddress localIP;
        List<IPAddress> listTeam1;
        List<IPAddress> listTeam2;
        Dictionary<IPAddress, int> listPlayers;
        int currIterate;
        List<int> turn;

        // The rate of fire of the player laser
        TimeSpan fireTime;

        public GunbondGame(IPeer inpeer)
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 240;
            graphics.PreferredBackBufferWidth = 800;
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
            turn = new List<int>();
            if (localIP.Equals(listTeam1[0]))
            {
                RandomizeTurn();
                Thread.Sleep(3000);
                SendTurnOrder();
            }
            else
            {
                while (turn.Count == 0)
                {

                }
            }
            currIterate = 0;

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

        private void RandomizeTurn()
        {
            Random rand = new Random();
            int tmp;
            while (turn.Count < listPlayers.Count) {
                tmp = rand.Next(listPlayers.Count);
                if (!turn.Contains(tmp))
                {
                    turn.Add(tmp);
                }
            }
        }

        private void SendTurnOrder()
        {
            peer.SendTurnOrder(turn);
        }

        public void UpdateTurnOrder(List<int> turn) 
        {
            this.turn = turn;
        }

        private void AddPlayer(Texture2D R0, Texture2D R1, Texture2D R2, Texture2D R3, Texture2D L0, Texture2D L1, Texture2D L2, Texture2D L3, Vector2 position)
        {
            Player player = new Player();
            player.Initialize(R0, R1, R2, R3, L0, L1, L2, L3, position);
            player.HealthInit(healthtxr);
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

            healthtxr = Content.Load<Texture2D>("HealthBar");
            Vector2 playerPosition = new Vector2(30, GraphicsDevice.Viewport.Height - 60);

            for (int i = 0; i < listTeam1.Count; i++)
            {
                if (listTeam1[i].Equals(localIP))
                {
                    PlayerNo = Players.Count;
                }
                switch (i)
                {
                    case 0:
                        AddPlayer(Content.Load<Texture2D>("400"), Content.Load<Texture2D>("401"), Content.Load<Texture2D>("402"), Content.Load<Texture2D>("403"), Content.Load<Texture2D>("410"), Content.Load<Texture2D>("411"), Content.Load<Texture2D>("412"), Content.Load<Texture2D>("413"), playerPosition);
                        break;
                    case 1:
                        AddPlayer(Content.Load<Texture2D>("300"), Content.Load<Texture2D>("301"), Content.Load<Texture2D>("302"), Content.Load<Texture2D>("303"), Content.Load<Texture2D>("310"), Content.Load<Texture2D>("311"), Content.Load<Texture2D>("312"), Content.Load<Texture2D>("313"), playerPosition);
                        break;
                    case 2:
                        AddPlayer(Content.Load<Texture2D>("200"), Content.Load<Texture2D>("201"), Content.Load<Texture2D>("202"), Content.Load<Texture2D>("203"), Content.Load<Texture2D>("210"), Content.Load<Texture2D>("211"), Content.Load<Texture2D>("212"), Content.Load<Texture2D>("213"), playerPosition);
                        break;
                    case 3:
                        AddPlayer(Content.Load<Texture2D>("100"), Content.Load<Texture2D>("101"), Content.Load<Texture2D>("102"), Content.Load<Texture2D>("103"), Content.Load<Texture2D>("110"), Content.Load<Texture2D>("111"), Content.Load<Texture2D>("112"), Content.Load<Texture2D>("113"), playerPosition);
                        break;
                }
            }
            for (int i = 0; i < listTeam2.Count; i++)
            {
                if (listTeam2[i].Equals(localIP))
                {
                    PlayerNo = Players.Count;
                }
                switch (i)
                {
                    case 0:
                        AddPlayer(Content.Load<Texture2D>("400"), Content.Load<Texture2D>("401"), Content.Load<Texture2D>("402"), Content.Load<Texture2D>("403"), Content.Load<Texture2D>("410"), Content.Load<Texture2D>("411"), Content.Load<Texture2D>("412"), Content.Load<Texture2D>("413"), playerPosition);
                        break;
                    case 1:
                        AddPlayer(Content.Load<Texture2D>("300"), Content.Load<Texture2D>("301"), Content.Load<Texture2D>("302"), Content.Load<Texture2D>("303"), Content.Load<Texture2D>("310"), Content.Load<Texture2D>("311"), Content.Load<Texture2D>("312"), Content.Load<Texture2D>("313"), playerPosition);
                        break;
                    case 2:
                        AddPlayer(Content.Load<Texture2D>("200"), Content.Load<Texture2D>("201"), Content.Load<Texture2D>("202"), Content.Load<Texture2D>("203"), Content.Load<Texture2D>("210"), Content.Load<Texture2D>("211"), Content.Load<Texture2D>("212"), Content.Load<Texture2D>("213"), playerPosition);
                        break;
                    case 3:
                        AddPlayer(Content.Load<Texture2D>("100"), Content.Load<Texture2D>("101"), Content.Load<Texture2D>("102"), Content.Load<Texture2D>("103"), Content.Load<Texture2D>("110"), Content.Load<Texture2D>("111"), Content.Load<Texture2D>("112"), Content.Load<Texture2D>("113"), playerPosition);
                        break;
                }
            }

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

        public void UpdateDeadPeer(IPAddress ip)
        {
            for (int i = 0; i < listTeam1.Count; i++)
            {
                if (listTeam1[i].Equals(ip)) 
                {
                    listTeam1.RemoveAt(i);
                }
            }
            for (int i = 0; i < listTeam2.Count; i++)
            {
                if (listTeam2[i].Equals(ip))
                {
                    listTeam2.RemoveAt(i);
                }
            }
            int index = listPlayers[ip];
            foreach (KeyValuePair<IPAddress, int> tuple in listPlayers)
            {
                if (tuple.Value == index)
                {
                    listPlayers.Remove(tuple.Key);
                }
                else if (tuple.Value > index)
                {
                    listPlayers.Remove(tuple.Key);
                    listPlayers.Add(tuple.Key, tuple.Value - 1);
                }
            }
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
            try
            {
                if (turn[currIterate % listPlayers.Count] == PlayerNo)
                {
                    // Space
                    if ((currentKeyboardState.IsKeyDown(Keys.Space)) && (previousKeyboardState.IsKeyUp(Keys.Space)))
                    {
                        AddProjectile();
                        peer.SendPosition(7);
                        currIterate++;
                    }

                    // Use the Keyboard / Dpad
                    if (currentKeyboardState.IsKeyDown(Keys.Left))
                    {
                        if (horizontal == 0) horizontal = 1;
                        Move(Players[PlayerNo], 1);
                        peer.SendPosition(-1);
                    }
                    if (currentKeyboardState.IsKeyDown(Keys.Right))
                    {
                        if (horizontal == 1) horizontal = 0;
                        Move(Players[PlayerNo], 0);
                        peer.SendPosition(1);
                    }
                    if (currentKeyboardState.IsKeyDown(Keys.Up) && previousKeyboardState.IsKeyUp(Keys.Up))
                    {
                        if (vertikal < 3) vertikal++;
                        peer.SendPosition(3);
                    }
                    if (currentKeyboardState.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down))
                    {
                        if (vertikal > 0) vertikal--;
                        peer.SendPosition(4);
                    }
                    // Make sure that the player does not go out of bounds
                    Players[PlayerNo].Position.X = MathHelper.Clamp(Players[PlayerNo].Position.X, 0, GraphicsDevice.Viewport.Width + -Players[PlayerNo].Width);
                    Players[PlayerNo].Position.Y = MathHelper.Clamp(Players[PlayerNo].Position.Y, 0, GraphicsDevice.Viewport.Height + -Players[PlayerNo].Height);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        private void HandleOtherPlayerMovement(int playerNo, int state)
        {
            // Space
            if (state == 7)
            {
                AddProjectile();
                currIterate++;
            }

            // Use the Keyboard / Dpad
            if (state == -1)
            {
                if (horizontal == 0) horizontal = 1;
                Move(Players[playerNo], 1);
            }
            if (state == 1)
            {
                if (horizontal == 1) horizontal = 0;
                Move(Players[playerNo], 0);
            }
            if (state == 3)
            {
                if (vertikal < 3) vertikal++;
            }
            if (state == 4)
            {
                if (vertikal > 0) vertikal--;
            }
            // Make sure that the player does not go out of bounds
            Players[playerNo].Position.X = MathHelper.Clamp(Players[playerNo].Position.X, 0, GraphicsDevice.Viewport.Width + -Players[playerNo].Width);
            Players[playerNo].Position.Y = MathHelper.Clamp(Players[playerNo].Position.Y, 0, GraphicsDevice.Viewport.Height + -Players[playerNo].Height);
        }

        private void UpdateProjectiles()
        {
            // Update the Projectiles
            int i = projectiles.Count - 1;
            while (i >= 0)
            {
                projectiles[i].Update();

                if (projectiles[i].Active == false)
                {
                    projectiles.RemoveAt(i);
                }
                else
                {
                    for (int j = 0; j < Players.Count; j++)
                    {
                        if (j != projectiles[i].Owner)
                        {
                            if (Tabrakan(Players[j], projectiles[i]))
                            {
                                projectiles[i].Active = false;
                                Players[j].GetDamage();
                                if (Players[j].Health <= 0)
                                {
                                    Players.RemoveAt(j);
                                }
                            }
                        }
                    }
                }
                i--;
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
                Players[i].DrawHealth(spriteBatch);
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

        private Boolean Tabrakan(Player P1, Projectile P2)
        {
            Rectangle R1 = new Rectangle((int)P1.Position.X, (int)P1.Position.Y, P1.Width, P1.Height);
            Rectangle R2 = new Rectangle((int)P2.Position.X, (int)P2.Position.Y, P2.Width, P2.Height);
            return (R1.Intersects(R2));
        }

        private void AddProjectile()
        {
            if (PlayerNo < Players.Count)
            {
                Player P = Players[turn[currIterate % listPlayers.Count]];
                int horizontal = P.horizontal;
                int vertikal = P.vertikal;
                Vector2 position = new Vector2();
                if (horizontal == 0) //kanan
                {
                    switch (vertikal)
                    {
                        case 0: // bawah 2
                            position = P.Position + new Vector2(60, 50);
                            break;
                        case 1: // bawah
                            position = P.Position + new Vector2(55, 40);
                            break;
                        case 2: // atas
                            position = P.Position + new Vector2(50, 30);
                            break;
                        default: // atas 2
                            position = P.Position + new Vector2(48, 20);
                            break;
                    }
                }
                else
                {
                    switch (vertikal)
                    {
                        case 0: // bawah 2
                            position = P.Position + new Vector2(5, 50);
                            break;
                        case 1: // bawah
                            position = P.Position + new Vector2(10, 40);
                            break;
                        case 2: // atas
                            position = P.Position + new Vector2(15, 30);
                            break;
                        default: // atas 2
                            position = P.Position + new Vector2(17, 20);
                            break;
                    }
                }

                Projectile projectile = new Projectile();
                projectile.Initialize(GraphicsDevice.Viewport, projectileTexture, position, horizontal, vertikal, PlayerNo);
                projectiles.Add(projectile);
            }
        }
    }
}
