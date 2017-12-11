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
        Shapes shapeObj = new Shapes();
        
        //X,Y position of the leftmost corner of the gameboard 
        protected const int BoardWidth = 330;   
        protected const int BoardHeight = 200;

        //X,Y boundaries of the gameboard 
        protected const int boundsX = BoardWidth+pixelWidth*9;
        protected const int boundsY = BoardHeight + pixelWidth * 17;

        //Move states 
        protected int moveLeftState = 0;
        protected int moveRightState = 0;
        protected int moveDownState = 0;

        //Gamboard Arrays
        protected int[,] gameBoard = new int[10, 18];   // 10x 18 board
        protected int[,] loadedBoard = new int[10, 18]; //array used when board is loaded with blocks
        public GameBoard()
        {
            //Adds instance of tetris gameboard to a list 
            int[,] TetrisBoard = new int[10, 18];
            TetrisBoardList.Add(TetrisBoard);
        }

        /// <summary>
        /// Tetris block returns these states while moving around on the board
        /// </summary>
        public enum BlockStates
        {
            Blocked,  //blockstate = 0
            OffGrid,  //blockstate = 1 
            CanSet    //blockstate = 2
        }

        /// <summary>
        /// Blockstates checkplacement updates the state of the block in real time determining 
        /// wether a tetris block is blocked, offgrid, or is ok to be placed. It will check each block 
        /// of the tetris block and will return the state. 
        /// </summary>
        /// <param name="gameboard"></param>
        /// <param name="block"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public BlockStates CheckPlacement(int[,] gameboard, int[,] block, int x, int y)
        {
            //Block dimension 
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
                        if (boardX < BoardWidth || boardX > boundsX) //Detect if board hits left or right wall of gameboard
                            return BlockStates.OffGrid;
                        else if (boardY >= boundsY) //Detect if block hits bottom of gameboard
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
                                        //if row below is not empty the tetris block is blocked 
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
        /// <summary>
        /// Loads the board with blocks that are placed this will be used 
        /// to update the gameboard and redraw the board to show the placed blocks 
        /// and their location. 
        /// </summary>
        /// <param name="gameboard"></param>
        /// <param name="block"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int[,] LoadBoard(int[,] gameboard, int[,] block, int x, int y)
        {
            int[,] loadboard = new int[10, 18];
            int boardX, boardY; 
            Array.Copy(gameboard, loadboard, gameboard.Length);
            //Board length width and height
            int length = loadboard.GetLength(1);
            int width = loadboard.GetLength(0);

            //runs through the gameboard and places block according to its x,y position 
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
                                    loadboard[px, py] = block[blkX,blkY]; //places the blocks number values into the board 

                            }
                        }
                      
                    }

                }
            }


            return loadboard; 
        }

        /// <summary>
        /// Used for console debugging to determine that the gameboard array is properly storing the blocks. 
        /// </summary>
        /// <param name="board"></param>
        public void ShowBoard(int[,] board)
        {            
            for(int y = 0; y < board.GetLength(1); y++)
            {
                for(int x = 0; x<board.GetLength(0); x++)
                {
                    Console.Write(" "+board[x, y]); 
                }
                Console.WriteLine("");

            }
        }
        /// <summary>
        /// Used to update the gameboard eachtime a new block is placed. 
        /// Runs throught the gameboard array and draws their colors respective 
        /// to their number values. 
        /// </summary>
        /// <param name="gameboard"></param>
        /// <param name="block"></param>
        /// <param name="spriteBatch"></param>
        public void UpdateBoard(int[,]gameboard, Texture2D block, SpriteBatch spriteBatch)
        {
            List<Color> colors = shapeObj.GetColorList();
            for(int i = 0; i<gameboard.GetLength(0);i++) //length
            {
                for(int k = 0; k<gameboard.GetLength(1); k++) //width
                {
                    //If gameboard element is nonzero, it draws the block. 
                    if(gameboard[i,k] != 0)
                    {
                        spriteBatch.Begin();
                        spriteBatch.Draw(block, new Vector2(BoardWidth + i * pixelWidth, BoardHeight + k * pixelLength), colors[gameboard[i,k]-1]); 
                        spriteBatch.End();
                    }
                }
            }
        }
        /// <summary>
        /// Eliminates lines when completed lines are present 
        /// copies rows above it and rechecks them to make sure they're completed or not. 
        /// </summary>
        /// <param name="gameArray"></param>
        /// <param name="linesCleared"></param>
        /// <returns></returns>
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
 
        }
        /// <summary>
        /// Gets gameboard list 
        /// </summary>
        /// <returns></returns>
        public List<int[,]> GetGameBoard()
        {

            return TetrisBoardList;
        }
    }
}
