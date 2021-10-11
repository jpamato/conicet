using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemotestAudio : ScreenMain
{
    [SerializeField] GameObject initialButtonPanel;
    [SerializeField] MemotestCard card_to_add;
    [SerializeField] Transform container;
    [SerializeField] FillAmountAnim fillAmountAnim;
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
    public override void OnEnable()
    {
        base.OnEnable();
        title.text = "";
    }
    public override void Show(bool fromRight)
    {
        base.Show(fromRight);
        fillAmountAnim.Init();
        id = 0;
        corrects.Clear();
        cards.Clear();
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
        TextsData.Content tipContent = Data.Instance.daysData.GetTip("tip_toca_la_imagen");
        Events.OnCharacterSay(tipContent, OnTipDone, tipContent.character_type);

        //bool lang = false;
        //if (Data.Instance.lang == Data.langs.QOM) lang = true;
        //TextsData.Content tipContent = Data.Instance.textsData.GetContent("tip_toca_la_imagen", lang);
        //Events.OnCharacterSay(tipContent, OnTipDone, tipContent.character_type);        
    }
    void OnTipDone()
    {
        fillAmountAnim.AnimateOff(10);
        initialButtonPanel.SetActive(true);
        Utils.RemoveAllChildsIn(container);
        GamesData.Content mContent = Data.Instance.gamesData.GetContent(storyData.id);
        Utils.Shuffle(mContent.memotestAudio);

        foreach (string animal in mContent.memotestAudio)
        {
            MemotestCard card = Instantiate(card_to_add, container);
            card.transform.localScale = new Vector2(1.2f, 1.2f);
            AssetsData.Content assetContent = Data.Instance.assetsData.GetContent(animal);
            card.Init(SetSelected, assetContent);
            cards.Add(card);
            corrects.Add(assetContent.name);
            card.GetComponent<SimpleFeedback>().SetOff();
        }
        Utils.Shuffle(corrects);
    }
    void SetNew()
    {
        if (id >= corrects.Count && id>0)
            Events.SetReadyButton(ButtonClicked);
        else
            SetWord();
    }
    void ButtonClicked()
    {
        OnComplete();
        Events.OnGoto(true);
    }
    void SetWord()
    {
        title.text = Data.Instance.assetsData.GetRealText( corrects[id] );
        Events.PlaySound("voices", "assets/audio" + Utils.GetLangFolder() + "/" + corrects[id], false);
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
            card.GetComponent<SimpleFeedback>().SetState(SimpleFeedback.states.OK, 2);
        }
        else
        {
            card.SetWrong();
            StartCoroutine(SetCardsOff(0.8f));
            card.GetComponent<SimpleFeedback>().SetState(SimpleFeedback.states.WRONG, 2);
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
        yield return new WaitForSeconds(1);
        SetWord();
        state = states.IDLE;
        // OnDone();
    }
}
