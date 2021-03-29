using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Days : ScreenMain
{
    public Transform container;
    public DayButton button;

    public override void OnReady()
    {
        Events.ShowHamburguer(true);
        Events.SetBackButton(true);
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
        }
    }
    public void OnSelected(DaysData.Content content)
    {
        Events.ShowHamburguer(false);
        Data.Instance.userData.InitDay(content);
    }
}
