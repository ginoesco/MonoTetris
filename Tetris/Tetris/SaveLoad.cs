using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Tetris
{
    public class SaveLoad
    {
        //File path and name of the saved game
        string path = @"SavedGame.txt";
        /// <summary>
        /// Saves the gameboard using json serializer. Serializes the array
        /// and stores to file. Allows user to save and quit. 
        /// </summary>
        /// <param name="gameboard"></param>
        public void Save(int[,] gameboard)
        {
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
        /// <summary>
        /// Loads the previously saved game by checking for the save file using 
        /// json deserializer. 
        /// </summary>
        /// <returns></returns>
        public int[,] Load()
        {
            int[,] deserial = JsonConvert.DeserializeObject<int[,]>(File.ReadAllText(path));
            return deserial;
        }
    }
}
