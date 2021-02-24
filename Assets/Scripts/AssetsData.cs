using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsData : MonoBehaviour
{
    public string url = "https://docs.google.com/spreadsheets/d/e/2PACX-1vTTgCbeSdQrchfjXqLW0-wWZHOS36UtJ7EAuEakSL91Y4PnRZs1hHhSnLcesFU18UcoA97eyAMAVoqM/pub?gid=2054995855&single=true&output=tsv";
    public List<Content> content;
    [HideInInspector] public Content activeContent;

    [System.Serializable]
    public class Content
    {
        public string name;
        public Sprite sprite;
        public AudioClip audioClip;
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
    public Content GetContent(string _name)
    {
        foreach(Content content in content)
        {
            if(string.Equals(content.name, _name))
                return content;
        }
            
        Debug.Log("No hay asset content para: " + _name);
        return null;
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
                    if (colID == 1)
                    {
                        if (value != "")
                        {
                            contentLine = new Content();
                            contentLine.name =  value;
                            contentLine.sprite = Resources.Load<Sprite>("animals/" + value) as Sprite;
                            contentLine.audioClip = Resources.Load<AudioClip>("animals/" + value) as AudioClip;
                            content.Add(contentLine);
                        }
                    }
                }
                colID++;
            }
            colID = 0;
            rowID++;
        }

    }
}
