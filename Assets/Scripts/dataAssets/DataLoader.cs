using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    public string url;
    public string url_qom;
    public string url_l1;

    public string fileName;

    System.Action OnReady;

    public void LoadData(System.Action OnReady)
    {
        this.OnReady = OnReady;
        if(Data.Instance.mode == Data.modes.OFFLINE)
        {

            string assetPath = "";

            if (fileName == "conicet database - data") // sin lang porque es generico este file:
                assetPath = assetPath = Application.streamingAssetsPath + "/" + fileName + ".tsv";
            else
            {
                switch (Data.Instance.lang)
                {

                    case Data.langs.ESP:
                        assetPath = Application.streamingAssetsPath + "/" + fileName + ".tsv"; break;
                    case Data.langs.QOM:
                        assetPath = Application.streamingAssetsPath + "/" + fileName + "_Qom.tsv"; break;
                    case Data.langs.L1:
                        assetPath = Application.streamingAssetsPath + "/" + fileName + "_L1.tsv"; break;
                }
            }
          

            print(assetPath);

            //if (Data.Instance.lang == Data.langs.ESP)
            //   s = Resources.Load<TextAsset>(fileName).text;
            //else if (Data.Instance.lang == Data.langs.L1)
            //    s = Resources.Load<TextAsset>(fileName + "_L1").text;
            //else
            //    s = Resources.Load<TextAsset>(fileName + "_Qom").text;

                WWW reader = new WWW(assetPath);
                while (!reader.isDone)
                {
                    print("111");
                }
                print("asdasdasd");
                Data.Instance.spreadsheetLoader.CreateListFromFile(reader.text, OnLoaded);
              
            return;
        }
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
    public virtual void Reset() { }

}
