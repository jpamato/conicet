using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoriesData : DataLoader
{
    public StoriesData.BookContent activeBookContent;
    public List<Content> content;
    // [HideInInspector] 
    public Content activeContent;
    public List<BookContent> books;

    [System.Serializable]
    public class BookContent
    {
        public string name;
        public string id;
        public Color color;
    }

    [System.Serializable]
    public class Content
    {
        public string bookName;
        public string id;
        public string name;
        public string folder;
        public int varType; // 0 full, 1:solo texto, 2: solo audio
        public AudioClip audioClip;
        public List<TimelineTextData> textsData;
        public CharactersManager.types characterType;
    }
    public override void Reset()
    {
        books.Clear();
        content.Clear();
    }
    public override void OnLoaded(List<SpreadsheetLoader.Line> d)
    {
        OnDataLoaded(content, d);
        base.OnLoaded(d);
    }
    public Content GetContent(string story_id)
    {
        foreach (Content c in content)
            if (c.id == story_id)
                return c;
        return null;
    }
    public void SetActiveBook(StoriesData.BookContent bookContent)
    {
        activeBookContent = bookContent;
    }
    public void SetActiveContent(string story_id)
    {
        Content content = GetContent(story_id);
        if(content == null)
        {
            content = new Content();
            string[] arr = story_id.Split(":"[0]);
            if(arr.Length>1)
                content = GetContent(arr[0]);
            else
                content = GetContent(story_id);

            if (content == null)
                Events.Log("No hay story para : " + story_id);

            content.id = story_id;
        }
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
                            string[] arr = value.Split(":"[0]);
                            if(arr.Length>1)
                                contentLine.bookName = arr[0];
                            else
                                contentLine.bookName = value;
                            AddBookIfIsNew(contentLine.bookName);
                            contentLine.textsData = new List<TimelineTextData>();

                            content.Add(contentLine);
                        }
                    }
                    else
                    {
                        if (colID == 1 && value != "")
                        {
                            contentLine.name = value;
                            activeBookContent.name = value;
                        }
                        else if (colID == 2 && value != "")
                        {
                            contentLine.folder = value;
                        }
                        else if (colID == 3 && value != "")
                        {
                            string url = "stories/" + contentLine.folder + "/audios/" + value;
                            contentLine.audioClip = Resources.Load<AudioClip>(url);
                        }
                        else if (colID == 4 && value != "")
                        {
                            textData = new TimelineTextData();
                            textData.text = value;
                        }
                        else if (colID == 5)
                        {
                            if (value != "")
                                textData.seconds = Utils.GetTotalSecondsFromString(value);
                            contentLine.textsData.Add(textData);
                        }
                        else if (colID == 6)
                        {
                            if (value != "")
                                contentLine.varType = int.Parse(value);
                        }
                        else if (colID == 7)
                        {
                            if (value != "")
                                contentLine.characterType = (CharactersManager.types)System.Enum.Parse(typeof(CharactersManager.types), value);
                        }
                    }
                }
                colID++;
            }
            colID = 0;
            rowID++;
        }
        
    }
    void AddBookIfIsNew(string bookID)
    {
        foreach (BookContent bn in books)
            if (bn.id == bookID)
                return;
        activeBookContent = new BookContent();
        activeBookContent.id = bookID;
        books.Add(activeBookContent);
    }
}
