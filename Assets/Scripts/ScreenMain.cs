using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMain : MonoBehaviour
{
    ScreensManager manager;
    public types type;
    public enum types
    {
        DAYS_SELECTOR,
        DAY,
        STORY_TELLER,
        MEMOTEST
    }
    public virtual void OnEnable()
    {
        Events.OnBack += OnBack;
    }
    public virtual void OnDisable()
    {
        Events.OnBack -= OnBack;
    }
    public virtual void OnBack()  {  }
    public virtual void Init(ScreensManager manager)
    {
        this.manager = manager;
    }
    public virtual void Show()
    {
        gameObject.SetActive(true);    
    }
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Open(types type)
    {
        manager.Open(type);
    }
    public void Open(StoriesData.Content storyData, GameData.types type)
    {
        Data.Instance.storiesData.SetContent(storyData);
        switch(type)
        {
            case GameData.types.read_automatic:
                manager.Open(types.STORY_TELLER);
                break;
            case GameData.types.memotest:
                manager.Open(types.MEMOTEST);
                break;
        }        
    }
}
