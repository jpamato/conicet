using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreensManager : MonoBehaviour
{
    
    public ScreenMain[] all;
    public ScreenMain activeScreen;
    static ScreensManager mInstance = null;

    public static ScreensManager Instance { get { return mInstance; } }
    int id = 0;

    void Awake()
    {        
        if (!mInstance)
            mInstance = this;

        Reset();
        if (Data.Instance.allLoaded)
            AllDataLoaded();
        else
            Events.AllDataLoaded += AllDataLoaded;
    }
    private void OnDestroy()
    {
        Events.AllDataLoaded -= AllDataLoaded;
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
        activeScreen = GetScreen(GameData.types.books);
        activeScreen.ForceOpen();
    }
    public void ForceOpen(GameData.types type, bool fromRight)
    {
        if (activeScreen)
            activeScreen.Hide(fromRight);

        activeScreen = GetScreen(type);
        activeScreen.Show(fromRight);
    }
    public void Open(GameData gameData, bool fromRight)
    {
        if (activeScreen)
            activeScreen.Hide(fromRight);

        activeScreen = GetScreen(gameData.type);
        activeScreen.gameID = gameData.gameID;
        activeScreen.Show(fromRight);
    }
    ScreenMain GetScreen(GameData.types type)
    {
        foreach (ScreenMain sm in all)
            if (sm.type == type)
                return sm;

        Debug.Log("No hay screen para: " + type);
        return null;
    }
    void Reset()
    {
        foreach (ScreenMain sMain in all)
            sMain.gameObject.SetActive(false);
    }
}
