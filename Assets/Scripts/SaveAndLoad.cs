using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// for serialization and deserialization generic type objects
/// </summary>
public static class SaveAndLoad
{
    public static void Save(string savePath, object serializeObject)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Create(Application.persistentDataPath + savePath);
        binaryFormatter.Serialize(fileStream, serializeObject);
        fileStream.Close();
    }

    public static T Load<T>(string savePath)
    {
        if (File.Exists(Application.persistentDataPath + savePath))
        {
            FileStream fileStream = File.Open(Application.persistentDataPath + savePath, FileMode.Open);

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            object loadObject = binaryFormatter.Deserialize(fileStream);

            fileStream.Close();

            return (T)loadObject;
        }
        else
        {
            return default(T);
        }
    }
}
