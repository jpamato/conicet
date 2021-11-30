using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DatabaseUsersUI : MonoBehaviour
{
    [SerializeField] DatabaseUserButton button;
    [SerializeField] GameObject panel;

    [SerializeField] Transform container;
    public DatabaseUser active;
    DatabaseUserAdd databaseUserAdd;
    DatabaseData databaseData;
    DatabaseManager databaseManager;

    void Start()
    {
        databaseManager = GetComponent<DatabaseManager>();
        databaseData = GetComponent<DatabaseData>();
        databaseUserAdd = GetComponent<DatabaseUserAdd>();
        databaseUserAdd.Close();
        Open();
        RefreshList();
        Events.OnStatsGameDone += OnStatsGameDone;
        Events.OnStatsAddWord += OnStatsAddWord;
    }
    public void Open()
    {
        panel.SetActive(true);
    }
    public void Close()
    {
        panel.SetActive(false);
    }
    public void SelectUser(DatabaseUser user)
    {
        active = user;
        Close();
    }
    public void Add()
    {
        databaseUserAdd.Init();
    }
    public void AddNewUser(DatabaseUser user)
    {
        active = user;
        databaseData.AddUser(user);
        RefreshList();
    }
    void RefreshList()
    {
        Utils.RemoveAllChildsIn(container);
        foreach(DatabaseUser user in databaseData.users)
        {
            DatabaseUserButton b = Instantiate(button, container);
            b.Init(user);
        }
    }
    public void Save()
    {
        int id = 0;
        foreach (DatabaseUser user in databaseData.users)
        {
            if(!user.IsSavedToDatabase())
                databaseManager.SaveUser(user, user.SavedToDatabase);
            id++;
        }
    }
    DatabaseUser UserActive()
    {
        foreach (DatabaseUser user in databaseData.users)
        {
            if (user.id == active.id)
                return user;
        }
        return null;
    }
    // <GameData.types, int, int, int> OnStatsGameDone = delegate { };
    void OnStatsGameDone(GameData.types type, int duration, int correct, int incorrect)
    {
        DatabaseUserGame dbUserGame = new DatabaseUserGame();
        dbUserGame.gameID = type.ToString();
        dbUserGame.correct = correct;
        dbUserGame.incorrect = incorrect;
        dbUserGame.duration = duration;

        DatabaseUser userActive = UserActive();

        switch (type)
        {
            case GameData.types.memotest:
                userActive.AddGame(dbUserGame);
                break;
        }
    }
    
    void OnStatsAddWord(GameData.types type, string word, bool isCorrect)
    {
        DatabaseUserWords dbUserData = new DatabaseUserWords();
        dbUserData.gameID = type.ToString();
        if(isCorrect)
            dbUserData.correct = 1;
        dbUserData.word = word;
        DatabaseUser userActive = UserActive();
        userActive.AddWord(dbUserData);
    }

}
