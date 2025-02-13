﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Memotest : ScreenMain
{
    [SerializeField] MemotestCard card_to_add;
    [SerializeField] Transform container;
    public List<MemotestCard> cards;
    StoriesData.Content storyData;
    [SerializeField] Text title;
    MemotestCard card;
    public states state;
    public float scale = 0.8f;
    int corrects = 0;
    public enum states
    {
        INIT,
        IDLE,
        CARD_SELECTED
    }
    private void OnEnable()
    {
        cards.Clear();
        Utils.RemoveAllChildsIn(container);
    }
    public override void Show(bool fromRight)
    {
        corrects = 0;
        base.Show(fromRight);
        cards.Clear();
        Utils.RemoveAllChildsIn(container);
    }
    List<string> arr;
    public override void OnReady()
    {        
        base.OnReady();
        storyData = Data.Instance.storiesData.activeContent;

        Utils.RemoveAllChildsIn(container);


        //GamesData.Content mContent = Data.Instance.gamesData.GetContent(storyData.id);




        string story_id = Data.Instance.storiesData.activeContent.id;
        GamesData.Content mContent = Data.Instance.gamesData.GetContent(story_id);

        StoriesData.Content story_content = Data.Instance.storiesData.activeContent;
        GamesData.Content gameDataContent = Data.Instance.gamesData.GetContent(story_content.id);
        arr = gameDataContent.GetContentFor(type, gameID);




        Utils.Shuffle(arr);
       
        AddCards(arr);
        TextsData.Content tipContent = Data.Instance.daysData.GetTip("tip_memotest");
        Events.OnCharacterSay(tipContent, OnTipDone, tipContent.character_type);
    }
    void AddCards(List<string> content)
    {
        List<string> all = new List<string>();
        foreach (string s in content)
            all.Add(s);
        foreach (string s in content)
            all.Add(s);
        Utils.Shuffle(all);
        foreach (string animal in all)
        {
            MemotestCard card = Instantiate(card_to_add, container);
            card.transform.localScale = new Vector3(scale, scale, scale);
            AssetsData.Content assetContent = Data.Instance.assetsData.GetContent(animal);
            if(assetContent == null)
                Events.Log("Falta asset: " + animal);
            else
                card.Init(SetSelected, assetContent);
            cards.Add(card);

            card.GetComponent<SimpleFeedback>().SetOff();
        }
    }
    void OnTipDone()
    {
        DatabaseInitTimer();
        StartCoroutine(SetCardsOff(1.25f));
    }
    void SetNew()
    {
        if (corrects >= cards.Count)
        {
            DatabaseOnSaveToDatabase();
            state = states.INIT;
            OnComplete();
            Events.SetReadyButton(OnNext);
        }
    }
    void SayWord()
    {
        string s = "assets/audio" + Utils.GetLangFolder() + "/" + card.content.name;
        Events.PlaySound("voices", s, false);
    }
    public MemotestCard lastSelected;

    public void SetSelected(MemotestCard card)
    {
        if (state != states.IDLE) return;

        if (lastSelected == null)
            lastSelected = card;

        state = states.CARD_SELECTED;
        this.card = card;
        SayWord();
        card.SetOn();
        StartCoroutine(CheckResults(1) );
    }
    
    IEnumerator CheckResults(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(lastSelected == card)
        {
            state = states.IDLE;
        } else if (card.content.name == lastSelected.content.name)
        {

            card.GetComponent<SimpleFeedback>().SetState(SimpleFeedback.states.OK, 2);
            Events.PlaySound("ui", "ui/feedback_ok", false);
            card.SetDone();
            lastSelected.SetDone();
            corrects += 2;
            state = states.IDLE;
            SetNew();
            DatabaseCorrect(card.content.name);
            lastSelected = null;
        }
        else
        {

            card.GetComponent<SimpleFeedback>().SetState(SimpleFeedback.states.WRONG, 2);
            Events.PlaySound("ui", "ui/feedback_bad", false);
            card.SetWrong();
            lastSelected.SetWrong();
            DatabaseIncorrect(card.content.name);
            lastSelected = null;
            StartCoroutine(SetCardsOff(0.8f));
        }            
    }
    IEnumerator SetCardsOff(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (MemotestCard card in cards)
        {
            if(card.state == MemotestCard.states.ON)
            {
                card.SetOff();
                card.GetComponent<SimpleFeedback>().SetOff();
            }
        }           
        yield return new WaitForSeconds(0.65f);
        state = states.IDLE;
        // OnDone();
    }
    void OnNext()
    {
        Events.OnGoto(true);
    }
}
