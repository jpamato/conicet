using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextsData : MonoBehaviour
{
    public string url = "https://docs.google.com/spreadsheets/d/e/2PACX-1vTTgCbeSdQrchfjXqLW0-wWZHOS36UtJ7EAuEakSL91Y4PnRZs1hHhSnLcesFU18UcoA97eyAMAVoqM/pub?gid=793140258&single=true&output=tsv";
    public List<Content> content;
    [HideInInspector] public Content activeContent;

    [System.Serializable]
    public class Content
    {
        public string id;
        public string text;
    }
    void Start()
    {
        Data.Instance.spreadsheetLoader.LoadFromTo(url, OnLoaded);
    }
    void OnLoaded(List<SpreadsheetLoader.Line> d)
    {
        OnDataLoaded(content, d);
    }
    public void SetContent(Content content)
    {
        activeContent = content;
    }
    public string GetText(string id)
    {
        Content c =  content.Find((x) => x.id == id);
        if (c == null) Debug.Log("Error: No existe TextData para " + id);
        return c.text;
    }
    public Content GetContent(string id)
    {
        return content.Find((x) => x.id == id);
    }
    void OnDataLoaded(List<Content> content, List<SpreadsheetLoader.Line> d)
    {
        int colID = 0;
        int rowID = 0;
        Content contentLine = null;
        foreach (SpreadsheetLoader.Line line in d)
        {
            foreach (string value in line.data)
            {
                //print("row: " + rowID + "  colID: " + colID + "  value: " + value);
                if (rowID >= 1)
                {
                    if (colID == 0)
                    {
                        if (value != "") // si está vacia la accion usa la anterior:
                        {
                            contentLine = new Content();
                            contentLine.id = value;
                            content.Add(contentLine);
                        }
                    }
                    else
                    {
                        if (colID == 1 && value != "")
                            contentLine.text = value;
                    }
                }
                colID++;
            }
            colID = 0;
            rowID++;
        }

    }
}
