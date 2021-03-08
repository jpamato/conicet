using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Days : ScreenMain
{
    public Transform container;
    public DayButton button;

    public override void OnReady()
    {
        Events.SetBackButton(false);
        Events.SetNextButton(false);
        Utils.RemoveAllChildsIn(container);
        int id = 0;
        foreach(DaysData.Content content in Data.Instance.daysData.content)
        {
            DayButton newButton = Instantiate(button);
            newButton.transform.SetParent(container);
            newButton.transform.localScale = Vector2.one;
            newButton.Init(this, content);
            id++;
            if (id > 0) return;
        }
    }
    public void OnSelected(DaysData.Content content)
    {
        Data.Instance.userData.InitDay(content);
    }
}
