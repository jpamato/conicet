using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMain : MonoBehaviour
{
    ScreensManager manager;
    public types type;
    public enum types
    {
        STORIES_SELECTOR,
        STORY_TELLER
    }
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
}
