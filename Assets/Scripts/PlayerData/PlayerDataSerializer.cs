using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class PlayerDataSerializer
{
    private static readonly string savePath = Application.persistentDataPath + "/Saves/";
    private static readonly string saveFileName = "PlayerSaveData.data";
    public static void Save()
    {
        if (PlayerData.MatchData == null)
        {
            Debug.LogError("Unable to save data, no player match data set");
            return;
        }
        
        var data = PlayerData.MatchData;
        var jsonData = JsonUtility.ToJson(data);

        Directory.CreateDirectory(savePath);
        File.WriteAllText(savePath + saveFileName, jsonData, Encoding.Default);
    }

    public static void Load()
    {
        if (!PlayerHasData()) return;
        
        var jsonData = File.ReadAllText(savePath + saveFileName, Encoding.Default);
        PlayerData.MatchData = JsonUtility.FromJson<PlayerMatchData>(jsonData);
    }

    public static void DeleteSaveData()
    {
        if (PlayerHasData())
            File.Delete(savePath + saveFileName);
    }

    public static bool PlayerHasData() => File.Exists(savePath + saveFileName);
}

public class PlayerMatchData
{
    public int seedKey;
    public List<int> matchedCards;
    public int score;
    public int comboMultiplier;
    public string cardInfoID;
    public Vector2Int layoutInfo;
}
