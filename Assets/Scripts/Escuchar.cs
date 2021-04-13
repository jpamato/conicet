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
    public Character character;
    public GameObject musicAsset;

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
        Events.OnCharacterSay(null, null, CharactersManager.types.Brisa);
    }
    public override void OnReady()
    {   
        base.OnReady();
        TextsData.Content tipContent = Data.Instance.daysData.GetTip("tip_escucha");
        Events.OnCharacterSay(tipContent, OnTipDone, tipContent.character_type);       
    }
    public void Repeat()
    {
        if (audio_text != "")
            Events.PlaySoundTillReady("voices", "genericTexts/" + audio_text, OnTextDone);
    }
    string audio_text = "";
    void OnTipDone()
    {
        fillAmountAnim.AnimateOff(10);
        StoriesData.Content content = Data.Instance.storiesData.activeContent;
        
        string text = Data.Instance.gamesData.GetContent(content.id).escuchar[id];
        string[] arr = text.Split("_"[0]);

        character.GetComponentInChildren<AudioSpectrumView>().enabled = true;
        musicAsset.SetActive(false);
        character.Idle();
        if (arr != null && arr.Length>1 && arr[0] == "cancion")
        {
            musicAsset.SetActive(true);
            character.GetComponentInChildren<AudioSpectrumView>().enabled = false;
            character.Dance();
        }

        audio_text = text;
        TextsData.Content c = Data.Instance.textsData.GetContent(text);
        if(c != null)  character.Init(c.character_type);
        field.text = text;
        Repeat();
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
