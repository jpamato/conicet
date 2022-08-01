using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Simon : ScreenMain
{
    public List<string> content;
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
    public override void Show(bool fromRight)
    {
        base.Show(fromRight);

        cardsArray.Clear();
        cardActive = 0;
    }
    public override void OnReady()
    {
        cards.Clear();
        Utils.RemoveAllChildsIn(container);

        ok = 0;
        base.OnReady();

        string story_id = Data.Instance.storiesData.activeContent.id;
        GamesData.Content c = Data.Instance.gamesData.GetContent(story_id);
        content = c.GetContentFor(type, gameID);

        if (content == null) return;
        field.text = "";

        TextsData.Content tipContent = Data.Instance.daysData.GetTip("tip_simon");

        Events.OnCharacterSay(tipContent, OnTipDone, tipContent.character_type);
        int id = 0;
        foreach(string text in content)
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

        if (cardActive >= cardsArray.Count) return;
        int cID = cardsArray[cardActive];
        string text_id = content[cID];
        if (button.id == cardsArray[cardActive])
        {
          
            DatabaseCorrect(text_id);
            button.GetComponent<SimpleFeedback>().SetState(SimpleFeedback.states.OK, 2);
            SetResults(true);
        }           
        else
        {
            DatabaseIncorrect(text_id);
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
            if (ok == 6)
            {
                Events.SetReadyButton(OnReadyClicked);
                DatabaseOnSaveToDatabase();
            }
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
        yield return new WaitForSeconds(0.5f);
        if (cardsArray.Count == cardActive)
            NewCard();
        else
            CanSelect();
    }
    void OnTipDone()
    {        
        DatabaseInitTimer();
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
        int cardID = Random.Range(0, content.Count);
        if (cardID == lastcardID)
            return GetCardRandom();
        lastcardID = cardID;
        return cardID;         
    }
    public int cardActive = 0;
    public void SayWords()
    {
        if (cardActive >= cardsArray.Count) return;
        int cID = cardsArray[cardActive];
        string text_id = content[cID];
        if (cardActive == 0) field.text = "";

        cardActive++;


        string assetRealName = Data.Instance.assetsData.GetAssetRealName(text_id);
        Events.PlaySoundTillReady("voices", "assets/audio" + Utils.GetLangFolder() + "/" + assetRealName, WordSaid);


        text_id = Data.Instance.assetsData.GetRealText(text_id);

        field.text += cardActive + ")" + text_id + " ";        
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
