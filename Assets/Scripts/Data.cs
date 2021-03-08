using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class Data : MonoBehaviour
{
    const string PREFAB_PATH = "Data";
    static Data mInstance = null;
    public bool DEBUG;
    [HideInInspector] public string lastScene;
    [HideInInspector] public string newScene;
    [HideInInspector] public SpreadsheetLoader spreadsheetLoader;
    [HideInInspector] public StoriesData storiesData;
    [HideInInspector] public DaysData daysData;
    [HideInInspector] public AssetsData assetsData;
    [HideInInspector] public GamesData gamesData;
    [HideInInspector] public TextsData textsData;
    [HideInInspector] public UserData userData;

    DataLoader[] allDataFiles;

    public static Data Instance
    {
        get
        {
            if (mInstance == null)
            {
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
    public void LoadLevel(string aLevelName, bool showMap)
    {
        this.newScene = aLevelName;
        Invoke("LoadDelayed", 0.75f);       
    }
    void LoadDelayed()
    {
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

        DontDestroyOnLoad(this);
        spreadsheetLoader = GetComponent<SpreadsheetLoader>();
        storiesData = GetComponent<StoriesData>();
        daysData = GetComponent<DaysData>();
        assetsData = GetComponent<AssetsData>();
        gamesData = GetComponent<GamesData>();
        textsData = GetComponent<TextsData>();
        userData = GetComponent<UserData>();

        LoadAll();
    }
    void LoadAll()
    {
        allDataFiles = GetComponents<DataLoader>();
        foreach (DataLoader dl in allDataFiles)
            dl.LoadData(OnDone);
    }
    int dataLoaded;
    void OnDone()
    {
        dataLoaded++;
        if (dataLoaded >= allDataFiles.Length)
            Events.AllDataLoaded();
    }
}
