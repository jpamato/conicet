using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayButton : MonoBehaviour
{
    StoriesData.Content storyContent;
    Days manager;
    public Text dayField;
    DaysData.Content content;

    public Transform slotsContainer;
    public DayButtonSlot dayButtonSlot;

    public GameObject done;

    public void Init(Days manager, DaysData.Content content)
    {
        this.manager = manager;
        this.content = content;
        storyContent = Data.Instance.storiesData.GetContent(content.story_id);
        dayField.text = content.day.ToString();
        bool allPlayed = true;
        foreach(GameData gd in content.games)
        {
            DayButtonSlot slot = Instantiate(dayButtonSlot, slotsContainer);
            slot.Init(gd.played);
            if (!gd.played)
                allPlayed = false;
        }
        if (allPlayed)
            done.SetActive(true);
        else
            done.SetActive(false);
    }
    public void Clicked()
    {
        manager.OnSelected(content);
    }
}
