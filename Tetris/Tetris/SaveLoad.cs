using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO; 

namespace Tetris
{
    class SaveLoad
    {
        public void Save(int[,] gameboard)
        {
            string path = @"SavedGame.txt"; 
            JsonSerializer serializer = new JsonSerializer();

            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        serializer.Serialize(writer, gameboard);
                    }
                }
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(path))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, gameboard);
                }
            }
        }
    }
}
