using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEngine
{
    class OtherPlayer
    {
        public Animation shoot;
        public Animation die;
        public Animation wait;

        public Game Game
        {
            get { return game; }
        }
        Game game;

        public bool IsAlive {
            get { return IsAlive; }
        }
        bool IsALive;

        public Vector2 Position {
            get { return position; }
            set { position = value; }
        }
        Vector2 position;

        public Vector2 Velocity {
            get { return velocity; }
            set { velocity = value; }
        }
        Vector2 velocity;
        
        // mengontrol pergerakan
        private const float MoveAcceleration = 7000.0f;
        private const float MaxMoveSpeed = 1000.0f;
        private const float GroundDragFactor = 0.58f;
        

           
    }
}
