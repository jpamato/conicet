using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DatabaseManager : MonoBehaviour
{
    string hashPassword = "pontura";
    string url = "http://localhost/conicet/";

    public void SaveUser(DatabaseUser userdata)
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
        StartCoroutine(RequestUser(urlReal, OnUserDataSaved));
    }
    void OnUserDataSaved(string result)
    {
        print("result " + result);
    }
    IEnumerator RequestUser(string url, System.Action<string> OnDone)
    {
        print(url);
        WWW www = new WWW(url);
        yield return www;
        if (www.error == null)
        {
            if (OnDone != null)
                OnDone(www.text);
        }
        else
        {
            Debug.Log("ERROR: " + www.error);
        }
    }
}
