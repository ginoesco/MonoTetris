using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;
/// <summary>
/// Aikman Ong - 816056118
/// Giancarlo Escolano - 813215631
/// COMPE361 Final Project
/// </summary>
namespace Tetris
{
    /// <summary>
    /// Aikamn Ong - 816056118
    /// Giancarlo Escolano 
    /// Main class for Tetris.
    /// </summary>
    public class TetrisGame : GameBoard
    {
        //Objects of other classes
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Shapes shapeObj = new Shapes();
        Shapes nextShapeObj = new Shapes();
        GameBoard gbObj = new GameBoard();
        Vector2 tetrisBlock;
        KeyboardInput keyIn = new KeyboardInput();
        Levels levelobj = new Levels();

        //Get the old keyboard state and current keyboard state to detect button pushes
        private KeyboardState oldKeyState;
        private KeyboardState currentKeyState;

        //option menu buttons and fonts
        private SpriteFont optionFont, enlargedFont;
        private SpriteFont font;
        private Texture2D backArrow;
        //end of option menu

        //in game board textures
        private Texture2D block, window, space;

        //Custom buttons for menus
        Button arrow, textWindow, soundWindow, pauseButton, newGame, mainMenu, loadSave;


        //game menu
        private Texture2D options, background, playGame;
        Button optionButton, playGameButton;
        Song themeSong;
        MouseState newMouseState, lastMouseState;
        const byte menuScreen = 0, game = 1, optionScreen = 2, pauseScreen = 3, gameOver = 4;
        int currentScreen = menuScreen;
        //end of game menu


        //game details
        private int score = 0;
        private int level = 1;
        private int linesCleared = 0;
        //end of game details

        int[] xcoords = new int[4];
        int[] ycoords = new int[4];

        int posX = 330 + pixelWidth * 4;
        int posY = 200;

        int count = 0;//Used in Fall method

        SaveLoad saveload = new SaveLoad(); 
        /// <summary>
        /// Constructor to make size of window 1k by 1k
        /// </summary>
        public TetrisGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "Tetris by Aikman Ong and Giancarlo Escolano";
            graphics.PreferredBackBufferWidth = 1000; //Make the game board a size 1k by 1k
            graphics.PreferredBackBufferHeight = 1000;
        }

        /// <summary>
        /// Initialize the gameboards to zero to make them empty
        /// </summary>
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

        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            IsMouseVisible = true; //Mouse in constantly visible

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
            backArrow = Content.Load<Texture2D>("arrow");
            background = Content.Load<Texture2D>("tetris_logo");
            optionFont = Content.Load<SpriteFont>("tetrisFont");
            enlargedFont = Content.Load<SpriteFont>("EnlargedFont");


            //buttons and define their boundaries
            optionButton = new Button(new Rectangle(600, 100, options.Width, options.Height), true);
            optionButton.load(Content, "options");

            playGameButton = new Button(new Rectangle(200, 100, playGame.Width + 100, playGame.Height), true);
            playGameButton.load(Content, "PlayGame");


            arrow = new Button(new Rectangle(25, 900, 75, 75), true);
            arrow.load(Content, "arrow");

            textWindow = new Button(new Rectangle(350, 500, 290, 50), true); //Responsible for quit game button
            soundWindow = new Button(new Rectangle(500, 900, 350, 50), true); //Responsible for mute/unmute
            pauseButton = new Button(new Rectangle(200, 900, 150, 40), true); //Pause button
            newGame = new Button(new Rectangle(350, 300, 290, 50), true); //New Game/Restart button
            mainMenu = new Button(new Rectangle(350, 400, 290,50),true); //Going back to main menu
            loadSave = new Button(new Rectangle(350,200,290,50),true); //Load and save button

