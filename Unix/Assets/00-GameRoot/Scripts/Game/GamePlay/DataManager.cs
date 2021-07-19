using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[Serializable]
public class PlayState 
{
    public PlayState(string characterID, string tileID, int redTeamScore, int blueTeamScore)
    {
        this.characterID = characterID;
        this.tileID = tileID;
        this.redTeamScore = redTeamScore;
        this.blueTeamScore = blueTeamScore;
    }
    public string characterID;
    public string tileID;

    public int redTeamScore;
    public int blueTeamScore;
}
[Serializable]
public class CurrentGameData
{
    public char winner;
    public List<PlayState> playStates = new List<PlayState>();
}

[Serializable]
public class HistoricGameData
{
    public List<CurrentGameData> gameData;
}

/// <summary>
/// Data Manager
/// </summary>
public class DataManager
{
    string path;
    const string DIRECTORY = "/Data/";
    const string FILE_NAME = "HistoricGameData.json";

    public void Save()
    { 
        path = Application.dataPath;
        string dir = path + DIRECTORY;
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        HistoricGameData history = GameData.historicGameData;

        if (history == null)                    //if there's no file 
        {
            history = new HistoricGameData();
            history.gameData = new List<CurrentGameData>();
        }

        history.gameData.Add(GameData.currentGameData);
        string json = JsonUtility.ToJson(history);
        File.WriteAllText(dir + FILE_NAME, json);

        Debug.Log("saved");
    }

    public HistoricGameData LoadFile() //anyone can call this = anyone can speak
    {
        string filePath = Application.dataPath + DIRECTORY + FILE_NAME;
        HistoricGameData historicGameData = new HistoricGameData();

        if (File.Exists(filePath)) //is there a text asset?
        {
            string json = File.ReadAllText(filePath);
            historicGameData = JsonUtility.FromJson<HistoricGameData>(json); // checking if the file should be loaded into a graph or a linked list      
            return historicGameData;

        }
        else
        {
            Debug.Log("No json file found");
            return null;
        }
        

    }
}
