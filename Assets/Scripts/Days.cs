using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Days : ScreenMain
{
    public Transform container;
    public DayButton button;

    public override void Show(bool fromRight)
    {
        base.Show(fromRight);
        Utils.RemoveAllChildsIn(container);
        int id = 0;
        string bookID = Data.Instance.storiesData.activeBookContent.id;
        DayButton.states dayState = DayButton.states.ACTIVE;
        bool lastOneReady = false;
        foreach (DaysData.Content content in Data.Instance.daysData.content)
        {
            string storyID = content.story_id;
            string[] arr = storyID.Split(":"[0]);
            if (arr.Length > 1)
                storyID = arr[0];


            if (storyID == bookID)
            {
                if (!lastOneReady && id != 0)
                    dayState = DayButton.states.BLOCKED;
                DayButton newButton = Instantiate(button);
                newButton.transform.SetParent(container);
                newButton.transform.localScale = Vector2.one;
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
}
