using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class Data : MonoBehaviour
{
    const string PREFAB_PATH = "Data";
    static Data mInstance = null;

    public langs lang;
    public enum langs
    {
        ESP,
        QOM,
        L1
    }

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
    public AudioSpectrum audioSpectrum;
    DataLoader[] allDataFiles;
    int dataLoaded;
    public bool allLoaded;

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
            print("Borra");
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this);
        spreadsheetLoader = GetComponent<SpreadsheetLoader>();
        storiesData = GetComponent<StoriesData>();
        daysData = GetComponent<DaysData>();
        assetsData = GetComponent<AssetsData>();
        gamesData = GetComponent<GamesData>();
        textsData = GetComponent<TextsData>();
        userData = GetComponent<UserData>();
        audioSpectrum = GetComponent<AudioSpectrum>();

        if(SceneManager.GetActiveScene().name != "Splash")
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
        if (dataLoaded >= allDataFiles.Length)
            Events.AllDataLoaded();
        allLoaded = true;
    }
}
