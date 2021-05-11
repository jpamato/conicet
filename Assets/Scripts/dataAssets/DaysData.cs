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

    public override void Reset() {
        content.Clear();
    }
    public override void OnLoaded(List<SpreadsheetLoader.Line> d)
    {
        OnDataLoaded(content, d);
        base.OnLoaded(d);
    }
    public void SetActivityComplete(int gameId)
    {
        if(gameId<= activeContent.games.Count-1)
            activeContent.games[gameId].SetPlayed(true);
        Data.Instance.userData.SetSavedData(Data.Instance.lang.ToString()+"_"+Data.Instance.daysData.activeContent.day+"_"+gameId, 1);
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
        GameData gameData = new GameData();
        foreach (SpreadsheetLoader.Line line in d)
        {
            foreach (string value in line.data)
            {
                //print("row: " + rowID + "  colID: " + colID + "  value: " + value);
                if (rowID >= 1)
                {
                    if (colID == 0)
                    {
                        gameData = new GameData();
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
                            gameData.type = (GameData.types)System.Enum.Parse(typeof(GameData.types), value);
                            gameData.gameID = GetGameID(contentLine.games, gameData.type);

                            string savedValue = Data.Instance.lang.ToString() + "_" + contentLine.day + "_" + contentLine.games.Count;
                            int playedID = Data.Instance.userData.GetValue(savedValue);
                            if (playedID > 0) gameData.SetPlayed(true);

                            if (Data.Instance.DEBUG) gameData.SetPlayed(true);
                            contentLine.games.Add(gameData);
                        }
                        else if (colID == 3 && value != "")
                        {
                                gameData.tip_id = value;
                        }
                    }
                }
                colID++;
            }
            colID = 0;
            rowID++;
        }

    }
    int GetGameID(List<GameData> arr, GameData.types gameType)
    {
        int qty = 1;
        foreach (GameData c in arr)
            if (c.type == gameType)
                qty++;
        if (qty == 1)
            return 0;
        else
            return qty;
    }
    public TextsData.Content GetTip(string tip)
    {
        bool ignoreLang = true;

        if (Data.Instance.lang == Data.langs.QOM) ignoreLang = false;

        string specialTip = Data.Instance.daysData.activeContent.games[Data.Instance.userData.activityID].tip_id;
        if (specialTip != null && specialTip.Length > 2) tip = specialTip;
        TextsData.Content content = Data.Instance.textsData.GetContent(tip, ignoreLang);
        if (content == null)
            Events.Log("Falta Tip para: " + tip);
        return content;
    }
    public bool IsLastDay()
    {
        int dayID = activeContent.day;
        string storyID = activeContent.story_id;

        string[] arr = activeContent.story_id.Split(":"[0]);
        if (arr.Length > 1)
            storyID = arr[0];

        bool isLastDay = false; 
        foreach(Content c in content)
        {
            arr = c.story_id.Split(":"[0]);
            string _storyId = c.story_id;
            if(arr.Length>1)
                _storyId = arr[0];
            print("_storyId: " + _storyId + "storyID: " + storyID + " c.day: " + c.day + " dayID: " + dayID);

            if (storyID == _storyId)
            {
                if (dayID == c.day )
                    isLastDay = true;
                else
                    isLastDay = false;
            }
        }
        return isLastDay;
    }
}
