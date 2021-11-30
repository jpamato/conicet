using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DatabaseManager : MonoBehaviour
{
    string hashPassword = "pontura";
    string url = "http://localhost/conicet/";

    public void SaveUser(DatabaseUser userdata, System.Action OnSaved)
    {
        string hash = Md5Test.Md5Sum(userdata.id + hashPassword);
        string urlReal = url
            + "setUser.php?name=" + userdata.name
            + "&age=" + userdata.age
            + "&gender=" + userdata.gender
            + "&text=" + userdata.text
            + "&userID=" + userdata.id
            + "&hash=" + hash;
        print("save: " + urlReal);
        StartCoroutine(RequestUser(urlReal, OnSaved));
    }
    IEnumerator RequestUser(string url, System.Action OnSaved)
    {
        print(url);

        WWW www = new WWW(url);
        yield return www;
        print("SAVE response: " + www.text);
        if (www.text == "ok")
        {
            Debug.Log("SAVED: " + www.text);
            OnSaved();
        }
        else
        {
            Debug.Log("ERROR: " + www.error);
        }
    }
}
