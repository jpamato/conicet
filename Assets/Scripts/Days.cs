using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Days : ScreenMain
{
    public Transform container;
    public DayButton button;
    public Image image;
    [SerializeField] Sprite[] allBooks;
    public Image book;

    public override void Show(bool fromRight)
    {
        base.Show(fromRight);
        Utils.RemoveAllChildsIn(container);
        int id = 0;
        string bookID = Data.Instance.storiesData.activeBookContent.id;
        DayButton.states dayState = DayButton.states.ACTIVE;
        bool lastOneReady = false;
        book.sprite = allBooks[Data.Instance.storiesData.activeBookContent.colorID];
        foreach (DaysData.Content content in Data.Instance.daysData.content)
        {
            string storyID = content.story_id;
            string[] arr = storyID.Split(":"[0]);
            if (arr.Length > 1)
                storyID = arr[0];

            string imageName = "stories/" + Data.Instance.storiesData.activeBookContent.id+ "/images/1";
            print("image  " + imageName);
            Sprite s = Resources.Load<Sprite>(imageName);
            float scaleFactor = 0.75f;
            SetSprite( s);

            if (storyID == bookID)
            {
                if (!lastOneReady && id != 0)
                    dayState = DayButton.states.BLOCKED;
                DayButton newButton = Instantiate(button);
                newButton.transform.SetParent(container);
                newButton.transform.localScale = new Vector2(scaleFactor, scaleFactor);
                newButton.Init(this, content, dayState);
                if (newButton.allPlayed)
                    lastOneReady = true;
                else
                    lastOneReady = false;
                id++;
            }
        }
    }
    public override void OnReady()
    {
        Events.ShowHamburguer(true);
        Events.SetBackButton(true);
        Events.SetNextButton(false);
        
    }
    public void OnSelected(DaysData.Content content)
    {
        print("On Select " + content.story_id);
        Events.ShowHamburguer(false);
        Data.Instance.userData.InitDay(content);
    }
    void SetSprite(Sprite s)
    {
        image.sprite = s;

        float _w = s.texture.width;
        float _h = s.texture.height;

        float factor = _h / image.GetComponent<RectTransform>().sizeDelta.y;

        _w = _w / factor;

        RectTransform rTransform = image.GetComponent<RectTransform>();
        rTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _w);
        // rTransform.anchoredPosition = new Vector3(-_w / 2, 0, 0);
        image.sprite = s;
    }
}
