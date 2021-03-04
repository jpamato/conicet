using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreensManager : MonoBehaviour
{
    public ScreenMain[] all;
    ScreenMain activeScreen;
    static ScreensManager mInstance = null;

    public static ScreensManager Instance { get { return mInstance; } }
    int id = 0;

    void Awake()
    {        
        if (!mInstance)
            mInstance = this;

        Reset();

        Events.AllDataLoaded += AllDataLoaded;
    }
    void AllDataLoaded()
    {
        print("AllDataLoaded");
        Reset();
        Init();      
    }
    void Init()
    {
        foreach (ScreenMain sMain in all)
            sMain.Init(this);
        activeScreen = GetScreen(GameData.types.all_days);
        activeScreen.ForceOpen();
    }
    public void Open(GameData.types type, bool fromRight)
    {
        if (activeScreen)
            activeScreen.Hide(fromRight);
        activeScreen = GetScreen(type);
        activeScreen.Show(fromRight);
    }
    ScreenMain GetScreen(GameData.types type)
    {
        foreach (ScreenMain sm in all)
            if (sm.type == type)
                return sm;
        return null;
    }
    void Reset()
    {
        foreach (ScreenMain sMain in all)
            sMain.gameObject.SetActive(false);
    }
}
