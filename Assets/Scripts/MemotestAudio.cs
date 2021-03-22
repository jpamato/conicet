using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemotestAudio : ScreenMain
{
    [SerializeField] GameObject initialButtonPanel;
    [SerializeField] MemotestCard card_to_add;
    [SerializeField] Transform container;
    public List<MemotestCard> cards;
    public List<string> corrects;
    StoriesData.Content storyData;
    [SerializeField] Text title;
    MemotestCard card;
    states state;
    int id;

    public enum states
    {
        INIT,
        IDLE,
        CARD_SELECTED
    }

    public override void OnReady()
    {        
        base.OnReady();
        storyData = Data.Instance.storiesData.activeContent;
        if (storyData == null) return;
        Init();
    }
    public void InitialButtonClicked()
    {
        StartCoroutine(SetCardsOff(0));
        initialButtonPanel.SetActive(false);
    }
    private void Init()
    {
        bool lang = false;
        if (Data.Instance.lang == Data.langs.QOM) lang = true;
        TextsData.Content tipContent = Data.Instance.textsData.GetContent("tip_memotest", lang);

        // Events.PlaySound("voices", "animals/" + corrects[id], false);
        initialButtonPanel.SetActive(true);

        Utils.RemoveAllChildsIn(container);
        GamesData.Content mContent = Data.Instance.gamesData.GetContent(storyData.id);
        Utils.Shuffle(mContent.memotestAudio);
        foreach (string animal in mContent.memotestAudio)
        {
            MemotestCard card = Instantiate(card_to_add, container);
            card.transform.localScale = Vector2.one;
            AssetsData.Content assetContent = Data.Instance.assetsData.GetContent(animal);
            card.Init(SetSelected, assetContent);
            cards.Add(card);
            corrects.Add(assetContent.name);
        }
        Utils.Shuffle(corrects);
        
    }
    void SetNew()
    {
        if (id >= corrects.Count)
            state = states.INIT;
        else
            SetWord();
    }
    void SetWord()
    {
        title.text = corrects[id];
        Events.PlaySound("voices", "assets/" + corrects[id], false);
    }
    public void SetSelected(MemotestCard card)
    {
        if (state != states.IDLE) return;
        state = states.CARD_SELECTED;
        this.card = card;
        card.SetOn();
        StartCoroutine(CheckResults(1) );
    }
    IEnumerator CheckResults(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (card.content.name == corrects[id])
        {
            card.SetDone();
            state = states.IDLE;
            id++;
            SetNew();
        }
        else
        {
            card.SetWrong();
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
        SetWord();
        state = states.IDLE;
        // OnDone();
    }
}
