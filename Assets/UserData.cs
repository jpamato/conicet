using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    public int activityID = 0;

    public void InitDay(DaysData.Content content)
    {
        Data.Instance.daysData.SetContent(content);
        Data.Instance.storiesData.SetActiveContent(content.story_id);
        SetActivity();
    }
    public void PrevActivity()
    {
        activityID--;
        if (activityID < 0)
        {
            activityID = 0;
            ScreensManager.Instance.Open(GameData.types.all_days, false);
        }else
            SetActivity();
    }
    public void NextActivity()
    {
        activityID++;
        if (activityID >= Data.Instance.daysData.activeContent.games.Count-1)
            activityID = Data.Instance.daysData.activeContent.games.Count-1;
        SetActivity();
    }
    int lastActivityID;
    void SetActivity()
    {
        GameData gd = Data.Instance.daysData.activeContent.games[activityID];
        bool fromRight = true;
        if (lastActivityID > activityID)
            fromRight = false;
        lastActivityID = activityID;
        ScreensManager.Instance.Open(gd.type, fromRight);
    }
}
