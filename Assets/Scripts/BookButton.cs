using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookButton : MonoBehaviour
{
    StoriesData.Content storyContent;
    Books manager;
    public Text field;
    StoriesData.BookContent bookContent;
    public Image image;
    public Image[] background;
    public GameObject asset;
    public GameObject blocked;
    public Image book;
    public Sprite[] books;

    public void Init(int id, Books manager, StoriesData.BookContent bookContent, Sprite sprite, bool isLast, bool isBlocked)
    {
        if (id >= books.Length)
            id = id - books.Length * (int)Mathf.Floor(id / books.Length);

        bookContent.colorID = id;
        book.sprite = books[id];
        image.sprite = sprite;
        this.manager = manager;
        this.bookContent = bookContent;
        field.text = bookContent.name;
        if (!isLast)
            foreach (Image i in background)
                i.gameObject.SetActive(false);
        else
            foreach (Image i in background)
                i.gameObject.SetActive(true);
        asset.transform.localPosition = new Vector3(Random.Range(-80,80), 0, 0);
        blocked.SetActive(isBlocked);
        if (isBlocked)
        {
            GetComponent<Button>().interactable = false;
        }
    }
    public void SetOn()
    {
        asset.GetComponent<Animation>().Play("on");
    }
    public void Reset()
    {
        asset.GetComponent<Animation>().Play("off");
    }
    public void Clicked()
    {
        manager.OnSelected(bookContent);
    }
}
