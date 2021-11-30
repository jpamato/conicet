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
        user.Save(users.Count);
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
