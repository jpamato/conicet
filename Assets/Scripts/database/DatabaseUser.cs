using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DatabaseUser
{
    public int saved;
    public string id;
    public string name;
    public int age;
    public string text;
    public string gender; // nene, nena
    public List<DatabaseUserGame> games;
    public List<DatabaseUserWords> words;
    public int arrayID;

    public void GenerateID()
    {
        id = DateTime.Today.Year.ToString();
        id += DateTime.Today.Month.ToString();
        id += DateTime.Today.Day.ToString();
        id += DateTime.Today.Hour.ToString();
        id += DateTime.Today.Minute.ToString();
        id += DateTime.Today.Second.ToString();
        id += DateTime.Today.Millisecond.ToString();
        id += (UnityEngine.Random.Range(0, 1000)).ToString();
    }
    public void SavedToDatabase()
    {
        saved = 1;
        Save(arrayID);
        Events.OnUpdateDatabaseUserData(this);        
    }
    public void Save(int arrayID)
    {
        this.arrayID = arrayID;
        string newUser = saved.ToString();
        newUser += ":" + id;
        newUser += ":" + name;
        newUser += ":" + age;
        newUser += ":" + text;
        newUser += ":" + gender;
        string key = "user" + arrayID;
        PlayerPrefs.SetString(key, newUser);
    }
    public bool IsSavedToDatabase()
    {
        if (saved == 1)
            return true;
        return false;
    }
    public int GetTotalGames()
    {
        if (games == null)
            return 0;
        return games.Count;
    }
    public int GetAllWordsInGames()
    {
        if (games == null)
            return 0;
        int total = 0;
        foreach (DatabaseUserGame game in games)
            total += words.Count;
        return total;
    }
    public void AddGame(DatabaseUserGame gameData)
    {
        if(games == null)
            games = new List<DatabaseUserGame>();
        games.Add(gameData);
    }
    public void AddWord(DatabaseUserWords wordData)
    {
        if (words == null)
            words = new List<DatabaseUserWords>();
        words.Add(wordData);
    }
}
