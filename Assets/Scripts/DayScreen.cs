using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayScreen : ScreenMain
{
    public Transform container;
    public DayContentButton button;

    public override void OnReady()
    {
        Events.SetBackButton(true);
        base.OnReady();
        Utils.RemoveAllChildsIn(container);
        DaysData.Content dayDataContent = Data.Instance.daysData.activeContent;
        foreach (GameData gd in dayDataContent.games)
        {
            DayContentButton newButton = Instantiate(button);
            newButton.transform.SetParent(container);
            newButton.transform.localScale = Vector2.one;
            newButton.Init(this, dayDataContent, gd);
        }
    }
    public override void OnDisable()
    {
        base.OnDisable();
        Events.StopAudioPlayer();
    }
    //public void OnContentClicked(DaysData.Content content, GameData gameData)
    //{
    //    StoriesData.Content storyData = Data.Instance.storiesData.GetContent(content.story_id);
    //    Open(storyData, gameData.type);
    //}
}
