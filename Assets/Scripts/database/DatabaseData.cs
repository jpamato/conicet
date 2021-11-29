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
            string userDataParsed = PlayerPrefs.GetString(key, "");
            if (userDataParsed == "")
                return;
            DatabaseUser user = new DatabaseUser();
            users.Add(user);
            string[] arr = userDataParsed.Split(":"[0]);
            if (arr.Length > 0)
            {

                if (arr[0] == null || arr[0].Length < 1)
                    user.saved = 0;
                else
                    user.saved = int.Parse(arr[0]);

                user.id = arr[1];
                user.name = arr[2];
                user.age = int.Parse(arr[3]);
                user.text = arr[4];
                user.gender = arr[5];
            }
        }
    }
    public void AddUser(DatabaseUser user)
    {
        users.Insert(0,user);
        string newUser = "0";
        newUser += ":" + user.id;
        newUser += ":" + user.name;
        newUser += ":" + user.age;
        newUser += ":" + user.text;
        newUser += ":" + user.gender;
        string key = "user" + users.Count;
        print("AddUser key: " + key + " value: " + newUser);
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
