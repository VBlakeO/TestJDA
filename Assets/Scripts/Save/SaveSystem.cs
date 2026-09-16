using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;

public static class SaveSystem
{
    #region MainSave
    public static void SaveMainGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        Directory.CreateDirectory(Application.persistentDataPath + "/saves");
        string path = Application.persistentDataPath + "/saves/Game.txt";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveGameData data = new SaveGameData();
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveGameData LoadMainGame() 
    {
        string path = Application.persistentDataPath + "/saves/Game.txt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveGameData data = formatter.Deserialize(stream) as SaveGameData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save not found in " + path);
            return null;
        }
    }

    public static bool MainSaveExists()
    {
        string path = Application.persistentDataPath + "/saves/Game.txt";
        return File.Exists(path);
    }
    #endregion 
}