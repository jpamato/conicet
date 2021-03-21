using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    public string url;
    public string url_qom;
    public string url_l1;

    System.Action OnReady;

    public void LoadData(System.Action OnReady)
    {
        this.OnReady = OnReady;
        string url_by_lang;
        if (Data.Instance.lang == Data.langs.ESP)
            url_by_lang = url;
        else if (Data.Instance.lang == Data.langs.L1)
            url_by_lang = url_l1;
        else
            url_by_lang = url_qom;

        Data.Instance.spreadsheetLoader.LoadFromTo(url_by_lang, OnLoaded);
    }
    public virtual void OnLoaded(List<SpreadsheetLoader.Line> d) {
        OnReady();
    }

}
