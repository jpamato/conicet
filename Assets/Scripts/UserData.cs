using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    public int activityID = 0;

    private void Start()
    {
        Events.OnGoto += OnGoto;
    }
    private void OnDestroy()
    {
        Events.OnGoto -= OnGoto;
    }
    public GameData GetActualActivity()
    {
        return Data.Instance.daysData.activeContent.games[activityID];
    }
    public void OnCompleteActivity()
    {
        Data.Instance.daysData.SetActivityComplete(activityID);
    }
    void OnGoto(bool next)
    {
        if (next)
            NextActivity();
        else
            PrevActivity();

        Events.SetNextButton(false);
        Events.SetBackButton(false);
    }
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
            BackToMainMenu();
        else
            SetActivity();
    }
    public void NextActivity()
    {      
        activityID++;

        if (activityID >= Data.Instance.daysData.activeContent.games.Count)
            EndDay();
        else
            SetActivity();
    }
    int lastActivityID;
    void SetActivity()
    {
        Events.SetNextButton(false);
        GameData gd = Data.Instance.daysData.activeContent.games[activityID];
        bool fromRight = true;
        if (lastActivityID > activityID)
            fromRight = false;
        lastActivityID = activityID;

        if (Data.Instance.DEBUG && Data.Instance.initialActivity != GameData.types.all_days)
            ScreensManager.Instance.Open(Data.Instance.initialActivity, fromRight);
        else
            ScreensManager.Instance.Open(gd.type, fromRight);
    }
    void EndDay()
    {
        print("EndDay");
        Events.SetNextButton(false);
        ScreensManager.Instance.Open(GameData.types.endDay, true);
    }
    public void BackToMainMenu(bool backFromEnd = false)
    {
        activityID = 0;
        lastActivityID = 0;
        ScreensManager.Instance.Open(GameData.types.all_days, backFromEnd);
    }
}
