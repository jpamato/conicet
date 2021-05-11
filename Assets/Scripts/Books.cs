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
    public List<BookButton> books;
    bool loaded;

    public override void Show(bool fromRight)
    {
        base.Show(fromRight);
        AddBooks();
        print("Show");
    }
    public override void OnReady()
    {
        Events.ShowHamburguer(true);
        Events.SetBackButton(false);
        Events.SetNextButton(false);
    }
    void AddBooks()
    {
        books.Clear();
        Utils.RemoveAllChildsIn(container);
        int id = 0;
        bool blocked = false;
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
            if (id>1 && !blocked && !Data.Instance.DEBUG)
            {                
                blocked = Data.Instance.userData.IsBookBlocked(id);
                if(blocked)
                    books[id - 2].SetOn();
            }
            if (Data.Instance.DEBUG)
                blocked = false;
            newButton.Init(id, this, bookContent, sprite, isLast, blocked);
            books.Add(newButton);
        }
    }
    public void OnSelected(StoriesData.BookContent bookContent)
    {
        Events.PlaySound("ui", "ui/click", false);
        Data.Instance.userData.InitBook(bookContent);
    }
}

