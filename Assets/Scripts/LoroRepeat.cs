﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoroRepeat : ScreenMain
{
    public GamesData.Content content;
    public SimpleButton simonCard;
    public Transform container;

   // public int done;
    int id;
    int lastcardID;
    public AssetsData.loroWordsType loroWordsType;

    public override void OnReady()
    {
       // done = 0;
        base.OnReady();
        string story_id = Data.Instance.storiesData.activeContent.id;
        content = Data.Instance.gamesData.GetContent(story_id);
        if (content == null) return;

        TextsData.Content tipContent = Data.Instance.daysData.GetTip("tip_loro_repeat");

        Events.OnCharacterSay(tipContent, OnTipDone, tipContent.character_type);        
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

        ////////////////// por si la palabra del loro es default, inicio o final:
        string[] arr = textID.Split("@"[0]);
        if (arr.Length > 1)
        {
            textID = arr[0];
            loroWordsType = Data.Instance.assetsData.SetTypeByText(arr[1]);
        }
        //////////////////////////////////////////
        ///
        Sprite sprite = Data.Instance.assetsData.GetContent(textID).sprite;
        sb.Init(id, sprite, "", null);

        string assetRealName = Data.Instance.assetsData.GetAssetRealName(textID);
        assetRealName = Data.Instance.assetsData.GetSoundForLoro(assetRealName, loroWordsType);
        Events.PlaySoundTillReady("voices", "assets/audio" + Utils.GetLangFolder() + "/loro_" + assetRealName, WordSaid);

        //done++;
       
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
        id++;
        if (id >= content.loro_repeat.Count)
        {
            id = 0;
            OnComplete();
            Events.SetReadyButton(OnReadyClicked);
        }
        StartCoroutine(SayNext());
    }
}
