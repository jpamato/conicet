using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryButton : MonoBehaviour
{
    StoriesData.Content content;
    StoriesSelector selector;
    public Text field;
    public Text field2;

    public void Init(StoriesSelector selector, StoriesData.Content content)
    {
        this.content = content;
        this.selector = selector;
        field.text = content.name;
        field2.text = content.id;
    }
    public void Clicked()
    {
        selector.Read(content);
    }
}
