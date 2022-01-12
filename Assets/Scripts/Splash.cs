using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Splash : MonoBehaviour
{
    public GameObject panel;
    public GameObject loading;
    public Animation toggleAnim;
    public Text field;
    public Image loadingImage;
    public Sprite[] sprites_lang;

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
    private void Start()
    {
        if (PlayerPrefs.GetInt("dificult") == 0)
            Data.Instance.dificult = Data.difficults.NORMAL;
        else
            Data.Instance.dificult = Data.difficults.EASY;

        SetToggle();
    }
    public void SetLang(int lang)
    {
        Events.PlaySound("ui", "ui/click", false);

        loading.SetActive(true);


        if (lang == 1)
        {
            Data.Instance.lang = Data.langs.ESP;
            loadingImage.sprite = sprites_lang[0];
        }
        else if (lang == 2)
        {
            Data.Instance.lang = Data.langs.QOM;
            loadingImage.sprite = sprites_lang[1];
        }
        else
        {
            Data.Instance.lang = Data.langs.L1;
            loadingImage.sprite = sprites_lang[2];
        }

        Events.PlaySound("voices", "intro/ayuda_familia_maestra_" + Data.Instance.lang, false);
        Invoke("Delayed", 2);
        Debug.Log("Set lang " + lang + " Data.Instance.lang: " + Data.Instance.lang);

        panel.SetActive(false);
    }
    public void Toggle()
    {
        Events.PlaySound("ui", "ui/click2", false);
        if (Data.Instance.dificult == Data.difficults.EASY)
        {
            Data.Instance.dificult = Data.difficults.NORMAL;
            PlayerPrefs.SetInt("dificult", 0);
        }
        else
        {
            Data.Instance.dificult = Data.difficults.EASY;
            PlayerPrefs.SetInt("dificult", 1);
        }
        SetToggle();
    }
    void SetToggle()
    {
        if (Data.Instance.dificult == Data.difficults.NORMAL)
        {
            field.text = "Todos los niveles";
            toggleAnim.Play("state1");
        }
        else
        {
            field.text = "Oculta los niveles más complicados";
            toggleAnim.Play("state2");
        }
    }
    void Delayed()
    {
        Data.Instance.LoadAll();
    }
}
