using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Books : ScreenMain
{
    public Transform container;
    public BookButton button;
    public Sprite firstImage;
    public Sprite[] loopImages;
    public Sprite lastImage;
    bool loaded;

    public override void OnReady()
    {
        Events.ShowHamburguer(true);
        Events.SetBackButton(false);
        Events.SetNextButton(false);

        AddBooks();
    }
    public override void OnEnable()
    {
        Utils.RemoveAllChildsIn(container);
    }
    void AddBooks()
    {
        Utils.RemoveAllChildsIn(container);
        int id = 0;
        foreach (StoriesData.BookContent bookContent in Data.Instance.storiesData.books)
        {
            BookButton newButton = Instantiate(button);
            newButton.transform.SetParent(container);
            newButton.transform.localScale = Vector2.one;
            Sprite sprite;
            bool isLast = false;
            if (id == 0)
                sprite = firstImage;
            else sprite = loopImages[Random.Range(0, loopImages.Length)];
            id++;
            if (id >= Data.Instance.storiesData.books.Count)
                isLast = true;
            bool blocked = Data.Instance.userData.IsBookBlocked(id);
            newButton.Init(this, bookContent, sprite, isLast, blocked);
          
        }
    }
    public void OnSelected(StoriesData.BookContent bookContent)
    {
        Data.Instance.userData.InitBook(bookContent);
    }
}

