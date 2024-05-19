using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class PlayerDataSerializer
{
    private static readonly string savePath = Application.persistentDataPath + "/Saves/";
    private static readonly string saveFileName = "PlayerSaveData.data";
    public static void Save()
    {
        if (PlayerData.MatchData == null)
            Debug.LogError("Unable to save data, no player match data set");
        
        var data = PlayerData.MatchData;

        var jsonData = JsonUtility.ToJson(data);
        
        File.WriteAllText(savePath + saveFileName, jsonData);
    }
}

public class PlayerMatchData
{
    public int seedkey;
    public List<int> matchedCards;
    public int score;
    public int comboMultiplier;
    public string cardInfoID;
    public Vector2Int layoutInfo;
}
