using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Aikman Ong - 816056118
/// Giancarlo Escolano - 813215631
/// COMPE361 Final Project
/// </summary>
namespace Tetris
{
    /// <summary>
    /// Class to contain the shapes we will be using in the game
    /// </summary>
    public class Shapes : Block
    {
        //Shape and Rotate lists
        List<int[,]> ShapeList = new List<int[,]>();
        List<int[,]> RotateList_T = new List<int[,]>();
        List<int[,]> RotateList_Z = new List<int[,]>();
        List<int[,]> RotateList_S = new List<int[,]>();
        List<int[,]> RotateList_L = new List<int[,]>();
        List<int[,]> RotateList_J = new List<int[,]>();
        List<int[,]> RotateList_Line = new List<int[,]>();
        List<int[,]> RotateList_Sq = new List<int[,]>();

        //List of colors to be used for tetris blocks
        List<Color> ColorList = new List<Color>();

        protected List<int[,]> rotate = new List<int[,]>();
        private bool rotatable = false;
        protected static Random rnd = new Random(DateTime.Now.Millisecond);




        protected int[,] shape = new int[4, 4];
        protected int[,] shape2 = new int[4, 4];
        protected int[,] rotated = new int[4, 4];
        protected int rotateIndex = 0;
        protected int rnum = 0;
        protected int currentShape = rnd.Next(1, 7);
        protected int nextShape = rnd.Next(1, 7);

        public Shapes()
        {
            //Add colors to color list
            ColorList.Add(Color.DarkOrange);
            ColorList.Add(Color.Cyan);
            ColorList.Add(Color.Yellow);
            ColorList.Add(Color.Blue);
            ColorList.Add(Color.MediumVioletRed);
            ColorList.Add(Color.LightGreen);
            ColorList.Add(Color.Purple);


            //T shape
            ShapeList.Add(new int[4, 4]{{0,1,0,0},
                                        {1,1,1,0},
                                        {0,0,0,0},
                                        {0,0,0,0} });
            //Z shape
            ShapeList.Add(new int[4, 4]{{2,2,0,0},
                                        {0,2,2,0},
                                        {0,0,0,0},
                                        {0,0,0,0} });

            //S shape
            ShapeList.Add(new int[4, 4]{{0,0,3,3},
                                        {0,3,3,0},
                                        {0,0,0,0},
                                        {0,0,0,0} });
            //L Shape
            ShapeList.Add(new int[4, 4]{{0,4,0,0},
                                        {0,4,0,0},
                                        {0,4,4,0},
                                        {0,0,0,0} });

            //J Shape
            ShapeList.Add(new int[4, 4]{{0,0,0,0},
                                        {5,0,0,0},
                                        {5,5,5,0},
                                        {0,0,0,0} });
            //Square Shape
            ShapeList.Add(new int[4, 4]{{0,0,0,0},
                                        {0,6,6,0},
                                        {0,6,6,0},
                                        {0,0,0,0} });
            //Line Shape
            ShapeList.Add(new int[4, 4]{{0,7,0,0},
                                        {0,7,0,0},
                                        {0,7,0,0},
                                        {0,7,0,0} });
            //T Shape Rotations 
            RotateList_T.Add(new int[4, 4]{{0,1,0,0},
                                          {1,1,1,0},
                                          {0,0,0,0},
                                          {0,0,0,0} });

            RotateList_T.Add(new int[4, 4]{{0,1,0,0},
                                          {0,1,1,0},
                                          {0,1,0,0},
                                          {0,0,0,0} });

            RotateList_T.Add(new int[4, 4]{{0,0,0,0},
                                          {1,1,1,0},
                                          {0,1,0,0},
                                          {0,0,0,0} });

            RotateList_T.Add(new int[4, 4]{{0,1,0,0},
                                          {1,1,0,0},
                                          {0,1,0,0},
                                          {0,0,0,0} });
            //Z Shape Rotations 
            RotateList_Z.Add(new int[4, 4]{{2,2,0,0},
                                        {0,2,2,0},
                                        {0,0,0,0},
                                        {0,0,0,0} });

            RotateList_Z.Add(new int[4, 4]{{0,0,2,0},
                                        {0,2,2,0},
                                        {0,2,0,0},
                                        {0,0,0,0} });

            RotateList_Z.Add(new int[4, 4]{{2,2,0,0},
                                        {0,2,2,0},
                                        {0,0,0,0},
                                        {0,0,0,0} });

            RotateList_Z.Add(new int[4, 4]{{0,0,2,0},
                                        {0,2,2,0},
                                        {0,2,0,0},
                                        {0,0,0,0} });

            //S Shape rotations
            RotateList_S.Add(new int[4, 4]{{0,0,3,3},
                                        {0,3,3,0},
                                        {0,0,0,0},
                                        {0,0,0,0} });

            RotateList_S.Add(new int[4, 4]{{0,3,0,0},
                                        {0,3,3,0},
                                        {0,0,3,0},
                                        {0,0,0,0} });

            RotateList_S.Add(new int[4, 4]{{0,0,3,3},
                                        {0,3,3,0},
                                        {0,0,0,0},
                                        {0,0,0,0} });

            RotateList_S.Add(new int[4, 4]{{0,3,0,0},
                                           {0,3,3,0},
                                           {0,0,3,0},
                                           {0,0,0,0} });

            //L Shape Rotations
            RotateList_L.Add(new int[4, 4]{{0,4,0,0},
                                           {0,4,0,0},
                                           {0,4,4,0},
                                           {0,0,0,0} });

            RotateList_L.Add(new int[4, 4]{{0,0,0,0},
                                           {4,4,4,0},
                                           {4,0,0,0},
                                           {0,0,0,0} });

            RotateList_L.Add(new int[4, 4]{{4,4,0,0},
                                           {0,4,0,0},
                                           {0,4,0,0},
                                           {0,0,0,0} });

            RotateList_L.Add(new int[4, 4]{{0,0,4,0},
                                           {4,4,4,0},
                                           {0,0,0,0},
                                           {0,0,0,0} });

            //J Shape rotations 
            RotateList_J.Add(new int[4, 4]{{0,0,0,0},
                                        {5,0,0,0},
                                        {5,5,5,0},
                                        {0,0,0,0} });

            RotateList_J.Add(new int[4, 4]{{0,0,0,0},
                                        {0,5,5,0},
                                        {0,5,0,0},
                                        {0,5,0,0} });

            RotateList_J.Add(new int[4, 4]{{0,0,0,0},
                                        {0,0,0,0},
                                        {5,5,5,0},
                                        {0,0,5,0} });

            RotateList_J.Add(new int[4, 4]{{0,0,5,0},
                                        {0,0,5,0},
                                        {0,5,5,0},
                                        {0,0,0,0} });
            //Square Shape Rotations
            RotateList_Sq.Add(new int[4, 4]{{0,0,0,0},
                                        {0,6,6,0},
                                        {0,6,6,0},
                                        {0,0,0,0} });

            RotateList_Sq.Add(new int[4, 4]{{0,0,0,0},
                                        {0,6,6,0},
                                        {0,6,6,0},
                                        {0,0,0,0} });

            RotateList_Sq.Add(new int[4, 4]{{0,0,0,0},
                                        {0,6,6,0},
                                        {0,6,6,0},
                                        {0,0,0,0} });
            RotateList_Sq.Add(new int[4, 4]{{0,0,0,0},
                                        {0,6,6,0},
                                        {0,6,6,0},
                                        {0,0,0,0} });
            //Line Shape Rotations
            RotateList_Line.Add(new int[4, 4]{{0,7,0,0},
                                        {0,7,0,0},
                                        {0,7,0,0},
                                        {0,7,0,0} });

            RotateList_Line.Add(new int[4, 4]{{0,0,0,0},
                                        {7,7,7,7},
                                        {0,0,0,0},
                                        {0,0,0,0} });

            RotateList_Line.Add(new int[4, 4]{{0,7,0,0},
                                        {0,7,0,0},
                                        {0,7,0,0},
                                        {0,7,0,0} });

            RotateList_Line.Add(new int[4, 4]{{0,0,0,0},
                                        {7,7,7,7},
                                        {0,0,0,0},
                                        {0,0,0,0} });



        }

