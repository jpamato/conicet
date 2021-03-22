using System.Collections;
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
    public override void OnReady()
    {        
        base.OnReady();
        storyData = Data.Instance.storiesData.activeContent;

        Utils.RemoveAllChildsIn(container);
        GamesData.Content mContent = Data.Instance.gamesData.GetContent(storyData.id);
        Utils.Shuffle(mContent.memotest);
       
        AddCards(mContent);

        bool lang = false;
        if (Data.Instance.lang == Data.langs.QOM) lang = true;
        TextsData.Content tipContent = Data.Instance.textsData.GetContent("tip_memotest", lang);
        Events.OnCharacterSay(tipContent, OnTipDone);
    }
    void AddCards(GamesData.Content mContent)
    {
        List<string> all = new List<string>();
        foreach (string s in mContent.memotest)
            all.Add(s);
        foreach (string s in mContent.memotest)
            all.Add(s);
        Utils.Shuffle(all);
        foreach (string animal in all)
        {
            MemotestCard card = Instantiate(card_to_add, container);
            card.transform.localScale = new Vector3(scale, scale, scale);
            AssetsData.Content assetContent = Data.Instance.assetsData.GetContent(animal);
            card.Init(SetSelected, assetContent);
            cards.Add(card);
        }
    }
    void OnTipDone()
    {
        StartCoroutine(SetCardsOff(2));
    }
    void SetNew()
    {
        if (corrects >= cards.Count)
        {
            state = states.INIT;
            OnComplete();
            Events.SetReadyButton(OnNext);
        }
    }
    void SayWord()
    {
        Events.PlaySound("voices", "assets/" + card.content.name, false);
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
            card.SetDone();
            lastSelected.SetDone();
            corrects += 2;
            state = states.IDLE;
            SetNew();

            lastSelected = null;
        }
        else
        {            
            card.SetWrong();
            lastSelected.SetWrong();

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
                card.SetOff();
        }           
        yield return new WaitForSeconds(1);
        state = states.IDLE;
        // OnDone();
    }
    void OnNext()
    {
        Events.OnGoto(true);
    }
}
