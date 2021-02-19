using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoriesSelector : ScreenMain
{
    public Transform container;
    public StoryButton button;

    public override void Show()
    {
        Events.SetBackButton(false);
        base.Show();
        Utils.RemoveAllChildsIn(container);
        foreach(StoriesData.Content content in Data.Instance.storiesData.content)
        {
            StoryButton newButton = Instantiate(button);
            newButton.transform.SetParent(container);
            newButton.transform.localScale = Vector2.one;
            newButton.Init(this, content);
        }
    }
    public void Read(StoriesData.Content content)
    {
        Data.Instance.storiesData.SetContent(content);
        Open(types.STORY_TELLER);
    }
}
