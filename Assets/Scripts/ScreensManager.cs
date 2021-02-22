using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreensManager : MonoBehaviour
{
    public ScreenMain[] all;
    ScreenMain activeScreen;

    void Start()
    {
        Reset();
        LoopLoad();
    }
    void LoopLoad()
    {
        if (
            Data.Instance.storiesData.content.Count > 0
            &&
            Data.Instance.daysData.content.Count > 0
            )
            Init();
        else
            Invoke("LoopLoad", 0.1f);
    }
    void Init()
    {
        foreach (ScreenMain sMain in all)
            sMain.Init(this);
        Open(ScreenMain.types.DAYS_SELECTOR);
    }
    public void Open(ScreenMain.types type)
    {
        if (activeScreen)
            activeScreen.Hide();
        activeScreen = GetScreen(type);
        activeScreen.Show();
    }
    ScreenMain GetScreen(ScreenMain.types type)
    {
        foreach (ScreenMain sm in all)
            if (sm.type == type)
                return sm;
        return null;
    }
    void Reset()
    {
        foreach (ScreenMain sMain in all)
            sMain.Hide();
    }
}
