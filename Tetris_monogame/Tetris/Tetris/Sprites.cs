using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Tetris
{
    class Sprites
    {

       // public Vector2 Location;
        Shapes shapeObj = new Shapes(); 
       // protected readonly Texture2D texture;
       // protected readonly Texture2D gameBoundaries;

        public Sprites()
        {

        }
        public void SpawnNewBlock(int currentShape, int x, int y)
        {
            List<int[,]> ShapeList = new List<int[,]>(shapeObj.GetShapeList());

        }
 
        

    }
}
