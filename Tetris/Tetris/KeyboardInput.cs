using System;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class KeyboardInput : GameBoard
    {
        //protected KeyboardState oldKeyState;
        //protected KeyboardState currentKeyState;
        GameBoard gbobj = new GameBoard();
        Shapes shapeobj = new Shapes();
        public void KeyboardTimer(int timer)
        {
            for (int i = timer; i > 0; i--)
            {

            }
        }

        public void MoveKeys(KeyboardState oldKeyState, KeyboardState currentKeyState, int posX, int posY)
        {
            int blockstate = (int)gbobj.CheckPlacement(loadedBoard, shape, posX, posY);

            // Console.WriteLine("blockstate: {0}, {1}", moveLeftState, moveRightState);
            //int rightWall = boundsX + pixelWidth * 9;
            if (oldKeyState.IsKeyDown(Keys.Up) && currentKeyState.IsKeyUp(Keys.Up) && blockstate != 1)
            { //updates when up is pressed
                //shapeobj.Rotate(currentShape);
                Console.WriteLine("CurrentShape, RotateLength: {0}, {1}", currentShape, rotate.Count);

                if ((currentShape == 6 || currentShape == 3 || currentShape == 4) && (moveLeftState >= 362 && moveRightState <= boundsX))
                {
                    if (rotateIndex < 4)
                        Array.Copy(rotate[rotateIndex++], shape, shape.Length);
                    else
                        rotateIndex = 0;
                }
                else if (currentShape != 6 && currentShape != 4 && currentShape != 3)
                {
                    if (rotateIndex < 4)
                    {
                        Array.Copy(rotate[rotateIndex++], shape, shape.Length);

                    }
                    else
                    {
                        rotateIndex = 0;
                    }
                }

            }
            else if (oldKeyState.IsKeyDown(Keys.Left) && currentKeyState.IsKeyUp(Keys.Left))
            {
                //int blockstate = (int)gbObj.CheckPlacement(gameBoard, shape,(int)tetrisBlock.X, (int)tetrisBlock.Y);
                // moveLeftState = blockstate;
                //if (moveLeftState >= 362)
                //{
                Console.WriteLine("Left: {0}", posX);

                posX -= pixelWidth;
                //}



            }
            else if (oldKeyState.IsKeyDown(Keys.Right) && currentKeyState.IsKeyUp(Keys.Right))
            {
                //int blockstate = (int)gbObj.CheckPlacement(gameBoard, shape, (int)tetrisBlock.X, (int)tetrisBlock.Y);
                //moveRightState = blockstate;
                Console.WriteLine("Right: {0}", posX);

                //if (moveRightState < boundsX)
                //{
                posX += pixelWidth;
                //}

            }
            else if (oldKeyState.IsKeyDown(Keys.Down) && currentKeyState.IsKeyUp(Keys.Down))
            {
                Console.WriteLine("Here");

                //if (moveDownState <= boundsY)
                posY += pixelWidth;
            }
            else if (oldKeyState.IsKeyDown(Keys.Enter) && currentKeyState.IsKeyUp(Keys.Enter))
            { //updates when enter is pressed
                //shape.SpawnShape();
            }


        }
    }

}
