using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Aikman Ong - 816056118
/// Giancarlo Escolano - 813215631
/// COMPE361 Final Project
/// </summary>
namespace Tetris
{
    public class GameBoard : Shapes
    {

        List<int[,]> TetrisBoardList = new List<int[,]>();
       // const int pixelWidth = 32;
        //const int pixelLength = 32; 
        protected const int BoardWidth = 330;   //X,Y  position of the gameboard in the window
        protected const int BoardHeight = 200;
        protected const int boundsX = BoardWidth+pixelWidth*9;
        protected const int boundsY = BoardHeight + pixelWidth * 17;
        //protected int posX = 330 + pixelWidth * 4;
        //protected int posY = 200;

        protected int moveLeftState = 0;
        protected int moveRightState = 0;
        protected int moveDownState = 0;

        protected int[,] gameBoard = new int[10, 18]; // 10x 18 board
        protected int[,] loadedBoard = new int[10, 18];
        Shapes shapeObj = new Shapes(); 
        public GameBoard()
        {
            int[,] TetrisBoard = new int[10, 18];
            TetrisBoardList.Add(TetrisBoard);
        }

        public enum BlockStates
        {
            Blocked,  //blockstate = 0
            OffGrid,  //blockstate = 1 
            CanSet    //blockstate = 2
        }


        public BlockStates CheckPlacement(int[,] gameboard, int[,] block, int x, int y)
        {
            int blockDim = block.GetLength(0);         
            int px, py; 
            for (px = 0; px < blockDim; px++)
            {
                for (py = 0; py < blockDim; py++)
                {
                    int boardX = x;
                    int boardY = y; 

                    //Check if space is empty
                    if (block[py, px] != 0)
                    {
                        if (boardX < BoardWidth || boardX > boundsX)
                            return BlockStates.OffGrid;
                        else if (boardY >= boundsY)
                            return BlockStates.Blocked;
                        else
                        {
                            for(int k = 0; k<gameboard.GetLength(1); k++)
                            {
                                for(int j = 0; j<gameboard.GetLength(0); j++)
                                {
                                    int pos_x = 330 + j * pixelWidth;
                                    int pos_y = 200 + k * pixelLength;

                                    if (pos_x == x && pos_y == y)
                                    {
                                        int row = k + 1; //row below
                                        int col = j + 1; //looks ahead col right
                                       if(gameboard[j,row] != 0)
                                        {
                                            return BlockStates.Blocked;
                                        }

                                    }
                                }
                                    
                            }
                        }
                        
                    }

                }
            }

            return BlockStates.CanSet;
        }

        public int[,] LoadBoard(int[,] gameboard, int[,] block, int x, int y)
        {
            int[,] loadboard = new int[10, 18];
            int boardX, boardY; 
            Array.Copy(gameboard, loadboard, gameboard.Length);
            int length = loadboard.GetLength(1);
            int width = loadboard.GetLength(0);

            for (int py = 0; py < length; py++)
            {
                for (int px = 0; px < width; px++)
                {
                    boardX = 330 + px * pixelWidth;
                    boardY = 200 + py * pixelLength;

                    if(boardX == x && boardY == y)
                    {

                        for(int blkY = 0; blkY<block.GetLength(1); blkY++)
                        {
                            for(int blkX = 0; blkX<block.GetLength(0); blkX++)
                            {
                                if(block[blkX,blkY] != 0)
                                    loadboard[px, py] = block[blkX,blkY];

                            }
                        }
                      
                    }

                }
            }


            return loadboard; 
        }

        public void ShowBoard(int[,] board)
        {
            Console.WriteLine("X width, y length: {0},{1}", board.GetLength(1),board.GetLength(0)); 
            
            for(int y = 0; y < board.GetLength(1); y++)
            {
                for(int x = 0; x<board.GetLength(0); x++)
                {
                    Console.Write(" "+board[x, y]); 
                }
                Console.WriteLine("");

            }
        }

        public void UpdateBoard(int[,]gameboard, Texture2D block, SpriteBatch spriteBatch)
        {
            List<Color> colors = shapeObj.GetColorList();
            for(int i = 0; i<gameboard.GetLength(0);i++) //length
            {
                for(int k = 0; k<gameboard.GetLength(1); k++) //width
                {
                    if(gameboard[i,k] != 0)
                    {
                        Console.WriteLine("gameboard: {0}", gameboard[i, k]);

                        spriteBatch.Begin();
                        spriteBatch.Draw(block, new Vector2(BoardWidth + i * pixelWidth, BoardHeight + k * pixelLength), colors[gameboard[i,k]-1]); 
                        spriteBatch.End();
                    }
                }
            }
        }

        public int DeleteLines(int[,] gameArray, int linesCleared)
        {
            int oldLines = linesCleared;
            for (int y = 17; y >= 0; y--) //Traversing the row
            {
                bool clear = true;
                for (int x = 0; x < 10; x++) //Traversing the columns
                {
                    if (gameArray[x, y] == 0)
                    {
                        clear = false;
                    }
                }
                if (clear)
                {
                    for (int otherY = y; otherY >= 1; otherY--)
                    {
                        for (int x = 0; x < 10; x++)
                        {
                            gameArray[x, otherY] = gameArray[x, otherY - 1];
                        }
                    }
                    linesCleared++;
                    y++;
                }
            }
            return linesCleared;
            //CalculateScore(oldLines, linesCleared);
            //CalculateLevel(); 
        }
        public List<int[,]> GetGameBoard()
        {

            return TetrisBoardList;
        }
    }
}
