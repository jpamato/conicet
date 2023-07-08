using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

public class SpreadsheetLoader : MonoBehaviour
{
    [Serializable]
    public class Line
    {
        public string[] data;
    }
    public void LoadFromTo(string googleURL, System.Action<List<Line>> onDone, string filename = "")
    {
        StartCoroutine(GetData(googleURL, onDone, filename));
    }
    IEnumerator GetData(string url, System.Action<List<Line>> onDone, string filename = "")
    {
        using (UnityWebRequest request = new UnityWebRequest(url))
        {
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) {
                Debug.Log(request.error);
                if (!Data.Instance.cacheManager.IsSheetCached()) {
                    Events.OnLoading("No hay conexión a internet para la descarga inicial de los contenidos");
                    Invoke("Quit", 2);
                } else
                    LoadFromCache(onDone, filename);
                //onSuccess("error");
            } else {
                if (Data.Instance.predownloading)
                    StartCoroutine(Data.Instance.cacheManager.SaveSheetCached(filename, request.downloadHandler.text, () => CreateListFromFile(request.downloadHandler.text, onDone)));
                else {
                    CreateListFromFile(request.downloadHandler.text, onDone);
                    StartCoroutine(Data.Instance.cacheManager.SaveSheetCached(filename, request.downloadHandler.text, null));
                }
            }
        }
    }

    void LoadFromCache(System.Action<List<Line>> onDone, string filename) {
        Debug.Log("#LoadFromCache Sheets");
        CreateListFromFile(Data.Instance.cacheManager.GetSheet(filename), onDone);
    }

    public void CreateListFromArr(string[] lines, System.Action<List<Line>> onDone)
    {
        List<Line> arr = new List<Line>();
        foreach (string line in lines)
            arr.Add(ParseLine(line));

        onDone(arr);
    }
    public void CreateListFromFile(string text, System.Action<List<Line>> onDone)
    {
        Debug.Log("#CreateListFromFile");
        //Debug.Log(text);
        string[] lines = text.Split("\n"[0]);
        List<Line> arr = new List<Line>();
        foreach (string line in lines)
            arr.Add(ParseLine(line));

        onDone(arr);
    }
    public Line ParseLine(string lineData)
    {
        Line line = new Line();
        line.data = lineData.Split("\t"[0]);
        int id = 0;
        foreach (string s in line.data)
        {
            line.data[id] = ParseString(s);
            id++;
        }            
        return line;
    }
    string ParseString(string text)
    {
        return text.Replace("\r", "").Replace("\n", "");
    }
}
