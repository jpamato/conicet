using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Simon : ScreenMain
{
    GamesData.Content content;
    public Text field;
    public SimpleButton simonCard;
    public List<SimpleButton> cards;
    public List<int> cardsArray;
    public Transform container;
    public GameObject signal;
    bool canSelect;
    int ok;
    int lastcardID;

    private void OnEnable()
    {
        field.text = "";
        signal.SetActive(false);
    }
    public override void OnReady()
    {
        cards.Clear();
        Utils.RemoveAllChildsIn(container);

        ok = 0;
        base.OnReady();
        string story_id = Data.Instance.storiesData.activeContent.id;
        content = Data.Instance.gamesData.GetContent(story_id);
        if (content == null) return;
        field.text = "";


        bool lang = false;
        if (Data.Instance.lang == Data.langs.QOM) lang = true;
        TextsData.Content tipContent = Data.Instance.textsData.GetContent("tip_simon", lang);

        Events.OnCharacterSay(tipContent, OnTipDone);
        int id = 0;
        foreach(string text in content.simons)
        {
            SimpleButton sb = Instantiate(simonCard, container);
            sb.transform.localScale = Vector2.one;
            Sprite sprite = Data.Instance.assetsData.GetContent(text).sprite;
            sb.Init(id, sprite, "",  OnClicked);
            id++;
            cards.Add(sb);            
        }
    }
    void Animate(string clipName)
    {
        foreach (SimpleButton sb in cards)
            sb.GetComponent<Animation>().Play(clipName);
    }
    void OnClicked(SimpleButton button)
    {
        if (!canSelect) return;
        
        if (button.id == cardsArray[cardActive])
        {
            button.GetComponent<SimpleFeedback>().SetState(SimpleFeedback.states.OK, 2);
            SetResults(true);
        }           
        else
        {
            button.GetComponent<SimpleFeedback>().SetState(SimpleFeedback.states.WRONG, 2);
            SetResults(false);
        }
        canSelect = false;
        Animate("allOn");
    }
    void SetResults(bool isOk)
    {
        if(isOk)
        {
            cardActive++;
            ok++;
            if (ok > 5)
                Events.SetReadyButton(OnReadyClicked);
        }
        else
        {
            cardsArray.Clear();
            cardActive = 0;
        }

        StartCoroutine(CheckResults());
       
    }
    void OnReadyClicked()
    {
        OnComplete();
        Events.OnGoto(true);
    }
    IEnumerator CheckResults()
    {
        yield return new WaitForSeconds(1);
        if (cardsArray.Count == cardActive)
            NewCard();
        else
            CanSelect();
    }
    void OnTipDone()
    {        
        NewCard();
    }
    void NewCard()
    {
        cardActive = 0;
        signal.SetActive(true);
        int cardID = GetCardRandom();
        cardsArray.Add(cardID);
        SayWords();       
    }
    int GetCardRandom()
    {
        int cardID = Random.Range(0, content.simons.Count);
        if (cardID == lastcardID)
            return GetCardRandom();
        lastcardID = cardID;
        return cardID;         
    }
    public int cardActive = 0;
    public void SayWords()
    {
        int cID = cardsArray[cardActive];
        string text_id = content.simons[cID];

        cardActive++;

        Events.PlaySoundTillReady("voices", "assets/" + text_id, WordSaid);
       
        field.text = cardActive + " -" + text_id;        
    }
    void WordSaid()
    {
        if (cardActive >= cardsArray.Count)
        {
            cardActive = 0;
            CanSelect();
        }            
        else
            Invoke("SayWords", 0.1f);
    }
    void CanSelect()
    {
        if(gameObject.activeSelf)
            Animate("rotateRightLeft");
        canSelect = true;
        
    }
}
