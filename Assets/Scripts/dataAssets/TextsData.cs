using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextsData : DataLoader
{
    public List<Content> content;
    [HideInInspector] public Content activeContent;

    [System.Serializable]
    public class Content
    {
        public string id;
        public string text;
        public CharactersManager.types character_type;
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
    public string GetText(string id, Data.langs lang)
    {
        Content c =  content.Find((x) => x.id == id);
        if (c == null)
        {
            return GetText(id, Data.Instance.lang);
            Debug.Log("Error: No existe TextData para " + id);
        }
        return c.text;
    }
    public Content GetContent(string id, bool ignoreLang = false)
    {
        string textID = id;
        if(!ignoreLang && Data.Instance.lang == Data.langs.QOM)  textID = "qom_" + textID;
        //else if (!ignoreLang && Data.Instance.lang == Data.langs.L1) textID = "l1_" + textID;

        if (ignoreLang)
            print("Get ID: " + id + "    -> textID: " + textID + " ignoreLang: " + ignoreLang);
        else
            print("Get ID: " + id + "    -> textID: " + textID);// + "-" + Data.Instance.storiesData.activeContent.id);

        Content tipContent = content.Find((x) => x.id == textID);// + "-" + Data.Instance.storiesData.activeContent.id);
        if (tipContent != null)
            return tipContent;

        return content.Find((x) => x.id == textID);
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

                            string idText = value;
                            if (Data.Instance.lang == Data.langs.QOM)
                                idText = "qom_" + idText;
                         // else if (Data.Instance.lang == Data.langs.L1)
                           //   idText = "l1_" + idText;
                            contentLine.id = idText;
                            content.Add(contentLine);
                        }
                    }
                    else
                    {
                        if (colID == 1 && value != "")
                            contentLine.character_type = (CharactersManager.types)System.Enum.Parse(typeof(CharactersManager.types), value);
                        if (colID == 2 && value != "")
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
