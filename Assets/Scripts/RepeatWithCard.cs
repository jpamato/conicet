using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepeatWithCard : ScreenMain
{
    GamesData.Content content;
    public SimpleButton simonCard;
    public Transform container;
    public Character character;
    [SerializeField] FillAmountAnim fillAmountAnim;

    public int done;
    int id;
    int lastcardID;
    public override void OnEnable()
    {
        base.OnEnable();
        fillAmountAnim.Init();
    }

    public override void OnReady()
    {
        base.OnReady();
        string story_id = Data.Instance.storiesData.activeContent.id;
        content = Data.Instance.gamesData.GetContent(story_id);
        if (content == null) return;

        TextsData.Content tipContent = Data.Instance.daysData.GetTip("tip_repeat_with_card");

        Events.OnCharacterSay(tipContent, OnTipDone, tipContent.character_type);
        done = 0;
    }
    void OnTipDone()
    {
        fillAmountAnim.AnimateOff(10);
        AddCard();
    }
    public void Repeat()
    {
        if (audio_text != "")
        {
            string s = "assets/audio" + Utils.GetLangFolder() + "/" + audio_text;

            Events.PlaySoundTillReady("voices", s, WordSaid);
        }
    }
    string audio_text = "";
    void AddCard()
    {
        Utils.RemoveAllChildsIn(container);
        string textID = content.repeat_with_card[id];
        SimpleButton sb = Instantiate(simonCard, container);
        sb.transform.localScale = Vector2.one;
        AssetsData.Content c = Data.Instance.assetsData.GetContent(textID);

        if (c == null)
            Events.Log("Falta Sprite para " + textID);
        else
        {
            Sprite sprite = c.sprite;
            sb.Init(id, sprite, "", null);
        }
        audio_text = textID;
        Repeat();

        done++;
        id++;
        if (id >= content.repeat_with_card.Count)
            id = 0;
        if (done > 5)
        {
            OnComplete();
            Events.SetReadyButton(OnReadyClicked);
        }
    }
    void OnReadyClicked()
    {
        OnComplete();
        Events.OnGoto(true);
    }
    IEnumerator SayNext()
    {
        yield return new WaitForSeconds(4);
        AddCard();
    }
    void WordSaid()
    {
        StartCoroutine(SayNext());
    }
}
