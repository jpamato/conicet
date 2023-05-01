using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Conicet.AssetsBundle;

public class Data : MonoBehaviour
{
    public string url = "http://pontura.com/conicet/AssetBundles/";
    const string PREFAB_PATH = "Data";
    static Data mInstance = null;

    public langs lang;
    public enum langs
    {
        ESP,
        QOM,
        L1
    }
    public difficults dificult;
    public enum difficults
    {
        EASY,
        NORMAL
    }

    public bool predownloading;
    public bool DEBUG;
    public GameData.types initialActivity;

    [HideInInspector] public string lastScene;
    [HideInInspector] public string newScene;
    [HideInInspector] public SpreadsheetLoader spreadsheetLoader;
    [HideInInspector] public StoriesData storiesData;
    [HideInInspector] public DaysData daysData;
    [HideInInspector] public AssetsData assetsData;
    [HideInInspector] public GamesData gamesData;
    [HideInInspector] public TextsData textsData;
    [HideInInspector] public UserData userData;
    [HideInInspector] public CacheManager cacheManager;    
    public AudioSpectrum audioSpectrum;
    DataLoader[] allDataFiles;
    int dataLoaded;
    public bool allLoaded;
    public modes mode;
    public enum modes
    {
        ONLINE,
        OFFLINE
    }

    public static Data Instance
    {
        get
        {
            if (mInstance == null)
            {
                print("ADS");
                mInstance = FindObjectOfType<Data>();

                if (mInstance == null)
                {
                    GameObject go = Instantiate(Resources.Load<GameObject>(PREFAB_PATH)) as GameObject;
                    mInstance = go.GetComponent<Data>();
                    go.transform.localPosition = new Vector3(0, 0, 0);
                }
            }
            return mInstance;
        }
    }
    public void LoadScene(string aLevelName)
    {
        this.newScene = aLevelName;
    //    Invoke("LoadDelayed", 0.75f);       
    //}
    //void LoadDelayed()
    //{
         SceneManager.LoadScene(newScene);
    }
    void Awake()
    {

        if (!mInstance)
            mInstance = this;

        else
        {
            Destroy(this.gameObject);
            return;
        }
        if (mode == modes.OFFLINE)
            url = Application.streamingAssetsPath + "/";

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        DontDestroyOnLoad(this);
        spreadsheetLoader = GetComponent<SpreadsheetLoader>();
        storiesData = GetComponent<StoriesData>();
        daysData = GetComponent<DaysData>();
        assetsData = GetComponent<AssetsData>();
        gamesData = GetComponent<GamesData>();
        textsData = GetComponent<TextsData>();
        userData = GetComponent<UserData>();
        audioSpectrum = GetComponent<AudioSpectrum>();
        cacheManager = GetComponent<CacheManager>();

        if(SceneManager.GetActiveScene().name != "Splash" && SceneManager.GetActiveScene().name != "Loading")
            LoadAll();

        Events.ResetApp += ResetApp;
    }
    void ResetApp()
    {
        dataLoaded = 0;
        allLoaded = false;
        foreach (DataLoader dl in allDataFiles)
            dl.Reset();
    }
    public void LoadAll()
    {
        allDataFiles = GetComponents<DataLoader>();
        foreach (DataLoader dl in allDataFiles)
            dl.LoadData(OnDone);
    }
    void OnDone()
    {
        dataLoaded++;
        if (dataLoaded >= allDataFiles.Length) {
            cacheManager.SetSheetCached();
            Events.AllDataLoaded();
            allLoaded = true;
            if (Data.Instance.predownloading)            
                Data.Instance.predownloading = false;
        }        
    }
    public Sprite GetSprite(string url)
    {
        string folder = "";
        string asset = url;

        string[] arr = url.Split("/"[0]);
        int id = 0;
        foreach (string s in arr)
        {
            if (id < arr.Length - 2)
                folder += s + "/";
            else if (id < arr.Length - 1)
                folder += s;
            else
                asset = s;
            id++;
        }
        return AssetsBundleManager.Instance.GetSprite(folder.ToLower(), asset);
    }
    public AudioClip GetAudio(string url)
    {
        if(url.Contains("intro/") || url.Contains("ui/") || url.Contains("specialLoops/"))
        {
            return Resources.Load<AudioClip>(url) as AudioClip;
        }
        string folder = "";
        string asset = url;

        string[] arr = url.Split("/"[0]);
        int id = 0;
        foreach(string s in arr)
        {
            if (id < arr.Length - 2)
                folder += s + "/";
            else if (id < arr.Length - 1)
                folder += s;
            else
                asset = s;
            id++;
        }
        return AssetsBundleManager.Instance.GetAudioClip(folder.ToLower(), asset);
    }
}
