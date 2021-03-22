using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoroWithTime : ScreenMain
{
    GamesData.Content content;
    int id = 0;
    public Text field;
    public SimpleButton simonCard;
    public List<SimpleButton> cards;
    public Transform container;
    public GameObject signal;
    public int cardID;
    bool canSelect;
    int ok;

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

        bool lang = false;
        if (Data.Instance.lang == Data.langs.QOM) lang = true;
        TextsData.Content tipContent = Data.Instance.textsData.GetContent("tip_loro_time", lang);
        Events.OnCharacterSay(tipContent, OnTipDone);
        int id = 0;
        foreach (string text in content.simons)
        {
            SimpleButton sb = Instantiate(simonCard, container);
            sb.transform.localScale = Vector2.one;
            Sprite sprite = Data.Instance.assetsData.GetContent(text).sprite;
            sb.Init(id, sprite, "", OnClicked);
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
        canSelect = false;
        if (button.id == cardID)
        {
            button.GetComponent<SimpleFeedback>().SetState(SimpleFeedback.states.OK, 2);
            SetResults(true);
        }
        else
        {
            button.GetComponent<SimpleFeedback>().SetState(SimpleFeedback.states.WRONG, 2);
            SetResults(false);
        }
        Animate("allOn");
    }
    void SetResults(bool isOk)
    {
        if (isOk)
        {
            ok++;
            if (ok > 5)
                Events.SetReadyButton(OnReadyClicked);
        }
        id++;

        StartCoroutine(CheckResults());
        signal.SetActive(false);
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
    }
    void SetCard()
    {
        signal.SetActive(true);
        id++;
        cardID = Random.Range(0, content.simons.Count);
        SayWord();
    }
    public void SayWord()
    {
        string text_id = content.simons[cardID];
        Events.PlaySoundTillReady("voices", "assets/" + text_id, CanSelect);
        field.text = text_id;
    }
    void CanSelect()
    {
        if (gameObject.activeSelf)
            Animate("rotateRightLeft");
        canSelect = true;
    }
}
