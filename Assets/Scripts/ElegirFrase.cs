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
    bool isFrase;

    private void OnEnable()
    {
        field.text = "";
        signal.SetActive(false);
    }
    public override void Show(bool fromRight)
    {
        base.Show(fromRight);
        cards.Clear();
        Utils.RemoveAllChildsIn(container);
    }
    public override void OnReady()
    {
        isFrase = false;
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
            string t = text;

            if (text.Contains("frase_"))
            {
                isFrase = true;
                t = text.Remove(0, 6);

                string[] arr = t.Split("_"[0]);
                if (arr.Length > 1) t = arr[0];
            }

            AssetsData.Content assetContent = Data.Instance.assetsData.GetContent(t);
            if (assetContent == null || assetContent.sprite == null)
                Events.Log("No hay imagen para: " + t);
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

        string t = Data.Instance.assetsData.GetAssetRealName(content[button.id]);
        if (t.Contains("frase_"))
            t = t.Remove(0, 6);
        string[] arr = t.Split("_"[0]);
        if (arr.Length > 1) t = arr[0];

        if (isFrase)
            Events.PlaySoundTillReady("voices", "assets/audio" + Utils.GetLangFolder() + "/" + t, NextWord);
        else
            NextWord();


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

        if (text_id.Contains("frase_"))
        {
            print("SayWords FRASE: " + text_id);
            Events.PlaySoundTillReady("voices", "frases"  + Utils.GetLangFolder() + "/" + text_id, WordSaid);
        }
        else
        {
            print("SayWords SIMPLE: " + text_id);
            string assetRealName = Data.Instance.assetsData.GetAssetRealName(text_id);
            Events.PlaySoundTillReady("voices", "assets/audio" + Utils.GetLangFolder() + "/" + assetRealName, WordSaid);
        }

        TextsData.Content c = Data.Instance.textsData.GetContent("frase_" + text_id);
        if (c != null)
            field.text = c.text;
    }
    void WordSaid()
    {
        CanSelect();
    }
}
