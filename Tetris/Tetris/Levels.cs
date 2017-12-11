using System;
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
    /// <summary>
    /// class is used to calculate the level we are on, how many lines are cleared, score, and the timer to vary the fall speed of the blocks
    /// </summary>
    public class Levels
    {
        

        /// <summary>
        /// Depending on which level we are on or how many lines we clear, the game will adjust the level
        /// </summary>
        /// <param name="level"></param>
        /// <param name="linesCleared"></param>
        /// <returns></returns>
        public int CalculateLevel(int level, int linesCleared)
        {
            if (level >=9)
            {
                level = 9;
            }
            else if (level == 9 || (linesCleared >= 160))
            {
                level = 9;
            }
            else if (level == 8 || (linesCleared >= 140 && linesCleared < 160))
            {
                level = 8;
            }
            else if (level == 7 || (linesCleared >= 120 && linesCleared < 140))
            {
                level = 7;
            }
            else if (level == 6 || (linesCleared >= 100 && linesCleared < 120))
            {
                level = 6;
            }
            else if (level == 5 || (linesCleared >= 80 && linesCleared < 100))
            {
                level = 5;
            }
            else if (level == 4 || (linesCleared >= 60 && linesCleared < 80))
            {
                level = 4;
            }
            else if (level == 3 || (linesCleared >= 40 && linesCleared <= 60))
            {
                level = 3;
            }
            else if (level == 2 || (linesCleared >= 20 && linesCleared < 40))
            {
                level = 2;
            }
            else if ( level ==1 || (linesCleared >= 0 && linesCleared < 20))
            {
                level = 1;
            }
            return level; 
        }

        /// <summary>
        /// Depending on which level we are on, the timer will vary
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int CalculateTimer(int level)
        {
            int timer;
            switch (level)
            {
                case 1:
                    timer = 60;
                    break;
                case 2:
                    timer = 50;
                    break;
                case 3:
                    timer = 40;
                    break;
                case 4:
                    timer = 35;
                    break;
                case 5:
                    timer = 30;
                    break;
                case 6:
                    timer = 25;
                    break;
                case 7:
                    timer = 20;
                    break;
                case 8:
                    timer = 15;
                    break;
                case 9:
                    timer = 10;
                    break;
                default: timer = 10;
                    break;
            }
            return timer;
        }

        /// <summary>
        /// Calculates the current score after every line completion 
        /// checks level if it needs to be incremented. 
        /// </summary>
        /// <param name="oldLines"></param>
        /// <param name="newLines"></param>
        /// <param name="level"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public int CalculateScore(int oldLines, int newLines, int level, int score)
        {
            switch (newLines - oldLines)
            {
                case 0:
                    score += 0;
                    break;
                case 1:
                    score += 40 * (level + 1);
                    break;
                case 2:
                    score += 100 * (level + 1);
                    break;
                case 3:
                    score += 300 * (level + 1);
                    break;
                case 4:
                    score += 1200 * (level + 1);
                    break;

            }

            return score; 
        }
    }
}