            //Music
            themeSong = Content.Load<Song>("Tetris");
            MediaPlayer.Volume = 0.1f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(themeSong);
        }


        /// <summary>
        /// Used to detect when the game is over. When blocks reach the very top, the game is over
        /// </summary>
        /// <param name="gameArray"></param>
        /// <returns></returns>
        private bool GameOver(int[,] gameArray)
        {
            int column = 0;
            for (int row = 0; row < 9; row++)
            {
                if (gameArray[row, column] != 0)
                {
                    return true;
                }
            }
            return false;
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
                if (count > timer)
                {
                    count = 0;
                }
                count++;
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

        public void KeyUp()
        {
            int blockstate = (int)gbObj.CheckPlacement(loadedBoard, shape, posX, posY);

            // Console.WriteLine("blockstate: {0}, {1}", moveLeftState, moveRightState);
            //int rightWall = boundsX + pixelWidth * 9;
            if (oldKeyState.IsKeyDown(Keys.Up) && currentKeyState.IsKeyUp(Keys.Up) && blockstate != 1)
            { //updates when up is pressed
                List<int[,]> rotate = shapeObj.Rotate(currentShape);


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
        }
        /// <summary>
        /// Moving the blocks down, left, or right.
        /// </summary>
        public void MoveKeys()
        {

            if (oldKeyState.IsKeyDown(Keys.Left))
            {
                //int blockstate = (int)gbObj.CheckPlacement(gameBoard, shape,(int)tetrisBlock.X, (int)tetrisBlock.Y);
                // moveLeftState = blockstate;
                if (moveLeftState >= 362)
                {
                    posX -= pixelWidth;
                }



            }
            else if (oldKeyState.IsKeyDown(Keys.Right))
            {
                //int blockstate = (int)gbObj.CheckPlacement(gameBoard, shape, (int)tetrisBlock.X, (int)tetrisBlock.Y);
                //moveRightState = blockstate;

                if (moveRightState < boundsX)
                {
                    posX += pixelWidth;
                }

            }
            else if (oldKeyState.IsKeyDown(Keys.Down))
            {
                if (moveDownState <= boundsY)
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
            int oldLines = linesCleared;
            linesCleared = gbObj.DeleteLines(loadedBoard, linesCleared);

            oldKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();

            //Old mouse state and newest mouse state
            lastMouseState = newMouseState;
            newMouseState = Mouse.GetState();

            int blocked = (int)GameBoard.BlockStates.Blocked;

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
                if (keyIn.KeyboardTimer(3))
                {
                    MoveKeys();
                }
                KeyUp();
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

            //To display which menu of the game
            switch (currentScreen)
            {
                case menuScreen:

                    //When play game is pressed
                    if ((playGameButton.update(new Vector2(newMouseState.X, newMouseState.Y)) == true && (newMouseState != lastMouseState) && (newMouseState.LeftButton == ButtonState.Pressed))
                                || ((currentKeyState != oldKeyState) && currentKeyState.IsKeyDown(Keys.Enter)))
                    {//play the game
                        NewGame();
                        linesCleared = 0;
                        level = 1;
                        score = 0;
                        currentScreen = game;
                    }
                    if (optionButton.update(new Vector2(newMouseState.X, newMouseState.Y)) == true && newMouseState != lastMouseState && newMouseState.LeftButton == ButtonState.Pressed)
                    {//goto options screen
                        currentScreen = optionScreen;
                    }
                    if (soundWindow.update(new Vector2(newMouseState.X, newMouseState.Y)) == true && newMouseState != lastMouseState && newMouseState.LeftButton == ButtonState.Pressed)
                    {//Mute/unmute music
                        if (MediaPlayer.State == MediaState.Paused)
                        {
                            MediaPlayer.Resume();
                        }
                        else if (MediaPlayer.State == MediaState.Playing)
                        {
                            MediaPlayer.Pause();
                        }
                    }
                    break;

                case optionScreen:
                    {
                        if (arrow.update(new Vector2(newMouseState.X, newMouseState.Y)) == true && newMouseState != lastMouseState && newMouseState.LeftButton == ButtonState.Pressed)
                        {//goto main menu
                            currentScreen = menuScreen;
                        }
                        if (textWindow.update(new Vector2(newMouseState.X, newMouseState.Y)) == true && newMouseState != lastMouseState && newMouseState.LeftButton == ButtonState.Pressed)
                        {//Escape game
                            Exit();
                        }
                        if (soundWindow.update(new Vector2(newMouseState.X, newMouseState.Y)) == true && newMouseState != lastMouseState && newMouseState.LeftButton == ButtonState.Pressed)
                        {//Mute/unmute music
                            if (MediaPlayer.State == MediaState.Paused)
                            {
                                MediaPlayer.Resume();
                            }
                            else if (MediaPlayer.State == MediaState.Playing)
                            {
                                MediaPlayer.Pause();
                            }
                        }
                        if (loadSave.update(new Vector2(newMouseState.X, newMouseState.Y)) == true && newMouseState != lastMouseState && newMouseState.LeftButton == ButtonState.Pressed)
                        {
                            //Code to load game
                            Array.Copy(saveload.Load(), loadedBoard, loadedBoard.Length);
                            gbObj.ShowBoard(loadedBoard);
                            currentScreen = game;
                        }
                            break;
                    }
                case pauseScreen: //When the user pauses the game
                    {
                        if (arrow.update(new Vector2(newMouseState.X, newMouseState.Y)) == true && newMouseState != lastMouseState && newMouseState.LeftButton == ButtonState.Pressed)
                        {//Go out of option screen back to the game
                            currentScreen = game;
                        }
                        if (textWindow.update(new Vector2(newMouseState.X, newMouseState.Y)) == true && newMouseState != lastMouseState && newMouseState.LeftButton == ButtonState.Pressed)
                        {//Escape game
                            Exit();
                        }
                        if (newGame.update(new Vector2(newMouseState.X, newMouseState.Y)) == true && newMouseState != lastMouseState && newMouseState.LeftButton == ButtonState.Pressed)
                        { //Restarting the game
                            NewGame();
                            linesCleared = 0;
                            level = 1;
                            score = 0;
                            currentScreen = game;
                        }
                        if (soundWindow.update(new Vector2(newMouseState.X, newMouseState.Y)) == true && newMouseState != lastMouseState && newMouseState.LeftButton == ButtonState.Pressed)
                        {//Mute/unmute music
                            if (MediaPlayer.State == MediaState.Paused)
                            {
                                MediaPlayer.Resume();
                            }
                            else if (MediaPlayer.State == MediaState.Playing)
                            {
                                MediaPlayer.Pause();
                            }
                        }
                        if (loadSave.update(new Vector2(newMouseState.X, newMouseState.Y)) == true && newMouseState != lastMouseState && newMouseState.LeftButton == ButtonState.Pressed)
                        {
                            //Code to save game
                            saveload.Save(loadedBoard);
                        }
                            break;
                    }
                case gameOver:
                    {
                        if (newGame.update(new Vector2(newMouseState.X, newMouseState.Y)) == true && newMouseState != lastMouseState && newMouseState.LeftButton == ButtonState.Pressed)
                        {//Making a new game
                            NewGame();
                            linesCleared = 0;
                            level = 1;
                            score = 0;
                            currentScreen = game;
                            break;
                        }
                        if (soundWindow.update(new Vector2(newMouseState.X, newMouseState.Y)) == true && newMouseState != lastMouseState && newMouseState.LeftButton == ButtonState.Pressed)
                        {//mute/unmute music
                            if (MediaPlayer.State == MediaState.Paused)
                            {
                                MediaPlayer.Resume();
                            }
                            else if (MediaPlayer.State == MediaState.Playing)
                            {
                                MediaPlayer.Pause();
                            }
                        }
                        if (mainMenu.update(new Vector2(newMouseState.X, newMouseState.Y)) == true && newMouseState != lastMouseState && newMouseState.LeftButton == ButtonState.Pressed)
                        { //Go back to main menu
                            currentScreen = menuScreen;
                        }
                        break;
                    }
                case game:


                    if (currentKeyState != oldKeyState && currentKeyState.IsKeyDown(Keys.Home))
                    { //When user presses home key, increment level
                        level++;
                    }
                    if (soundWindow.update(new Vector2(newMouseState.X, newMouseState.Y)) == true && newMouseState != lastMouseState && newMouseState.LeftButton == ButtonState.Pressed)
                    {//mute/unmute music
                        if (MediaPlayer.State == MediaState.Paused)
                        {
                            MediaPlayer.Resume();
                        }
                        else if (MediaPlayer.State == MediaState.Playing)
                        {
                            MediaPlayer.Pause();
                        }
                    }
                    if ((pauseButton.update(new Vector2(newMouseState.X, newMouseState.Y)) == true && newMouseState != lastMouseState && newMouseState.LeftButton == ButtonState.Pressed)
                            || (currentKeyState.IsKeyDown(Keys.LeftControl) && currentKeyState.IsKeyDown(Keys.P)))
                    {//Either user presses the pause button or user press left control+P to go to pause screen
                        currentScreen = pauseScreen;
                    }
                    if (GameOver(loadedBoard)) //If game is over, go to game over screen
                    {
                        currentScreen = gameOver;
                    }
                    Fall(levelobj.CalculateTimer(level)); //Allow the blocks to fall
                    linesCleared = gbObj.DeleteLines(loadedBoard, linesCleared); //Clear the lines
                    level = levelobj.CalculateLevel(level, linesCleared); //Calculate the level we are on
                    score = levelobj.CalculateScore(oldLines, linesCleared, level, score); //Calculate the score
                    break;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Drawing the game board shapes that appear on the screen
        /// </summary>
        /// <param name="whichShape"></param>
        public void DrawShape(bool whichShape)
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

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            switch (currentScreen)
            {
                case menuScreen: //Default main menu screen
                    optionButton.Available = true;
                    playGameButton.Available = true;
                    arrow.Available = false;

                    spriteBatch.Begin();
                    spriteBatch.Draw(background, GraphicsDevice.Viewport.Bounds, Color.White);


                    if (playGameButton.update(new Vector2(newMouseState.X, newMouseState.Y)) == true)
                    {//If mouse is in playbutton range, draw white box around it
                        spriteBatch.Draw(window, new Rectangle(199, 99, 302, 52), Color.White);
                    }
                    spriteBatch.Draw(playGame, new Rectangle(200, 100, 300, 50), Color.White);
                    if (optionButton.update(new Vector2(newMouseState.X, newMouseState.Y)))
                    {//If mouse is in option range, draw white box around it
                        spriteBatch.Draw(window, new Rectangle(599, 99, options.Width + 2, options.Height + 2), Color.White);
                    }

                    if (soundWindow.update(new Vector2(newMouseState.X, newMouseState.Y)))
                    {//If mouse is in mute/unmute, draw larger text around it
                        spriteBatch.DrawString(enlargedFont, "Mute/Unmute", new Vector2(500, 900), Color.White);
                    }
                    else
                    {
                        spriteBatch.DrawString(optionFont, "Mute/Unmute", new Vector2(500, 900), Color.White);
                    }
                    spriteBatch.Draw(options, new Rectangle(600, 100, options.Width, options.Height), Color.White);
                    spriteBatch.End();
                    break;

                case optionScreen://When the user goes to the option screen
                    {
                        optionButton.Available = false;
                        playGameButton.Available = false;
                        arrow.Available = true;

                        spriteBatch.Begin();
                        spriteBatch.Draw(window, GraphicsDevice.Viewport.Bounds, Color.DimGray);
                        if (arrow.update(new Vector2(newMouseState.X, newMouseState.Y)))
                        {//If mouse is in back arrow range, draw white box around it
                            spriteBatch.Draw(window, new Rectangle(24, 899, 77, 77), Color.White);
                        }
                        //Drawing the arrow in the bottom left corner
                        spriteBatch.Draw(backArrow, new Rectangle(25, 900, 75, 75), Color.White);

                        if (loadSave.update(new Vector2(newMouseState.X, newMouseState.Y)))
                        {
                            spriteBatch.DrawString(enlargedFont, "Load Game", new Vector2(350, 200), Color.White);

                        }
                        else
                        {
                            spriteBatch.DrawString(optionFont, "Load Game", new Vector2(350, 200), Color.White);
                        }
                        if (textWindow.update(new Vector2(newMouseState.X, newMouseState.Y)))
                        {//If mouse is in "quit game" range, draw larger text
                            spriteBatch.DrawString(enlargedFont, "Quit Game", new Vector2(350, 500), Color.White);

                        }
                        else
                        {
                            spriteBatch.DrawString(optionFont, "Quit Game", new Vector2(350, 500), Color.White);

                        }

                        if (soundWindow.update(new Vector2(newMouseState.X, newMouseState.Y)))
                        {//If mouse is in mute/unmute, draw larger text around it
                            spriteBatch.DrawString(enlargedFont, "Mute/Unmute", new Vector2(500, 900), Color.White);
                        }
                        else
                        {
                            spriteBatch.DrawString(optionFont, "Mute/Unmute", new Vector2(500, 900), Color.White);
                        }

                        spriteBatch.End();
                        break;
                    }

                case pauseScreen://Buttons for the pause screen
                    {
                        arrow.Available = true;

                        spriteBatch.Begin();
                        spriteBatch.Draw(window, GraphicsDevice.Viewport.Bounds, Color.DimGray);

                        if (arrow.update(new Vector2(newMouseState.X, newMouseState.Y)))
                        {//If mouse is in back arrow range, draw white box around it
                            spriteBatch.Draw(window, new Rectangle(24, 899, 77, 77), Color.White);
                        }
                        spriteBatch.Draw(backArrow, new Rectangle(25, 900, 75, 75), Color.White);

                        if (loadSave.update(new Vector2(newMouseState.X, newMouseState.Y)))
                        {
                            spriteBatch.DrawString(enlargedFont, "Save Game", new Vector2(350, 200), Color.White);

                        }
                        else
                        {
                            spriteBatch.DrawString(optionFont, "Save Game", new Vector2(350, 200), Color.White);
                        }

                        if (newGame.update(new Vector2(newMouseState.X, newMouseState.Y)))
                        {//If mouse is on the "restart game", draw larger text around to indicate to user you are able to click the button
                            spriteBatch.DrawString(enlargedFont, "Restart Game", new Vector2(350, 300), Color.White);

                        }
                        else
                        {
                            spriteBatch.DrawString(optionFont, "Restart Game", new Vector2(350, 300), Color.White);

                        }

                        if (textWindow.update(new Vector2(newMouseState.X, newMouseState.Y)))
                        {//If mouse is in "quit game" range, draw larger text around it
                            spriteBatch.DrawString(enlargedFont, "Quit Game", new Vector2(350, 500), Color.White);

                        }
                        else
                        {
                            spriteBatch.DrawString(optionFont, "Quit Game", new Vector2(350, 500), Color.White);

                        }

                        if (soundWindow.update(new Vector2(newMouseState.X, newMouseState.Y)))
                        {//If mouse is in mute/unmute, draw larger around it
                            spriteBatch.DrawString(enlargedFont, "Mute/Unmute", new Vector2(500, 900), Color.White);
                        }
                        else
                        {
                            spriteBatch.DrawString(optionFont, "Mute/Unmute", new Vector2(500, 900), Color.White);
                        }

                        spriteBatch.End();
                        break;
                    }

                case gameOver:
                    {
                        spriteBatch.Begin();
                        spriteBatch.Draw(window, GraphicsDevice.Viewport.Bounds, Color.DimGray);

                        //Draw game over and user's score for the game
                        spriteBatch.DrawString(optionFont, "Game Over", new Vector2(350, 100), Color.White);
                        spriteBatch.DrawString(optionFont, "Your score: " + score, new Vector2(350, 200), Color.White);

                        if (newGame.update(new Vector2(newMouseState.X, newMouseState.Y)))
                        {//If mouse is on "new game" range, draw larger text around it
                            spriteBatch.DrawString(enlargedFont, "New Game ", new Vector2(350, 300), Color.White);
                        }
                        else
                        {
                            spriteBatch.DrawString(optionFont, "New Game ", new Vector2(350, 300), Color.White);
                        }
                        if (mainMenu.update(new Vector2(newMouseState.X, newMouseState.Y)))
                        {   //If mouse is on "main menu" range, draw larger text around it
                            spriteBatch.DrawString(enlargedFont, "Main Menu ", new Vector2(350, 400), Color.White);

                        }
                        else
                        {
                            spriteBatch.DrawString(optionFont, "Main Menu ", new Vector2(350, 400), Color.White);
                        }

                        if (textWindow.update(new Vector2(newMouseState.X, newMouseState.Y)))
                        {//If mouse is in "quit game" range, draw larger text around it
                            spriteBatch.DrawString(enlargedFont, "Quit Game", new Vector2(350, 500), Color.White);

                        }
                        else
                        {
                            spriteBatch.DrawString(optionFont, "Quit Game", new Vector2(350, 500), Color.White);

                        }

                        if (soundWindow.update(new Vector2(newMouseState.X, newMouseState.Y)))
                        {//If mouse is in mute/unmute, draw larger text around it
                            spriteBatch.DrawString(enlargedFont, "Mute/Unmute", new Vector2(500, 900), Color.White);
                        }
                        else
                        {
                            spriteBatch.DrawString(optionFont, "Mute/Unmute", new Vector2(500, 900), Color.White);
                        }

                        spriteBatch.End();
                        break;
                    }
                case game: //Drawing the game board

                    GraphicsDevice.Clear(Color.Gray);
                    List<int[,]> GameBoardList = gbObj.GetGameBoard();
                    Color boardColor = new Color();
                    bool boardShape = true;
                    bool nextShape = false;
                    gameBoard = GameBoardList[0];

                    spriteBatch.Begin();
                    //Creating the space background
                    spriteBatch.Draw(space, GraphicsDevice.Viewport.Bounds, Color.White);
                    for (int i = 0; i < 10; i++)
                    {//Drawing the grid
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
                    DrawShape(boardShape);
                    spriteBatch.End();


                    //display the score, level, and lines cleared
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font, "Score: " + score, new Vector2(700, 200), Color.White);
                    spriteBatch.DrawString(font, "Level: " + level, new Vector2(700, 300), Color.White);
                    spriteBatch.DrawString(font, "Lines Cleared: " + linesCleared, new Vector2(50, 200), Color.White);
                    spriteBatch.End();

                    //Next block square
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font, "Next Block", new Vector2(700, 400), Color.White);
                    spriteBatch.Draw(window, new Rectangle(700, 450, 200, 200), Color.Gray);
                    DrawShape(nextShape);
                    if (soundWindow.update(new Vector2(newMouseState.X, newMouseState.Y)))
                    {//If mouse is in mute/unmute, draw larger text around it
                        spriteBatch.DrawString(enlargedFont, "Mute/Unmute", new Vector2(500, 900), Color.White);
                    }
                    else
                    {
                        spriteBatch.DrawString(optionFont, "Mute/Unmute", new Vector2(500, 900), Color.White);
                    }

                    if (pauseButton.update(new Vector2(newMouseState.X, newMouseState.Y)))
                    {//If mouse is in pause, draw larger text around it
                        spriteBatch.DrawString(enlargedFont, "Pause", new Vector2(200, 900), Color.White);

                    }
                    else
                    {
                        spriteBatch.DrawString(optionFont, "Pause", new Vector2(200, 900), Color.White);
                    }
                    spriteBatch.End();

                    break;
            }
            base.Draw(gameTime);
        }
    }
}