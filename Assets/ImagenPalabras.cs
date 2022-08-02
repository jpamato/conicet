using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ImagenPalabras : ScreenMain
{
    public List<string> content;
    int ok = 0;
    public Image imageCard;
    public List<SimpleButton> allButtons;
    int totalObjects;
    [SerializeField] string ok_word;
    bool clicked;

    public override void Hide(bool toLeft)
    {
        base.Hide(toLeft);
    }
    private void OnEnable()
    {
        ShowContent(false);
    }
    void ShowContent(bool hide)
    {
        foreach (SimpleButton s in allButtons)
            s.gameObject.SetActive(hide);
        imageCard.gameObject.SetActive(hide);
    }

    TextsData.Content tipContent;
    public override void OnReady()
    {
        ShowContent(false);

        base.OnReady();
        string story_id = Data.Instance.storiesData.activeContent.id;

        GamesData.Content c = Data.Instance.gamesData.GetContent(story_id);
        content = c.GetContentFor(type, gameID);

        if (content == null) return;
        clicked = false;
        tipContent = Data.Instance.daysData.GetTip("toca_dice");
        Events.OnCharacterSay(tipContent, OnTipDone, tipContent.character_type);
    }
    void OnTipDone()
    {
        ShowContent(true);
        ok_word = content[0];
        Say(ok_word.ToLower());
        Sprite sprite = Data.Instance.assetsData.GetContent(ok_word).sprite;
        imageCard.sprite = sprite;
        if (sprite == null)    Events.Log("No hay asset para " + content[0]);
        AddButtons();
    }
    public void Say(string audioName)
    {
        Events.PlaySound("voices", "assets/audio" + Utils.GetLangFolder() + "/" + audioName, false);
    }
    void AddButtons()
    {
        Utils.Shuffle(content);
        int id = 0;
        foreach (SimpleButton sb in allButtons)
        {
            sb.Init(0, null, content[id], OnClicked, false);
            id++;
        }
    }
    bool IsOk(int id)
    {
        return false;
    }
    void OnClicked(SimpleButton button)
    {
        Say(button.text);

        if (clicked) return;
        clicked = true;
        if (button.text == ok_word)
        {
            button.GetComponent<SimpleFeedback>().SetState(SimpleFeedback.states.OK, 2);
            Invoke("OnCorrect", 1);
        }
        else
        {
            button.GetComponent<SimpleFeedback>().SetState(SimpleFeedback.states.WRONG, 2);
            Invoke("ResetLetters", 2);
        }
    }
    void OnCorrect()
    {
        Events.SetReadyButton(OnReadyClicked);
    }
    void ResetLetters()
    {
        clicked = false;
        foreach (SimpleButton sb in allButtons)
            sb.GetComponent<SimpleFeedback>().SetOff();
    }
    void OnReadyClicked()
    {
        OnComplete();
        Events.OnGoto(true);
    }

}
