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

    public void Init(Days manager, DaysData.Content content)
    {
        this.manager = manager;
        this.content = content;
        storyContent = Data.Instance.storiesData.GetContent(content.story_id);
        dayField.text = content.day.ToString();
    }
    public void Clicked()
    {
        manager.OnSelected(content);
    }
}
