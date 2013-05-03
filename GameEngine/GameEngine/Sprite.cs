using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    class Sprite
    {
        //The current position of the Sprite
        public Vector2 Position = new Vector2(0, 0);

        //The texture object used when drawing the sprite
        public Texture2D mSpriteTexture;

        //The asset name for the Sprite's Texture
        public string AssetName;

        //The Size of the Sprite (with scale applied)
        public Rectangle Size;

        //The amount to increase/decrease the size of the original sprite. 
        private float mScale = 1.0f;

        //Load the texture for the sprite using the Content Pipeline
        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            mSpriteTexture = theContentManager.Load<Texture2D>(theAssetName);
            AssetName = theAssetName;
            Size = new Rectangle(0, 0, (int)(mSpriteTexture.Width * Scale), (int)(mSpriteTexture.Height * Scale));
        }

        //Draw the sprite to the screen
        public virtual void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(mSpriteTexture, Position,
                new Rectangle(0, 0, mSpriteTexture.Width, mSpriteTexture.Height),
                Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }

        //Update the Sprite and change it's position based on the passed in speed, direction and elapsed time.
        public void Update(GameTime theGameTime, Vector2 theSpeed, Vector2 theDirection)
        {
            Position += theDirection * theSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
        }

        //When the scale is modified throught he property, the Size of the 
        //sprite is recalculated with the new scale applied.
        public float Scale
        {
            get { return mScale; }
            set
            {
                mScale = value;
                //Recalculate the Size of the Sprite with the new scale
                Size = new Rectangle(0, 0, (int)(mSpriteTexture.Width * Scale), (int)(mSpriteTexture.Height * Scale));
            }
        }
    }
}
