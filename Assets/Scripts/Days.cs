using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Days : ScreenMain
{
    public Transform container;
    public DayButton button;

    public override void Show()
    {
        Events.SetBackButton(false);
        base.Show();
        Utils.RemoveAllChildsIn(container);
        foreach(DaysData.Content content in Data.Instance.daysData.content)
        {
            DayButton newButton = Instantiate(button);
            newButton.transform.SetParent(container);
            newButton.transform.localScale = Vector2.one;
            newButton.Init(this, content);
        }
    }
    public void OnSelected(DaysData.Content content)
    {
        Data.Instance.daysData.SetContent(content);
        Open(types.DAY);
    }
}
