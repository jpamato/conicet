using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoriesData : MonoBehaviour
{
    string url = "https://docs.google.com/spreadsheets/d/e/2PACX-1vTTgCbeSdQrchfjXqLW0-wWZHOS36UtJ7EAuEakSL91Y4PnRZs1hHhSnLcesFU18UcoA97eyAMAVoqM/pub?gid=0&single=true&output=tsv";
    public List<Content> content;
    [HideInInspector] public Content activeContent;

    [System.Serializable]
    public class Content
    {
        public string id;
        public string name;
        public string folder;
        public AudioClip audioClip;
        public List<TimelineTextData> textsData;
    }   
    void Start()
    {
        Data.Instance.spreadsheetLoader.LoadFromTo(url, OnLoaded);
    }
    void OnLoaded(List<SpreadsheetLoader.Line> d)
    {
        OnDataLoaded(content, d);
    }
    public Content GetContent(string story_id)
    {
        foreach (Content c in content)
            if (c.id == story_id)
                return c;
        return null;
    }
    public void SetContent(Content content)
    {
        activeContent = content;
    }
    void OnDataLoaded(List<Content> content, List<SpreadsheetLoader.Line> d)
    {
        int colID = 0;
        int rowID = 0;
        Content contentLine = null;
        TimelineTextData textData = null;
        foreach (SpreadsheetLoader.Line line in d)
        {
            foreach (string value in line.data)
            {
               // print("row: " + rowID + "  colID: " + colID + "  value: " + value);
                if (rowID >= 1)
                {
                    if (colID == 0)
                    {
                        if (value != "") // si está vacia la accion usa la anterior:
                        {
                            contentLine = new Content();
                            contentLine.id = value;
                           
                            contentLine.textsData = new List<TimelineTextData>();

                            content.Add(contentLine);
                        }
                    }
                    else
                    {
                        if (colID == 1 && value != "")
                            contentLine.name = value;                       
                        else if (colID == 2 && value != "")
                        {
                            contentLine.folder = value;
                        }
                        else if (colID == 3 && value != "")
                        {
                            string url = "stories/" + contentLine.folder + "/audios/" + value;
                            print(url);
                            contentLine.audioClip = Resources.Load<AudioClip>(url);
                        }
                        else if (colID == 4 && value != "")
                        {
                            textData = new TimelineTextData();
                            textData.text = value;
                        }
                        else if (colID == 5)
                        {
                            if(value != "")
                                textData.seconds = Utils.GetTotalSecondsFromString(value);
                            contentLine.textsData.Add(textData);
                            //print("contentLine: " + contentLine.textsData.Count);
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