        /// <summary>
        /// Gets ShapeList 
        /// </summary>
        /// <returns></returns>
        public List<int[,]> GetShapeList()
        {
            return ShapeList;
        }
        /// <summary>
        /// Gets Color list
        /// </summary>
        /// <returns></returns>
        public List<Color> GetColorList()
        {
            return ColorList;
        }
        /// <summary>
        /// Gets Rotate List for shape T
        /// </summary>
        /// <returns></returns>
        public List<int[,]> GetRotate_T()
        {
            return RotateList_T; 
        }
        /// <summary>
        /// Gets rotate list for Z
        /// </summary>
        /// <returns></returns>
        public List<int[,]> GetRotate_Z()
        {
            return RotateList_Z;
        }
        public List<int[,]> GetRotate_S()
        {
            return RotateList_S;
        }
        /// <summary>
        /// Gets rotate list for L 
        /// </summary>
        /// <returns></returns>
        public List<int[,]> GetRotate_L()
        {
            return RotateList_L;
        }
        /// <summary>
        /// Gets rotate list for J
        /// </summary>
        /// <returns></returns>
        public List<int[,]> GetRotate_J()
        {
            return RotateList_J;
        }
        /// <summary>
        /// Gets rotate list for line
        /// </summary>
        /// <returns></returns>
        public List<int[,]> GetRotate_Line()
        {
            return RotateList_Line;
        }
        /// <summary>
        /// Gets rotate list for square
        /// </summary>
        /// <returns></returns>
        public List<int[,]> GetRotate_Sq()
        {
            return RotateList_Sq;
        }
        /// <summary>
        /// Checks if a shape is rotatable 
        /// </summary>
        public bool Rotatable
        {
            get { return rotatable; }
            set { rotatable = value; }
        }
        /// <summary>
        /// Returns the corresponding rotate list for the current shape 
        /// </summary>
        /// <param name="currentShape"></param>
        /// <returns></returns>
        public List<int[,]> Rotate(int currentShape)
        {
            switch (currentShape)
            {
                case 0:
                    rotate = GetRotate_T();
                    break;
                case 1:
                    rotate = GetRotate_Z();
                    break;
                case 2:
                    rotate = GetRotate_S();
                    break;
                case 3:
                    rotate = GetRotate_L();
                    break;
                case 4:
                    rotate = GetRotate_J();
                    break;
                case 5:
                    rotate = GetRotate_Sq();
                    break;
                case 6:
                    rotate = GetRotate_Line();
                    break;
                default:
                    break;
            }
            return rotate; 
        }


    }
}
