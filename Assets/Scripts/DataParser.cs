using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public static class DataParser {
    //private static string _LevelDataPath = "Assets/Resources/Data/levels.json";
    private static string _LevelDataPath = Application.streamingAssetsPath + "/levels.json";

    public static string ReadLevelData() {
        StreamReader reader = new StreamReader(_LevelDataPath);
        string leveldata = reader.ReadToEnd();
        reader.Close();
        return leveldata;
    }
    public static LevelData CreateFromJSON() {
        string leveldata = DataParser.ReadLevelData();
        return JsonUtility.FromJson<LevelData>(leveldata);
    }
}

[System.Serializable]
public class LevelData {
    public List<LevelInfo> levels;
}

[System.Serializable]
public class LevelInfo {
    public int id;
    public List<LevelItem> items;
    public override string ToString()
    {
        string ans = $"{id}\n";
        foreach (var item in items) {
            ans = ans + item.ToString();
        }
        return ans;
    }
}

[System.Serializable]
public class LevelItem {
    public string type;
    public int x,y;
    public string family;
    public string direction;

    public override string ToString()
    {
        return $"{x},{y},{type},{family},{direction}";
    }
}