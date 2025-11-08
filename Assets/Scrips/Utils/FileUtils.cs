using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class FileUtils
{
    public static readonly string fileExtention = ".json";

    public static void TrySaveFile(string folderPath, string filename, object contents)
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var filePath = Path.Combine(folderPath, filename);
        var filePathWithExtention = filePath + fileExtention;

        var jsonString = JsonConvert.SerializeObject(contents);
        File.WriteAllText(filePathWithExtention, jsonString);
        try
        {
            File.WriteAllText(filePath, jsonString);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save file at {filePathWithExtention}. Exception: {e}");
        }
    }

    public static string[] GetFilesInAppDataFolder(string folderName)
    {
        // path to the folder in appData
        // folderName could be "Levels" or "GameState"
        // path looks like: C:/Users/username/AppData/LocalLow/CompanyName/folderName
        var folderPath = Path.Combine(Application.persistentDataPath, folderName);

        try
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            return Directory.EnumerateFiles(folderPath).ToArray();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to get files in folder at {folderPath}. Exception: {e}");
            return new string[0];
        }
    }
}
