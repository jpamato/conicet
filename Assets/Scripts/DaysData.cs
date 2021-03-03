using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaysData : DataLoader
{
    public List<Content> content;
    // [HideInInspector] 
    public Content activeContent;

    [System.Serializable]
    public class Content
    {
        public int day;
        public types type;
        public enum types
        {
            INACTIVE,
            ACTIVE,
            DONE
        }
        public string story_id;
        public List<GameData> games;
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
                //print("row: " + rowID + "  colID: " + colID + "  value: " + value);
                if (rowID >= 1)
                {
                    if (colID == 0)
                    {
                        if (value != "") // si está vacia la accion usa la anterior:
                        {
                            contentLine = new Content();
                            contentLine.day = int.Parse(value);
                            contentLine.games = new List<GameData>();
                            content.Add(contentLine);
                        }
                    }
                    else
                    {
                        if (colID == 1 && value != "")
                            contentLine.story_id = value;
                        else if (colID == 2 && value != "")
                        {
                            GameData gameData = new GameData();
                            gameData.type = (GameData.types)System.Enum.Parse(typeof(GameData.types), value);
                            contentLine.games.Add(gameData);
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
