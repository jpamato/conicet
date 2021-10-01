using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loro_Multiple : ScreenMain
{
    GamesData.Content content;
    int id = 0;
    public Text field;
    public SimpleButton simonCard;
    public List<SimpleButton> cards;

    public List<string> ok_words;
    public List<string> wrong_words;

    public Transform container;
    public GameObject signal;
    public int cardID;
    bool canSelect;
    int ok;
    string firstWord;

    private void OnEnable()
    {
        field.text = "";
        cards.Clear();
        Utils.RemoveAllChildsIn(container);
        signal.SetActive(false);
    }
    public override void OnReady()
    {
        Utils.RemoveAllChildsIn(container);
        ok = 0;
        base.OnReady();
        string story_id = Data.Instance.storiesData.activeContent.id;
        content = Data.Instance.gamesData.GetContent(story_id);
        if (content == null) return;
        field.text = "";
        TextsData.Content tipContent = Data.Instance.daysData.GetTip("toca_empiezan_igual");
        Events.OnCharacterSay(tipContent, OnTipDone, tipContent.character_type);
        int id = 0;
        bool isOk = true;
        foreach (string text in content.loro_multiple)
        {
            if (id == 0)
            {
                firstWord = text;
            }
            else if (text == "-")
                isOk = false;
            else
            {
                if (isOk)
                    ok_words.Add(text);
                else
                    wrong_words.Add(text);
                print(isOk + " : " + text);
                SimpleButton sb = Instantiate(simonCard);
                sb.transform.localScale = Vector2.one;
                Sprite sprite = Data.Instance.assetsData.GetContent(text).sprite;
                sb.Init(id, sprite, text, OnClicked);               
                cards.Add(sb);
            }
            id++;
        }
        Utils.Shuffle(cards);
        foreach(SimpleButton sb in cards)
        {
            sb.transform.SetParent(container);
            sb.transform.localScale = Vector2.one;
        }
    }
    void Animate(string clipName)
    {
        foreach (SimpleButton sb in cards)
            sb.GetComponent<Animation>().Play(clipName);
    }
    void OnClicked(SimpleButton button)
    {
    //    if (!canSelect) return;
    //    canSelect = false;
        if (IsOk(button.text))
        {
            button.GetComponent<SimpleFeedback>().SetState(SimpleFeedback.states.OK, 60);
            SetResults(true);
            button.InactivateFor(60);
        }
        else
        {
            button.GetComponent<SimpleFeedback>().SetState(SimpleFeedback.states.WRONG, 60);
            SetResults(false);
        }
        SayAsset(button.text);
    }
    void SayAsset(string assetRealName)
    {
        Events.PlaySoundTillReady("voices", "assets/audio" + Utils.GetLangFolder() + "/" + assetRealName, null);
    }
    bool IsOk(string text)
    {
        foreach (string s in ok_words)
            if (s == text)
                return true;
        return false;
    }
    void SetResults(bool isOk)
    {
        if (isOk)
        {
            ok++;
            if (ok >= ok_words.Count)
            {
                Invoke("AllDone", 0.5f);
            }
        }
        id++;

        StartCoroutine(CheckResults());
        signal.SetActive(false);
    }
    void AllDone()
    {
        Events.SetReadyButton(OnReadyClicked);
    }
    void OnReadyClicked()
    {
        OnComplete();
        Events.OnGoto(true);
    }
    IEnumerator CheckResults()
    {
        yield return new WaitForSeconds(1);
        SetCard();
    }
    void OnTipDone()
    {
        id = 0;
        SetCard();
        SayWord();
    }
    void SetCard()
    {
        signal.SetActive(true);
        id++;
        //cardID = Random.Range(0, content.loro_time.Count);
        
    }
    public void SayWord()
    {
        string text_id = firstWord;
        print("__________ cardID: " + text_id);
        string assetRealName = Data.Instance.assetsData.GetAssetRealName(text_id);
        Events.PlaySoundTillReady("voices", "assets/audio" + Utils.GetLangFolder() + "/loro_" + assetRealName, null);
        field.text = text_id;
        Invoke("CanSelect", 0.5f);
    }
    void CanSelect()
    {
        if (gameObject.activeSelf)
            Animate("rotateRightLeft");
        canSelect = true;
    }
}
