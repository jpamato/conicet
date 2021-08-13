using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElegirFrase : ScreenMain
{
    public List<string> content;
    public Text field;
    public SimpleButton simonCard;
    public List<SimpleButton> cards;
    public Transform container;
    public GameObject signal;
    public bool canSelect;
    public int cardActive = 0;

    private void OnEnable()
    {
        field.text = "";
        signal.SetActive(false);
    }
    public override void OnReady()
    {
        cardActive = 0;
        cards.Clear();
        Utils.RemoveAllChildsIn(container);

        base.OnReady();
        string story_id = Data.Instance.storiesData.activeContent.id;
        GamesData.Content c = Data.Instance.gamesData.GetContent(story_id);
        content = c.GetContentFor(type, gameID);        

        TextsData.Content tipContent = Data.Instance.daysData.GetTip("tip_toca_la_imagen");

        Utils.Shuffle(content);

        int id = 0;
        foreach (string text in content)
        {
            SimpleButton sb = Instantiate(simonCard);
            AssetsData.Content assetContent = Data.Instance.assetsData.GetContent(text);
            if (assetContent == null || assetContent.sprite == null)
                Events.Log("No hay imagen para: " + text);
            else
            {
                Sprite sprite = assetContent.sprite;
                sb.Init(id, sprite, "", OnClicked);
            }
            id++;
            cards.Add(sb);
        }
        Utils.Shuffle(cards);
        foreach (SimpleButton sb in cards)
        {
            sb.transform.SetParent(container);
            sb.transform.localScale = Vector2.one;
        }


        Events.OnCharacterSay(tipContent, OnTipDone, tipContent.character_type);
    }
    void Animate(string clipName)
    {
        foreach (SimpleButton sb in cards)
            sb.GetComponent<Animation>().Play(clipName);
    }
    void OnClicked(SimpleButton button)
    {
        if (!canSelect) return;

        signal.SetActive(false);
        Animate("allOn");

        if (button.id == cardActive)
        {
            button.GetComponent<SimpleFeedback>().SetState(SimpleFeedback.states.OK, 2);
        }
        else
        {
            button.GetComponent<SimpleFeedback>().SetState(SimpleFeedback.states.WRONG, 2);
        }
        canSelect = false;
        cardActive++;
        if (cardActive >= content.Count)
        {
            cardActive = 0;
            Events.SetReadyButton(OnReadyClicked);
        }

        Events.PlaySoundTillReady("voices", "assets/audio/" + Data.Instance.assetsData.GetAssetRealName(content[button.id]), NextWord);
       
    }
    void NextWord()
    {
        Invoke("SayWords", 2);
    }
    void OnTipDone()
    {       
        SayWords();
    }
    void CanSelect()
    {
        canSelect = true;
        Animate("rotateRightLeft");
    }
    void OnReadyClicked()
    {
        CancelInvoke();
        OnComplete();
        Events.OnGoto(true);
    }
   
    public void SayWords()
    {
        signal.SetActive(true);
        int cID = cardActive;
        string text_id = content[cID];
       // Events.PlaySoundTillReady("voices", "frases/" + text_id, WordSaid);

        string assetRealName = Data.Instance.assetsData.GetAssetRealName(text_id);
        Events.PlaySoundTillReady("voices", "assets/audio/" + assetRealName, WordSaid);

        TextsData.Content c = Data.Instance.textsData.GetContent("frase_" + text_id);
        if (c != null)
            field.text = c.text;
    }
    void WordSaid()
    {
        CanSelect();
    }
}
