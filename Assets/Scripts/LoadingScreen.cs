using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {
    public GameObject loading;
    public GameObject playButton;
    public GameObject downloadPanel;

    public List<Text> downloadBtnsText;
    public List<Button> downloadBtns;

    [SerializeField] GameObject allDownloadItems;

    [SerializeField] GameObject loadingAsset;
    [SerializeField] Image bar;
    [SerializeField] Text loadingfield;

    void Start() {
        playButton.SetActive(false);
        loading.gameObject.SetActive(true);
        Invoke("DataLoaded", 4);

        loading.SetActive(false);
        Events.AllDataLoaded += AllDataLoaded;
        Events.OnLoading += OnLoading;
        Events.OnLoadingProgress += OnLoadingProgress;

        UpdateDownloadBtns();

    }

    void UpdateDownloadBtns() {
        for (int i = 0; i < downloadBtnsText.Count; i++) {
            Data.Instance.lang = (Data.langs)i;
            if (Data.Instance.cacheManager.IsBundleCached())
                downloadBtnsText[i].text = "ACTUALIZAR";
            else
                downloadBtnsText[i].text = "DESCARGAR";

            downloadBtns[i].interactable = true;
        }
    }

    private void OnDestroy() {
        Events.AllDataLoaded -= AllDataLoaded;
        Events.OnLoading -= OnLoading;
        Events.OnLoadingProgress -= OnLoadingProgress;
    }

    void OnLoadingProgress(float a) {
        bar.fillAmount = a;
    }
    void OnLoading(string a) {
        loadingfield.text = "DESCARGANDO: " + a;
    }

    void DataLoaded() {
        playButton.gameObject.SetActive(true);
        loading.gameObject.SetActive(false);
    }
    public void GotoSplash() {
        Events.ResetApp();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Splash");
    }

    public void OpenDownloadPanel(bool enable) {        
        downloadPanel.SetActive(enable);
    }

    public void DownloadData(int langIndex) {
        Events.ResetApp();
        downloadBtns[langIndex].interactable = false;
        allDownloadItems.SetActive(false);
        loadingAsset.SetActive(true);
        Data.Instance.predownloading = true;
        Data.Instance.lang = (Data.langs)langIndex;
        Events.LoadAssetBundles(OnAssetsLoaded);
    }
    void OnAssetsLoaded(string results) {
        Debug.Log("AssetBundles Load: " + results);
        Data.Instance.LoadAll();
    }

    void AllDataLoaded() {
        allDownloadItems.SetActive(true);
        loadingAsset.SetActive(false);
        Invoke("UpdateDownloadBtns", 10f);
        UpdateDownloadBtns();
        //Conicet.AssetsBundle.AssetsBundleManager.Instance.ResetAllBundles();
    }
}
