using System;
using Microsoft.Xna.Framework.Graphics;


namespace GameEngine
{
    class Animation
    {
        public Texture2D Texture 
        {
            get { return texture; }
        }

        Texture2D texture;

        public float FrameTime {
            get { return frameTime; }
        }

        float frameTime;

        public int FrameWidth
        {
            // Assume square frames.
            get { return Texture.Height; }
        }

        /// <summary>
        /// Gets the height of a frame in the animation.
        /// </summary>
        public int FrameHeight
        {
            get { return Texture.Height; }
        }


        public Animation(Texture2D texture, float frameTime)
        {
            this.texture = texture;
            this.frameTime = frameTime;
        }
    }
}
