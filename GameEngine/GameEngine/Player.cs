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
        public Texture2D sprite;
        public Rectangle destinationRect;
        public Rectangle sourceRect;

        // Position of the Player relative to the upper left side of the screen
        public Vector2 Position;

        // State of the player
        public bool Active;

        // Amount of hit points that player has
        public int Health;

        //Posisi draw health
        public int HealthPos;

        // Get the width of the player ship
        public int Width = 65;

        // Get the height of the player ship
        public int Height = 60;

        // Health Texture
        public Texture2D mSpriteTexture;

        // Team
        public int Team;

        public void Initialize(Texture2D S, Vector2 position, int Team)
        {
            sprite = S;
            this.Team = Team;

            // Set the starting position of the player around the middle of the screen and to the back
            Position = position;

            // Set the player to be active
            Active = true;

            // Set the player health
            Health = 100;
            horizontal = 0;
            vertikal = 0;
            sourceRect = new Rectangle(0, 0, Width, Height);
            destinationRect = new Rectangle((int)Position.X - (int)(Width),
            (int)Position.Y - (int)(Height),
            Width, Height);
        }

        public void GetDamage()
        {
            Health -= 10;
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

        // Update the player animation
        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sourceRect = new Rectangle(((4 * horizontal) + vertikal) * Width, 0, Width, Height);
            destinationRect = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            spriteBatch.Draw(sprite, destinationRect, sourceRect, Color.White);
        }
    }
}
