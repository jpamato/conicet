using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamesData : DataLoader
{
    public List<Content> content;
    [HideInInspector] public Content activeContent;

    [System.Serializable]
    public class Content
    {
        public string id;
        public List<string> memotest;
        public List<string> questions;
        public List<string> simons;
        public List<string> fallingObjects;
        public List<string> rimas;
        public List<string> loro_repeat;
        public List<string> loro_time;
        public List<string> memotestAudio;
        public List<string> GetContentFor(GameData.types gameType, int id)
        {
            switch(gameType)
            {
                case GameData.types.memotest: return GetTextsById(memotest, id);
                case GameData.types.questions: return GetTextsById(questions, id);

                case GameData.types.simon: return GetTextsById(simons, id);
                case GameData.types.falling_objects: return GetTextsById(fallingObjects, id);

                case GameData.types.rimas: return GetTextsById(rimas, id);
                case GameData.types.loro_repeat: return GetTextsById(loro_repeat, id);

                case GameData.types.memotest_audio: return GetTextsById(memotestAudio, id);
                default: return GetTextsById(memotestAudio, id);
            }
        }

        // agarra la lista por id de game (por si hay varios el mismo día:
        List<string> GetTextsById(List<string> arr, int id)
        {
            List<string> returnedArr = new List<string>();
            foreach (string s in arr)
            {
                string[] stringArr = s.Split(":"[0]);
                if (stringArr.Length==1 && id == 0)
                    returnedArr.Add(s);
                else if (stringArr.Length >1 && int.Parse(stringArr[1]) == id)
                    returnedArr.Add(stringArr[0]);
            }
            return returnedArr;
        }
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
    public Content GetContent(string storyID)
    {
        return content.Find((x) => x.id == storyID);
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

                            contentLine.memotest = new List<string>();
                            contentLine.questions = new List<string>();
                            contentLine.simons = new List<string>();
                            contentLine.fallingObjects = new List<string>();
                            contentLine.rimas = new List<string>();
                            contentLine.loro_repeat = new List<string>();
                            contentLine.loro_time = new List<string>();
                            contentLine.memotestAudio = new List<string>();


                            content.Add(contentLine);
                        }
                    }
                    else
                    {
                        if (colID == 1 && value != "")
                            contentLine.memotest.Add(value);
                        if (colID == 2 && value != "")
                            contentLine.questions.Add(value);
                        if (colID == 3 && value != "")
                            contentLine.simons.Add(value);
                        if (colID == 4 && value != "")
                            contentLine.fallingObjects.Add(value);
                        if (colID == 5 && value != "")
                            contentLine.rimas.Add(value);
                        if (colID == 6 && value != "")
                            contentLine.loro_time.Add(value);
                        if (colID == 7 && value != "")
                            contentLine.loro_repeat.Add(value);
                        if (colID == 8 && value != "")
                            contentLine.memotestAudio.Add(value);
                    }
                }
                colID++;
            }
            colID = 0;
            rowID++;
        }

    }
}
