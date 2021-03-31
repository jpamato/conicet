using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class UserData : MonoBehaviour
{
    public int activityID = 0;
    public List<SavedData> savedData;

    [Serializable]
    public class SavedData
    {
        public string key;
        public int value;
    }
    public void SetSavedData(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        if (GetSavedKey(key) == null)
            AddSavedData(key, value);
    }
    void AddSavedData(string key, int value)
    {
        SavedData sd = new SavedData();
        sd.key = key;
        sd.value = value;
        savedData.Add(sd);
    }
    SavedData GetSavedKey(string key)
    {
        foreach (SavedData sd in savedData)
            if (sd.key == key)
                return sd;
        return null;
    }
    public int GetValue(string key)
    {
        SavedData sd = GetSavedKey(key);
        if (sd != null)  return sd.value;
        int savedValue = PlayerPrefs.GetInt(key, 0);
        if (savedValue > 0)
            SetSavedData(key, savedValue);
        return savedValue;
    }
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
    public void InitBook(StoriesData.BookContent bookContent)
    {
        Data.Instance.storiesData.SetActiveBook(bookContent);
        ScreensManager.Instance.ForceOpen(GameData.types.all_days, true);
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
        {
            if (ScreensManager.Instance.activeScreen.type == GameData.types.all_days)
                BackToBooks();
            else
                BackToMainMenu();
        }
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
            ScreensManager.Instance.ForceOpen(Data.Instance.initialActivity, fromRight);
        else
            ScreensManager.Instance.Open(gd, fromRight);
    }
    void EndDay()
    {
        print("EndDay");
        Events.SetNextButton(false);
        ScreensManager.Instance.ForceOpen(GameData.types.endDay, true);
    }
    public void BackToMainMenu(bool backFromEnd = false)
    {
        activityID = 0;
        lastActivityID = 0;
        ScreensManager.Instance.ForceOpen(GameData.types.all_days, backFromEnd);
    }
    public void BackToBooks(bool backFromEnd = false)
    {
        InitBook(Data.Instance.storiesData.activeBookContent);
        activityID = 0;
        lastActivityID = 0;
        ScreensManager.Instance.ForceOpen(GameData.types.books, backFromEnd);
    }
}
