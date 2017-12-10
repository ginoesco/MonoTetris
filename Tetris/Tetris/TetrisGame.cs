using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;
namespace Tetris
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TetrisGame : GameBoard
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Shapes shapeObj = new Shapes();
        Shapes nextShapeObj = new Shapes();
        static Random rnd = new Random(DateTime.Now.Millisecond);
        GameBoard gbObj = new GameBoard();
        Vector2 tetrisBlock;
        KeyboardInput keyIn = new KeyboardInput(); 
        //in game
        private Texture2D block, window, space;
        //end of ingame

        //game menu
        private Texture2D options, background, playGame;
        Button optionButton, playGameButton;
        Song themeSong;
        MouseState newMouseState, lastMouseState;
        const byte menuScreen = 0, game = 1, optionScreen = 2;
        int currentScreen = menuScreen;
        //end of game menu
        private SpriteFont font;
        private KeyboardState oldKeyState;
        private KeyboardState currentKeyState;

        //game details
        private int score = 0;
        private int level = 0;
        private int linesCleared = 0;
        //end of game details

        //const int pixelWidth = 32;
        //const int pixelLength = 32;
        //const int boardX = 330;
        //const int boardY = 200;
        //int[,] shape = new int[4, 4];
        //int[,] shape2 = new int[4, 4];
        //int[,] rotated = new int[4, 4];
        //int[,] gameBoard = new int[10, 18]; // 10x 18 board
        //int[,] loadedBoard = new int[10, 18];
        int[] xcoords = new int[4];
        int[] ycoords = new int[4];

        //int posX = 330 + pixelWidth * 4;
        //int posY = 200;
        //int boundsX = boardX + pixelWidth * 8;
        //int boundsY = boardY + pixelWidth * 16;
        int rotateIndex = 0;
        int rnum = 0;
        int count = 0; //used in timer
        int currentShape = rnd.Next(1,7);
        int nextShape= rnd.Next(1,7);
        int moveLeftState = 0;
        int moveRightState = 0;
        int moveDownState = 0;

        
        public TetrisGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1000; //Make the game board a size 1k by 1k
            graphics.PreferredBackBufferHeight = 1000;
        }

        public void NewGame()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 18; j++)
                {
                    gameBoard[i, j] = 0; // Initialize each location to a zero
                    loadedBoard[i, j] = 0;
                }
            }
            //linesCleared = 0;
           //level = 1;
           //score = 0;
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // in game content
            block = Content.Load<Texture2D>("block");
            font = Content.Load<SpriteFont>("Score");
            window = Content.Load<Texture2D>("Window");
            space = Content.Load<Texture2D>("maxresdefault");

            //game menu
            playGame = Content.Load<Texture2D>("PlayGame");
            options = Content.Load<Texture2D>("options");
            background = Content.Load<Texture2D>("tetris_logo");

            optionButton = new Button(new Rectangle(400, 100, options.Width, options.Height), true);
            optionButton.load(Content, "options");

            playGameButton = new Button(new Rectangle(200, 75, playGame.Width + 100, playGame.Height), true);
            playGameButton.load(Content, "PlayGame");

            //Music
            themeSong = Content.Load<Song>("Tetris");
            MediaPlayer.Volume = 0.1f;
            MediaPlayer.IsRepeating = true;
            //MediaPlayer.Play(themeSong);
        }

        /// <summary>
        /// Based on lines cleared, change the level we are on. The higher the level we are on the faster the blocks will fall
        /// </summary>
        public void CalculateLevel()
        {
            if (level ==9 || (linesCleared >=160))
            {
                level = 9;
            }
            else if (level == 8 || (linesCleared >= 140 && linesCleared < 160))
            {
                level = 8;
            }
            else if (level ==7 || (linesCleared >= 120 && linesCleared < 140))
            {
                level = 7;
            }
            else if (level ==6 ||(linesCleared >= 100 && linesCleared < 120))
            {
                level = 6;
            }
            else if (level ==5 ||(linesCleared >= 80 && linesCleared < 100))
            {
                level = 5;
            }
            else if (level ==4 ||(linesCleared >= 60 && linesCleared < 80))
            {
                level = 4;
            }
            else if (level == 3 ||(linesCleared >= 40 && linesCleared <= 60))
            {
                level = 3;
            }
            else if (level == 2|| (linesCleared >= 20 && linesCleared < 40))
            {
                level = 2;
            }
            else if ((linesCleared >= 0 && linesCleared < 20))
            {
                level = 1;
            }
        }

        /// <summary>
        /// Used to detect when the game is over
        /// </summary>
        /// <param name="gameArray"></param>
        /// <returns></returns>
        private bool GameOver(int[,] gameArray)
        {
            int column = 1;
            for (int row = 0; row < 9; row++)
            {
                if (gameArray[row,column] != 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Depending on which level we are on, adjust the fall speed
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private int calculateTimer(int level)
        {
            int timer=0;
            switch (level)
            {
                case 1: timer = 100;
                        break;
                case 2: timer = 90;
                        break;
                case 3: timer = 80;
                        break;
                case 4: timer = 70;
                        break;
                case 5: timer = 60;
                        break;
                case 6: timer = 50;
                        break;
                case 7: timer = 40;
                        break;
                case 8: timer = 30;
                        break;
                case 9: timer = 20;
                        break;
            }
            return timer; 
        }

        /// <summary>
        /// To make the blocks fall down at a certain speed defined by the parameter
        /// </summary>
        /// <param name="timer"></param>
        private void Fall(int timer)
        {
            if (posY < boundsY)
            {
                if (count == timer)
                {
                    posY += pixelWidth;
                    count = 0;
                }
                count++;
            }
        }

        /// <summary>
        /// Used to rotate our shapes
        /// </summary>
        /// <param name="currentShape"></param>
        private void Rotate(int currentShape)
        {
            switch (currentShape)
            {
                case 0:
                    rotate = shapeObj.GetRotate_T();
                    break;
                case 1:
                    rotate = shapeObj.GetRotate_Z();
                    break;
                case 2:
                    rotate = shapeObj.GetRotate_S();
                    break;
                case 3:
                    rotate = shapeObj.GetRotate_L();
                    break;
                case 4:
                    rotate = shapeObj.GetRotate_J();
                    break;
                case 5:
                    rotate = shapeObj.GetRotate_Sq();
                    break;
                case 6:
                    rotate = shapeObj.GetRotate_Line();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Using a random number generator, this is used to randomly select the next shape
        /// </summary>
        public void SpawnShape()
        {
            posX = 330 + pixelWidth * 4;
            posY = 200;
            rnum = rnd.Next(0, 7);

            currentShape = nextShape;
            nextShape = rnum;
        }

        /// <summary>
        /// Moving the blocks down, left, or right.
        /// </summary>
        public void MoveKeys()
        {
            int blockstate = (int)gbObj.CheckPlacement(loadedBoard, shape, posX, posY);

            // Console.WriteLine("blockstate: {0}, {1}", moveLeftState, moveRightState);
            //int rightWall = boundsX + pixelWidth * 9;
            if (oldKeyState.IsKeyDown(Keys.Up) && currentKeyState.IsKeyUp(Keys.Up) && blockstate != 1)
            { //updates when up is pressed
                Rotate(currentShape);
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
                if (moveLeftState >= 362)
                {
                    posX -= pixelWidth;
                }



            }
            else if (oldKeyState.IsKeyDown(Keys.Right) && currentKeyState.IsKeyUp(Keys.Right))
            {
                //int blockstate = (int)gbObj.CheckPlacement(gameBoard, shape, (int)tetrisBlock.X, (int)tetrisBlock.Y);
                //moveRightState = blockstate;

                if (moveRightState < boundsX)
                {
                    posX += pixelWidth;
                }

            }
            else if (oldKeyState.IsKeyDown(Keys.Down) && currentKeyState.IsKeyUp(Keys.Down))
            {
                //if (moveDownState <= boundsY)
                posY += pixelWidth;
            }
            else if (oldKeyState.IsKeyDown(Keys.Enter) && currentKeyState.IsKeyUp(Keys.Enter))
            { //updates when enter is pressed
                SpawnShape();
            }


        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            block.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        int j = 0;
        int blockstate = -1;
        protected override void Update(GameTime gameTime)
        {
            DeleteLines(loadedBoard);

            oldKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();

            lastMouseState = newMouseState;
            newMouseState = Mouse.GetState();
            int blocked = (int)GameBoard.BlockStates.Blocked;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (j < 4 && blockstate != blocked)
            {
                blockstate = (int)gbObj.CheckPlacement(loadedBoard, shape, xcoords[j], ycoords[j]);
                j++;
            }
            else
            {
                j = 0;
            }


            //Checks for what keys are pressed, Moves or rotates block
            if (blockstate != blocked || (oldKeyState.IsKeyDown(Keys.Enter) && currentKeyState.IsKeyUp(Keys.Enter)))
            {
                
                Console.WriteLine("Elapsed time: {0}", gameTime.ElapsedGameTime.Seconds);
                MoveKeys();
            }
  
            if (blockstate == blocked)
            {
                //DeleteLines(loadedBoard);
                for (int i = 0; i < 4; i++)
                {
                    Array.Copy(gbObj.LoadBoard(loadedBoard, shape, xcoords[i], ycoords[i]), loadedBoard, loadedBoard.Length);
                }
                gbObj.ShowBoard(loadedBoard);
                blockstate = -1;
                SpawnShape();
            }
            // TODO: Add your update logic here

            //Testing menu
            switch (currentScreen)
            {
                case menuScreen:

                    if ((playGameButton.update(new Vector2(newMouseState.X, newMouseState.Y)) == true && (newMouseState != lastMouseState) && (newMouseState.LeftButton == ButtonState.Pressed))
                                || ((currentKeyState != oldKeyState) && currentKeyState.IsKeyDown(Keys.Enter)))
                    {//play the game
                        NewGame();
                        currentScreen = game;
                    }
                    if (optionButton.update(new Vector2(newMouseState.X, newMouseState.Y)) == true && newMouseState != lastMouseState && newMouseState.LeftButton == ButtonState.Pressed)
                    {//goto options screen
                        currentScreen = optionScreen;
                    }
                    break;

                case game:
                    if (currentKeyState != oldKeyState && currentKeyState.IsKeyDown(Keys.Escape))
                    {
                        currentScreen = menuScreen;
                    }
                    if (GameOver(loadedBoard))
                    {
                        currentScreen = menuScreen;
                    }
                    Fall(calculateTimer(level));
                    break;
            }
            DeleteLines(loadedBoard);
            base.Update(gameTime);
        }

        public void drawShape(bool whichShape)
        {
            List<int[,]> shapeList = shapeObj.GetShapeList();
            List<int[,]> nextShapeList = nextShapeObj.GetShapeList();
            List<Color> Colors = shapeObj.GetColorList();
            int leftmostX = 99;
            int rightmostX = -1;
            int lowestY = -1;
            int j = 0;
            if (whichShape) //Drawing the current game board shape
            {
                shape = shapeList[currentShape];

                for (int i = 0; i < 4; i++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        if (shape[k, i] != 0)
                        {
                            if (i < leftmostX)
                            {
                                leftmostX = i;
                                moveLeftState = posX + i * pixelWidth;
                            }
                            if (i > rightmostX)
                            {
                                rightmostX = i;
                                moveRightState = posX + i * pixelWidth;
                            }
                            if (k > lowestY)
                            {
                                lowestY = k;
                                moveDownState = posY + k * pixelLength;

                            }

                            spriteBatch.Draw(block, tetrisBlock = new Vector2(posX + i * pixelWidth, posY + k * pixelLength), Colors[currentShape]);

                            //stores coords of each block
                            if (j < 4)
                            {
                                xcoords[j] = (int)tetrisBlock.X;
                                ycoords[j] = (int)tetrisBlock.Y;
                                j++;
                            }

                        }


                    }
                }
            }
            else //Drawing the shape in the next shape block
            {
                shape2 = nextShapeList[nextShape];
                for (int i = 0; i < 4; i++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        if (shape2[k, i] != 0)
                        {
                            spriteBatch.Draw(block, new Rectangle(750 + i * pixelWidth, 500 + k * pixelLength, pixelWidth, pixelLength), Colors[nextShape]);
                        }
                    }
                }
            }

        }

        public void DeleteLines(int[,] gameArray)
        {
            int oldLines = linesCleared;
            for (int y =17; y>=0; y--) //Traversing the row
            {
                bool clear = true;
                for (int x =0; x<10; x++) //Traversing the columns
                {
                    if (gameArray[x,y] == 0)
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
            CalculateScore(oldLines, linesCleared);
            CalculateLevel();
        }

        /// <summary>
        /// Function used to calculate the score of the current game. As the user advances to a higher level, there is a multiplication factor
        /// </summary>
        /// <param name="oldLines"></param>
        /// <param name="newLines"></param>
        public void CalculateScore(int oldLines, int newLines)
        {
            switch (newLines - oldLines)
            {
                case 0: score += 0;
                        break;
                case 1: score += 40*(level+1);
                        break;
                case 2: score += 100* (level + 1);
                        break;
                case 3: score += 300* (level + 1);
                        break;
                case 4: score += 1200* (level + 1);
                        break;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            switch (currentScreen)
            {
                case menuScreen:
                    spriteBatch.Begin();
                    spriteBatch.Draw(background, GraphicsDevice.Viewport.Bounds, Color.White);
                    if (playGameButton.update(new Vector2(newMouseState.X, newMouseState.Y)) == true) 
                    {//If mouse is in playbutton range, draw white box around it
                        spriteBatch.Draw(window, new Rectangle(199, 99, 302, 52), Color.White);
                    }
                    spriteBatch.Draw(playGame, new Rectangle(200, 100, 300, 50), Color.White);
                    spriteBatch.Draw(options, new Rectangle(600, 100, options.Width, options.Height), Color.White);
                    spriteBatch.End();
                    break;

                case game:
                    GraphicsDevice.Clear(Color.Gray);
                    List<int[,]> GameBoardList = gbObj.GetGameBoard();
                    Color boardColor = new Color();
                    bool boardShape = true;
                    bool nextShape = false;
                    gameBoard = GameBoardList[0];
                    //Game board
                    spriteBatch.Begin();
                    spriteBatch.Draw(space, GraphicsDevice.Viewport.Bounds, Color.White);
                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < 18; j++)
                        {
                            if (gameBoard[i, j] == 0)
                            {

                                boardColor = Color.FromNonPremultiplied(50, 50, 50, 50);
                                spriteBatch.Draw(block, new Rectangle(BoardWidth + i * pixelWidth, BoardHeight + j * pixelWidth, pixelWidth, pixelWidth), new Rectangle(0, 0, 32, 32), Color.DarkGray);
                            }
                        }
                    }
                    spriteBatch.End();
                    gbObj.UpdateBoard(loadedBoard, block, spriteBatch);


                    //Drawing the shape to go onto the board
                    spriteBatch.Begin();
                    drawShape(boardShape);
                    spriteBatch.End();


                    //display the score, level, and lines cleared
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font, "Score: "+ score, new Vector2(700, 200), Color.White);
                    spriteBatch.DrawString(font, "Level: " + level, new Vector2(700, 300), Color.White);
                    spriteBatch.DrawString(font, "Lines Cleared: " + linesCleared, new Vector2(50,200), Color.White);
                    spriteBatch.End();

                    //Next block square
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font, "Next Block", new Vector2(700, 400), Color.White);
                    spriteBatch.Draw(window, new Rectangle(700, 450, 200, 200), Color.Gray);
                    drawShape(nextShape);
                    spriteBatch.End();

                    break;
            }
            base.Draw(gameTime);
        }
    }
}
