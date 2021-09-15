using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{
    public GameObject panel;
    public GameObject loading;

    private void Awake()
    {
        loading.SetActive(false);
        Events.AllDataLoaded += AllDataLoaded;
    }
    private void OnDestroy()
    {
        Events.AllDataLoaded -= AllDataLoaded;
    }
    void AllDataLoaded()
    {
        Data.Instance.LoadScene("Game");        
    }
    public void SetLang(int lang)
    {
        Events.PlaySound("ui", "ui/click", false);
        loading.SetActive(true);
        if (lang == 1)
            Data.Instance.lang = Data.langs.ESP;
        else if (lang == 2)
            Data.Instance.lang = Data.langs.QOM;
          else
            Data.Instance.lang = Data.langs.L1;

        Debug.Log("Set lang " + lang + " Data.Instance.lang: " + Data.Instance.lang);

        Data.Instance.LoadAll();
        panel.SetActive(false);
    }
}
