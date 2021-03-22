using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Escuchar : ScreenMain
{
    public Text field;
    [SerializeField] FillAmountAnim fillAmountAnim;
    bool gameReady;
    public GameObject dialoguesPanel;
    int id = 0;

    public override void OnEnable()
    {
        base.OnEnable();
        dialoguesPanel.SetActive(false);

        fillAmountAnim.Init();
    }
    public override void OnDisable()
    {
        base.OnDisable();
    }
    public override void OnOff()
    {
        Events.StopAudioPlayer();
        Events.OnCharacterSay(null, null);
    }
    public override void OnReady()
    {   
        base.OnReady();
        bool lang = false;
        if (Data.Instance.lang == Data.langs.QOM)  lang = true;
        TextsData.Content  tipContent = Data.Instance.textsData.GetContent("tip_escucha", lang);

        Events.OnCharacterSay(tipContent, OnTipDone);       
    }
    
    void OnTipDone()
    {
        fillAmountAnim.AnimateOff(10);
        StoriesData.Content content = Data.Instance.storiesData.activeContent;
        string text = Data.Instance.gamesData.GetContent(content.id).escuchar[id];
        field.text = text;
        Events.PlaySoundTillReady("voices", "genericTexts/" + text, OnTextDone);
        dialoguesPanel.SetActive(false);
    }
    public override void OnComplete()
    {
        base.OnComplete();
        Events.SetReadyButton(OnReadyClicked);
    }
    void OnTextDone()
    {
        OnComplete();
        Events.SetNextButton(true);
    }
    void OnReadyClicked()
    {
        Events.OnGoto(true);
    }
}
