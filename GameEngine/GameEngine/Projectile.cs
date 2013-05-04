using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameEngine
{
    class Projectile
    {
        // Image representing the Projectile
        public Texture2D Texture;

        // Position of the Projectile relative to the upper left side of the screen
        public Vector2 Position;

        // State of the Projectile
        public bool Active;

        // The amount of damage the projectile can inflict to an enemy
        public int Damage;

        // Pemilik
        public int Owner;

        // Represents the viewable boundary of the game
        Viewport viewport;

        // Get the width of the projectile ship
        public int Width
        {
            get { return Texture.Width; }
        }

        // Get the height of the projectile ship
        public int Height
        {
            get { return Texture.Height; }
        }

        // Determines how fast the projectile moves
        float projectileMoveSpeed;
        float projectileMoveSpeedY;
        float grafitasi;
        float percepatangrafitasi;

        public void Initialize(Viewport viewport, Texture2D texture, Vector2 position, int horizontal, int vertikal, int Own)
        {
            Texture = texture;
            Position = position;
            this.viewport = viewport;

            Active = true;

            Damage = 2;

            Owner = Own;

            if (horizontal == 0)//kanan
            {
                projectileMoveSpeed = 10f;
            }
            else // kiri
            {
                projectileMoveSpeed = -10f;
            }

            switch (vertikal)
            {
                case 0: // bawah 2
                    projectileMoveSpeedY = 4f;
                    percepatangrafitasi = 1f;
                    break;
                case 1: // bawah
                    projectileMoveSpeedY = 8f;
                    percepatangrafitasi = 0.55f;
                    break;
                case 2: // atas
                    projectileMoveSpeedY = 8f;
                    percepatangrafitasi = 0.34f;
                    break;
                default: // atas 2
                    projectileMoveSpeedY = 10f;
                    percepatangrafitasi = 0.29f;
                    break;
            }
            grafitasi = 0;
        }
        public void Update()
        {
            grafitasi += percepatangrafitasi;
            Position.X += projectileMoveSpeed;
            Position.Y -= projectileMoveSpeedY - grafitasi;

            // Deactivate the bullet if it goes out of screen
            if (Position.X + Texture.Width / 2 > viewport.Width)
                Active = false;
            else if (Position.Y + Texture.Height / 2 > viewport.Height)
                Active = false;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f,
            new Vector2(Width / 2, Height / 2), 1f, SpriteEffects.None, 0f);
        }
    }
}
