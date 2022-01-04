using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjetoDiferente : ScreenMain
{
    GamesData.Content content;
    int id = 0;
    public Text field;
    public SimpleButton simonCard;
    public List<SimpleButton> cards;

    public Transform container;
    public GameObject signal;
    bool canSelect;
    public string okWord;
    public string wrongWord;

    private void OnEnable()
    {
        field.text = "";
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
        signal.SetActive(false);
        field.text = "";
        cards.Clear();
        Utils.RemoveAllChildsIn(container);

        base.OnReady();
        string story_id = Data.Instance.storiesData.activeContent.id;
        content = Data.Instance.gamesData.GetContent(story_id);

        StoriesData.Content story_content = Data.Instance.storiesData.activeContent;
        GamesData.Content gameDataContent = Data.Instance.gamesData.GetContent(story_content.id);
        List<string> arr = gameDataContent.GetContentFor(type, gameID);

        field.text = "";
        TextsData.Content tipContent = Data.Instance.daysData.GetTip("cual_esDiferente");
        int id = 0;
        bool isOk = true;
        foreach (string text in arr)
        {
            if (id == 0)
            {
                wrongWord = text;
                AddCard(text); // agrega 2 bien:
            }
            else
                okWord = text;

            AddCard(text);            
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
    void AddCard(string text)
    {
        SimpleButton sb = Instantiate(simonCard);
        sb.transform.localScale = Vector2.one;
        Sprite sprite = Data.Instance.assetsData.GetContent(text).sprite;
        sb.Init(id, sprite, text, OnClicked);
        cards.Add(sb);
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
            canSelect = false;
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
      //  SayAsset(button.text, null);
    }
    void SayAsset(string assetRealName, System.Action OnSaid)
    {
        Events.PlaySoundTillReady("voices", "assets/audio" + Utils.GetLangFolder() + "/" + assetRealName, OnSaid);
    }
    bool IsOk(string text)
    {
        if (okWord == text)
            return true;
        return false;
    }
    void SetResults(bool isOk)
    {
        if (isOk)
        {
            Invoke("AllDone", 1f);
        }
        id++;

        StartCoroutine(CheckResults());
        signal.SetActive(false);
    }
    void AllDone()
    {
        Events.SetReadyButton(OnReadyClicked);
        Utils.RemoveAllChildsIn(container);
        cards.Clear();
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
        CanSelect();
    }
    void SetCard()
    {
        signal.SetActive(true);
        id++;
    }
    void CanSelect()
    {
        if (gameObject.activeSelf)
            Animate("rotateRightLeft");
        canSelect = true;
    }
}
