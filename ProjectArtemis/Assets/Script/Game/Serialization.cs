using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Serialization
{
    public static void Save(object saveObj, string fileName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(fileName, FileMode.Create);

        bf.Serialize(stream, saveObj);
        stream.Close();
        Debug.Log($"<color=cyan>File {fileName} = Saved</color>");
    }

    public static object Load(string fileName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(fileName, FileMode.Open);
        object result = bf.Deserialize(stream);
        stream.Close();
        Debug.Log($"<color=cyan>File {fileName} = Loaded</color>");

        return result;
    }

    public static bool FileExistenceCheck(string fileName)
    {
        bool fileExists = File.Exists(fileName);
        if (!fileExists) Debug.LogError($"FILE {fileName} DOES NOT EXIST");
        return fileExists; 
    }

    public static void DeleteFile(string fileName)
    {
        File.Delete(fileName);
    }

    public static void SaveText(string text, string filename)
    {
        File.WriteAllText(filename, text);
        Debug.Log($"<color=cyan>File {filename} = Saved</color>");
    }

    public static List<string> LoadTextByLine(string filename)
    {
        var content = new List<string>();

        foreach (var line in File.ReadAllLines(filename))
        {
            content.Add(line);
        }

        Debug.Log($"<color=cyan>File {filename} = Loaded</color>");
        return content;
    }

    public static string LoadTextAll(string filename)
    {
        var content = File.ReadAllText(filename);
        return content;
    }   
}
