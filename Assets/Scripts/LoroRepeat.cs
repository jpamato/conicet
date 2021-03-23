using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoroRepeat : ScreenMain
{
    GamesData.Content content;
    public SimpleButton simonCard;
    public Transform container;

    int done;
    int id;
    int lastcardID;

    public override void OnReady()
    {
        base.OnReady();
        string story_id = Data.Instance.storiesData.activeContent.id;
        content = Data.Instance.gamesData.GetContent(story_id);
        if (content == null) return;

        TextsData.Content tipContent = Data.Instance.daysData.GetTip("tip_loro_repeat");

        Events.OnCharacterSay(tipContent, OnTipDone);        
    }
    void OnTipDone()
    {
        AddCard();
    }
    void AddCard()
    {
        Utils.RemoveAllChildsIn(container);
        string textID = content.loro_repeat[id];
        SimpleButton sb = Instantiate(simonCard, container);
        sb.transform.localScale = Vector2.one;
        Sprite sprite = Data.Instance.assetsData.GetContent(textID).sprite;
        sb.Init(id, sprite, "", null);  
        Events.PlaySoundTillReady("voices", "assets/" + textID, WordSaid);

        done++;
        id++;
        if (id >= content.loro_repeat.Count)
            id = 0;
        if (done == 5)
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
