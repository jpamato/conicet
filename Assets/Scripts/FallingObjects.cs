using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallingObjects : ScreenMain
{
    GamesData.Content content;
    int id = 0;
    public Text field;
    public SimpleButton card;
    public List<SimpleButton> cards;
    public Transform container;
    public GameObject signal;
    public int cardID;
    bool canSelect;

    private void OnEnable()
    {
        cards.Clear();
        Utils.RemoveAllChildsIn(container);
        signal.SetActive(false);
    }
    public override void OnReady()
    {
        base.OnReady();
        string story_id = Data.Instance.storiesData.activeContent.id;
        content = Data.Instance.gamesData.GetContent(story_id);
        if (content == null) return;
        field.text = "";
        TextsData.Content tipContent = Data.Instance.textsData.GetContent("escucha_juguete");
        Events.OnCharacterSay(tipContent, OnTipDone);
        int id = 0;
        foreach (string text in content.fallingObjects)
        {
            SimpleButton sb = Instantiate(card, container);
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
            Events.PlaySound("ui", "ui/feedback_ok", false);
            SetResults(true);
        }
        else
        {
            button.GetComponent<SimpleFeedback>().SetState(SimpleFeedback.states.WRONG, 2);
            Events.PlaySound("ui", "ui/feedback_bad", false);
            SetResults(false);
        }
        Animate("allOn");
    }
    void SetResults(bool isOk)
    {
        id++;
        if (id > 10)
        {
            OnComplete();
            Events.SetNextButton(true);
        }

        StartCoroutine(CheckResults());
        signal.SetActive(false);
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
        cardID = Random.Range(0, content.fallingObjects.Count);
        SayWord();
    }
    public void SayWord()
    {
        string text_id = content.fallingObjects[cardID];
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
