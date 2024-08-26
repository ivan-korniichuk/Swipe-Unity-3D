using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public static class SaveGameData 
{
    public static void SaveData(int level, int health, int diamonds, int maxLevel, Dictionary<string, bool[]> levelsDiamondsStatus)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/game.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(level, health, diamonds, maxLevel, levelsDiamondsStatus);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData LoadData()
    {
        string path = Application.persistentDataPath + "/game.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();

            return data;
        } 
        else
        {
            Debug.LogError("Save file not found");
            return null;
        }
    }
}
