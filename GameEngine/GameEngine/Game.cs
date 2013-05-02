using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace GameEngine
{
    class Game : IDisposable
    {
        //private Tile[,] tiles;
        private Texture2D[] layer;

        public Player Pemain
        { 
            get {return pemain;}   
        }

        Player pemain;

        public OtherPlayer PemainLain
        {
            get {return pemainLain;}
        }

        OtherPlayer pemainLain;

        //public void UpdatePlayer { }

    }
}
