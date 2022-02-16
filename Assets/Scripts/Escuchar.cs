using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Escuchar : ScreenMain
{
    public Text field;
    public Text titleField;
    [SerializeField] FillAmountAnim fillAmountAnim;
    bool gameReady;
    public GameObject dialoguesPanel;
    int id = 0;
    public Character character;
    public GameObject musicAsset;
    public Image image;

    public override void OnEnable()
    {
        image.gameObject.SetActive(false);
        base.OnEnable();
        dialoguesPanel.SetActive(false);

        fillAmountAnim.Init();
    }
    public override void OnDisable()
    {
        base.OnDisable();
        titleField.text = "";
    }
    public override void OnOff()
    {
        Events.StopAudioPlayer();
        Events.OnCharacterSay(null, null, CharactersManager.types.Nasheli);
    }
    public override void OnReady()
    {
        titleField.text = "";
        base.OnReady();
        TextsData.Content tipContent = Data.Instance.daysData.GetTip("tip_escucha");
        Events.OnCharacterSay(tipContent, OnTipDone, tipContent.character_type);       
    }
    public override void Hide(bool toLeft)
    {
        base.Hide(toLeft);
        fillAmountAnim.Init();
    }
    public void Repeat()
    {
        if (audio_text != "")
        {
            Events.PlaySoundTillReady("voices", "genericTexts" + Utils.GetLangFolder() + "/" + audio_text, OnTextDone);
        }
    }
    string audio_text = "";
    void OnTipDone()
    {
        fillAmountAnim.AnimateOff(10);
        StoriesData.Content content = Data.Instance.storiesData.activeContent;

        GamesData.Content gameDataContent = Data.Instance.gamesData.GetContent(content.id);
        string text = gameDataContent.GetContentFor(type, gameID)[0];

        //string text = Data.Instance.gamesData.GetContent(content.id).escuchar[id];


        string[] arr = text.Split("_"[0]);

        character.GetComponentInChildren<AudioSpectrumView>().enabled = true;
        musicAsset.SetActive(false);
        character.Idle();       

        if (arr != null && arr.Length>1 && arr[0] == "cancion")
        {
            string[] songTitleArr = text.Split("@"[0]);
            if (songTitleArr.Length > 1)
            {
                text = songTitleArr[0];
                string songTtile = songTitleArr[1];
                titleField.text = "'" + songTtile + "'";
                if (songTitleArr.Length > 2)
                    titleField.text += "\n" + songTitleArr[2];
            }

            musicAsset.SetActive(true);
            character.GetComponentInChildren<AudioSpectrumView>().enabled = false;
            character.Dance();
            image.gameObject.SetActive(false);
            Sprite sprite = Data.Instance.GetSprite("canciones/" + text);
            if (sprite != null)
            {
                image.gameObject.SetActive(true);
                image.sprite = sprite;
            }                
        }
        else
        {
            Sprite sprite = Data.Instance.GetSprite("rimas/" + text);
            //  Sprite sprite = Resources.Load<Sprite>("rimas/" + text);
            if (sprite != null)
            {
                image.sprite = sprite;
                image.gameObject.SetActive(true);
            }
            else
            {
                image.gameObject.SetActive(false);
            }
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
