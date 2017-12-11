using System;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;
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
    public class KeyboardInput : GameBoard
    {
        GameBoard gbobj = new GameBoard();
        Shapes shapeobj = new Shapes();
        int keyboardTick = 0;
        /// <summary>
        /// Keyboard input buffer to prevent button spamming but also allows for smoother 
        /// directional movement. 
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        public bool KeyboardTimer(int timer)
        {
            if (keyboardTick == timer)
            {
                keyboardTick = 0;
                return true;
            }
            keyboardTick++;
            return false;
        }
    }
 }
