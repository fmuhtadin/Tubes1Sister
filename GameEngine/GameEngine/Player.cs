#region File Description
//-----------------------------------------------------------------------------
// Player.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using GameEngine;

namespace GameEngine
{
    /// <summary>
    /// Our fearless adventurer!
    /// </summary>
    class Player
    {
        public int horizontal = 0; // 0 kanan, 1 kiri
        public int vertikal = 0; // 0 bawah2, 1 bawah, 2 atas, 3 atas2

        // Animation representing the player
        public Texture2D kanan0;
        public Texture2D kanan1;
        public Texture2D kanan2;
        public Texture2D kanan3;
        public Texture2D kiri0;
        public Texture2D kiri1;
        public Texture2D kiri2;
        public Texture2D kiri3;

        // Health Texture
        public Texture2D mSpriteTexture;


        // Position of the Player relative to the upper left side of the screen
        public Vector2 Position;

        // State of the player
        public bool Active;

        // Amount of hit points that player has
        public int Health;

        //Posisi draw health
        public int HealthPos;

        // Get the width of the player ship
        public int Width
        {
            get { return kanan0.Width; }
        }

        // Get the height of the player ship
        public int Height
        {
            get { return kanan0.Height; }
        }

        public void Initialize(Texture2D R0, Texture2D R1, Texture2D R2, Texture2D R3, Texture2D L0, Texture2D L1, Texture2D L2, Texture2D L3, Vector2 position)
        {
            kanan0 = R0;
            kanan1 = R1;
            kanan2 = R2;
            kanan3 = R3;
            kiri0 = L0;
            kiri1 = L1;
            kiri2 = L2;
            kiri3 = L3;


            // Set the starting position of the player around the middle of the screen and to the back
            Position = position;

            // Set the player to be active
            Active = true;

            // Set the player health
            Health = 100;
            horizontal = 0;
            vertikal = 0;
        }

        // Update the player animation
        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (horizontal == 0)//kanan
            {
                switch (vertikal)
                {
                    case 0: // bawah 2
                        spriteBatch.Draw(kanan0, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        break;
                    case 1: // bawah
                        spriteBatch.Draw(kanan1, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        break;
                    case 2: // atas
                        spriteBatch.Draw(kanan2, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        break;
                    case 3: // atas 2
                        spriteBatch.Draw(kanan3, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        break;
                }
            }
            else
            {
                switch (vertikal)
                {
                    case 0: // bawah 2
                        spriteBatch.Draw(kiri0, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        break;
                    case 1: // bawah
                        spriteBatch.Draw(kiri1, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        break;
                    case 2: // atas
                        spriteBatch.Draw(kiri2, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        break;
                    case 3: // atas 2
                        spriteBatch.Draw(kiri3, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        break;
                }
            }

        }

        public void DrawHealth(SpriteBatch spriteBatch)
        {
            UpdatePos();

            spriteBatch.Draw(mSpriteTexture, new Rectangle(HealthPos - (int)(mSpriteTexture.Width * 0.1) / 2, 160, (int)(mSpriteTexture.Width * 0.1), 14), new Rectangle(0, 45, (int)(mSpriteTexture.Width * 0.1), 14), Color.Gray);

            //Draw the current health level based on the current Health
            spriteBatch.Draw(mSpriteTexture, new Rectangle(HealthPos - (int)(mSpriteTexture.Width * 0.1) / 2, 160, (int)((int)(mSpriteTexture.Width * 0.1) * ((double)Health / 100)), 14), new Rectangle(0, 45, (int)(mSpriteTexture.Width * 0.1), 14), Color.Red);

        }

        public void HealthInit(Texture2D healthtexture)
        {
            mSpriteTexture = healthtexture;
        }

        public void UpdatePos()
        {
            HealthPos = (int)Position.X + 35;
        }

        public void GetDamage()
        {
            Health = Health - 10;
        }
    }
}
