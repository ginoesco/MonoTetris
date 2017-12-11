using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

/// <summary>
/// Aikman Ong - 816056118
/// Giancarlo Escolano - 813215631
/// COMPE361 Final Project
/// </summary>
namespace Tetris
{
    /// <summary>
    /// Custom class to make clickable buttons for our menus
    /// </summary>
    class Button
    {
        Rectangle posSize;
        bool clicked;
        bool available;
        Texture2D image;


        /// <summary>
        /// Constructor to make a button if no parameters are defined
        /// </summary>
        public Button()
        {
            posSize = new Rectangle(100, 100, 100, 50);
            clicked = false;
            available = true;
        }

        /// <summary>
        /// Overloaded constructor to make button custom size
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="avail"></param>
        public Button(Rectangle rec, bool avail)
        {
            posSize = rec;
            available = avail;
            clicked = false;
        }

        /// <summary>
        /// Load any custom content
        /// </summary>
        /// <param name="content"></param>
        /// <param name="name"></param>
        public void load(ContentManager content, string name)
        {
            image = content.Load<Texture2D>(name);
        }

        /// <summary>
        /// To check to see if the current mouses state is in the boundaries of the button defined in the constructor
        /// </summary>
        /// <param name="mouse"></param>
        /// <returns></returns>
        public bool update(Vector2 mouse)
        {
            if (mouse.X >= posSize.X && mouse.X <= posSize.X + posSize.Width && mouse.Y >= posSize.Y && mouse.Y <= posSize.Y + posSize.Height)
            {
                clicked = true;
            }

            else
            {
                clicked = false;
            }

            if (!available)
            {
                clicked = false;
            }

            return clicked;

        }

        //Draw
        public void draw(SpriteBatch sp)
        {

            Color col = Color.White;

            if (!available)
            {
                col = new Color(50, 50, 50);
            }

            if (clicked)
            {
                col = Color.Green;
            }

            sp.Draw(image, posSize, col);

        }


        /// <summary>
        /// Properties for Clicked, available, and PosSize.
        /// </summary>
        public bool Clicked
        {

            get { return clicked; }

            set { clicked = value; }

        }

        public bool Available
        {

            get { return available; }

            set { available = value; }

        }

        public Rectangle PosSize
        {

            get { return posSize; }

            set { posSize = value; }

        }

    }
}
