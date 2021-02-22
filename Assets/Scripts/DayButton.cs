using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayButton : MonoBehaviour
{
    StoriesData.Content storyContent;
    Days manager;
    public Text field;
    public Text field2;
    public Text dayField;
    DaysData.Content content;

    public void Init(Days manager, DaysData.Content content)
    {
        this.manager = manager;
        this.content = content;
        field.text = content.story_id;
        storyContent = Data.Instance.storiesData.GetContent(content.story_id);
        field2.text = storyContent.id;
        dayField.text = content.day.ToString();
    }
    public void Clicked()
    {
        manager.OnSelected(content);
    }
}
