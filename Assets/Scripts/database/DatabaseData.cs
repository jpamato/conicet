using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseData : MonoBehaviour
{
    public List<DatabaseUser> users;

    private void Awake()
    {
        LoadUsers();
    }
    void LoadUsers()
    {
        for (int a = 1; a < 1000; a++)
        {
            string key = "user" + a;
            print("key: " + key);
            string userDataParsed = PlayerPrefs.GetString(key, "");
            print("userDataParsed_ " + userDataParsed);
            if (userDataParsed == "")
                return;
            DatabaseUser user = new DatabaseUser();
            users.Add(user);
            string[] arr = userDataParsed.Split(":"[0]);
            user.id = arr[0];
            user.name = arr[1];
            user.age = int.Parse(arr[2]);
            user.text = arr[3];
        }
    }
    public void AddUser(DatabaseUser user)
    {
        users.Insert(0,user);
        string newUser = user.id;
        newUser += ":" + user.name;
        newUser += ":" + user.age;
        newUser += ":" + user.text;
        string key = "user" + users.Count;
        print("key: " + key + " value: " + newUser);
        PlayerPrefs.SetString(key, newUser);
    }
    public void Delete(DatabaseUser user)
    {
        users.Remove(user);
    }
    public void DeleteAll()
    {
        users.Clear();
    }
}
