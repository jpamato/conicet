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
        public string text;
        public Sprite sprite;
        public AudioClip audioClip;
    }
    public enum loroWordsType
    {
        DEFAULT,
        MEDIO,
        INITIAL,
        FINAL
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
    public string GetSoundForLoro(string _name, loroWordsType loroType)
    {
        if (loroType == loroWordsType.MEDIO)
            return _name + "_medio";
        else if (loroType == loroWordsType.INITIAL)
            return _name + "_inicio";
        else if (loroType == loroWordsType.FINAL)
            return _name + "_final";
        else
            return _name;
    }
    public Content GetContent(string __name)
    {
        string _name = GetAssetRealName(__name.ToLower());
        if(__name == _name)  _name = GetAssetRealName(__name);

        foreach (Content content in content)
        {
            if(string.Equals(content.name, _name))
                return content;
        }

        Events.Log("No hay asset content para: " + _name);
        return null;
    }
    public loroWordsType SetTypeByText(string text)
    {
        switch (text)
        {
            case "final": return AssetsData.loroWordsType.FINAL;
            case "inicio": return AssetsData.loroWordsType.INITIAL;
            case "medio": return AssetsData.loroWordsType.MEDIO;
            default: return AssetsData.loroWordsType.DEFAULT;
        }
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
                            contentLine.name = value;
                            contentLine.sprite = Resources.Load<Sprite>("assets/" + value) as Sprite;
                            string folderName = "audio";
                            if (Data.Instance.lang == Data.langs.QOM)
                                folderName = "audio_qom";
                            contentLine.audioClip = Resources.Load<AudioClip>("assets/" + folderName + "/" + value) as AudioClip;
                            content.Add(contentLine);
                        }
                    }
                    if (colID == 3 && Data.Instance.lang != Data.langs.QOM)
                    {
                        if (value != null && value.Length < 2)
                            contentLine.text = contentLine.name;
                        else
                            contentLine.text = value;
                    }
                    if (colID == 4 && Data.Instance.lang == Data.langs.QOM)
                    {
                        if (value != null && value.Length < 2)
                            contentLine.text = contentLine.name;
                        else
                            contentLine.text = value;
                    }
                }
                colID++;
            }
            colID = 0;
            rowID++;
        }

    }
    public string GetRealText(string textName)
    {
        foreach(Content c in content)
        {
            if (c.name == textName)
                return c.text;
        }
        Debug.LogError("No hay traducción para " + textName + " en assetsData");
        return textName;
    }
}
