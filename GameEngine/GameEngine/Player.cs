using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEngine
{
    class Player
    {
        public Animation shoot;
        public Animation die;
        public Animation wait;

        public Game game {
            get { return game; }
        }

        public bool IsAlive {
            get { return IsAlive; }
        }

        public Vector2 Position {
            get { return position; }
            set { position = value; }
        }
        Vector2 position;

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        Vector2 velocity;
        

        // mengontrol pergerakan 
        private const float MoveAcceleration = 7000.0f;
        private const float MaxMoveSpeed = 1000.0f;
        private const float GroundDragFactor = 0.58f;
        private const float AirDragFactor = 0.65f;


        public Player(Game game, Vector2 position) {
            this.game = game;

            //LoadContent;


        }

        public void LoadContent() {
            //shoot = new Animation(Game.Content.Load<Texture2D>("--insert file--"), 0.1f, true);
            //die = new Animation(Game.Content.Load<Texture2D>("--insert file--"), 0.1f, true);
            //wait = new Animation(Game.Content.Load<Texture2D>("--insert file--"), 0.1f, true);
        }   
    }
}
