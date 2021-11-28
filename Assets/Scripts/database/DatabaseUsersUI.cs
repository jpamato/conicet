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

    void Start()
    {
        databaseData = GetComponent<DatabaseData>();
        databaseUserAdd = GetComponent<DatabaseUserAdd>();
        databaseUserAdd.Close();
        Open();
        RefreshList();
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
        PlayerPrefs.DeleteAll();
        databaseData.DeleteAll();
        Close();
    }
}
