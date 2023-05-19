using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chico_Multiple : ScreenMain
{
    GamesData.Content content;
    int id = 0;
    public SimpleButton simonCard;
    public List<SimpleButton> cards;

    public List<string> ok_words;
    public List<string> wrong_words;

    public Transform container;
    public GameObject signal;
    bool canSelect;
    public int ok;

    public AssetsData.loroWordsType loroWordsType;

    private void OnEnable()
    {
        cards.Clear();
        Utils.RemoveAllChildsIn(container);
        signal.SetActive(false);
    }
    public override void Show(bool fromRight)
    {
        base.Show(fromRight);
        canSelect = false;
        Utils.RemoveAllChildsIn(container);
    }
    public override void OnReady()
    {
        ok_words.Clear();
        wrong_words.Clear();
        signal.SetActive(false);
        cards.Clear();
        Utils.RemoveAllChildsIn(container);
        ok = 0;
        base.OnReady();
        string story_id = Data.Instance.storiesData.activeContent.id;
        content = Data.Instance.gamesData.GetContent(story_id);

        StoriesData.Content story_content = Data.Instance.storiesData.activeContent;
        GamesData.Content gameDataContent = Data.Instance.gamesData.GetContent(story_content.id);
        List<string> arr = gameDataContent.GetContentFor(type, gameID);

        TextsData.Content tipContent = Data.Instance.daysData.GetTip("toca_empiezan_igual");
        int id = 0;
        bool isOk = true;
        foreach (string text in arr)
        {
            if (text == "-" || text.Contains("-"))
                isOk = false;
            else
            {
                string _text = GetParsedString(text);
                if (isOk)
                    ok_words.Add(_text);
                else
                    wrong_words.Add(_text);
                SimpleButton sb = Instantiate(simonCard);
                sb.transform.localScale = Vector2.one;

                // string _text = GetParsedString(text);
                Sprite sprite = Data.Instance.assetsData.GetContent(_text).sprite;
                sb.Init(id, sprite, _text, OnClicked);
                cards.Add(sb);
            }
            id++;

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
        if (button == null) return;
        if (!canSelect) return;
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
        SayAsset(button.text, null);
    }
    void SayAsset(string assetRealName, System.Action OnSaid)
    {
        assetRealName = Data.Instance.assetsData.GetSoundForLoro(assetRealName, loroWordsType);
        Events.PlaySoundTillReady("voices", "assets/audio" + Utils.GetLangFolder() + "/" + assetRealName, OnSaid);
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
                Invoke("AllDone", 2.5f);
                ok = 0;
            }
        }
        id++;

        StartCoroutine(CheckResults());
        signal.SetActive(false);
    }
    void AllDone()
    {
        Events.SetReadyButton(OnReadyClicked);
        //Utils.RemoveAllChildsIn(container);
        canSelect = false;
        foreach (SimpleButton sb in cards)
            sb.GetComponent<Animation>().Stop("rotateRightLeft");
        signal.SetActive(false);
        cards.Clear();
    }
    void OnReadyClicked()
    {
        Utils.RemoveAllChildsIn(container); // TODO: Test. Not sure if next activity needs it to be empty.
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
        SayWord();
        SetCard();
    }
    void SetCard()
    {
        signal.SetActive(true);
        id++;

    }
    public void Repeat()
    {
        TextsData.Content tipContent = Data.Instance.daysData.GetTip("toca_empiezan_igual");
        Events.PlaySound("voices", "genericTexts" + Utils.GetLangFolder() + "/" + tipContent.id, false);
        Invoke("SayWord", 2.5f);
    }
    public void SayWord()
    {      
        sayID = 0;
        SayLoop();
    }
    int sayID = 0;
    void SayLoop()
    {
        if (sayID >= cards.Count)
            CanSelect();
        else
            Invoke("Delayed", 0.1f);
    }
    void Delayed()
    {
        print("sayID " + sayID + " cards.Count: " + cards.Count);
        SayAsset(cards[sayID].text, SayLoop);
        cards[sayID].GetComponent<Animation>().Play("allOn");
        sayID++;
    }
    void CanSelect()
    {
        if (gameObject.activeSelf)
            Animate("rotateRightLeft");
        canSelect = true;
    }
    ////////////////// por si la palabra del loro es default, inicio o final:
    string GetParsedString(string text_id)
    {
        string[] arr = text_id.Split("@"[0]);
        if (arr.Length > 1)
        {
            text_id = arr[0];
            loroWordsType = Data.Instance.assetsData.SetTypeByText(arr[1]);
        }
        return text_id;
    }
}
