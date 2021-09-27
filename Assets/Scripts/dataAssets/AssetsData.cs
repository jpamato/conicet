using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsData : DataLoader
{
    public List<Content> content;
    [HideInInspector] public Content activeContent;

    [System.Serializable]
    public class Content
    {
        public string name;
        public Sprite sprite;
        public AudioClip audioClip;
    }
    public override void Reset()
    {
        content.Clear();
    }
    public override void OnLoaded(List<SpreadsheetLoader.Line> d)
    {
        OnDataLoaded(content, d);
        base.OnLoaded(d);
    }
    public void SetContent(Content content)
    {
        activeContent = content;
    }
    public string GetAssetRealName(string _name)
    {
        string[] arr = _name.Split("-"[0]);
        if (arr.Length > 1)
            _name = arr[0];
        return _name;
    }
    public Content GetContent(string _name)
    {
        _name = GetAssetRealName(_name);
        foreach (Content content in content)
        {
            if(string.Equals(content.name, _name))
                return content;
        }

        Events.Log("No hay asset content para: " + _name);
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
                            contentLine.sprite = Resources.Load<Sprite>("assets/" + value) as Sprite;
                            string folderName = "audio";
                            if (Data.Instance.lang == Data.langs.QOM)
                                folderName = "audio_qom";
                            contentLine.audioClip = Resources.Load<AudioClip>("assets/" + folderName + "/" + value) as AudioClip;
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
