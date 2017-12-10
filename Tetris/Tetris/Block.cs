using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace Tetris
{
    public abstract class Block : Game
    {
        protected const int pixelWidth = 32;
        protected const int pixelLength = 32; 
        public Block()
        {
            
        }
    }
}
