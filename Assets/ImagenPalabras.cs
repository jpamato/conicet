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
    bool mayus;

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

        // la actividad se debe repetir dos veces, primero en minuscula y segundo en mayuscula (salvo en L1 - tip leer)
        if (tipContent.id != "tip_leer")
        {
            if (gameID > 0 && gameID % 2 == 0)
            {
                mayus = true;
            }
            else
            {
                mayus = false;
            }
        }
        else
            mayus = false;
        
        Sprite sprite = Data.Instance.assetsData.GetContent(ok_word).sprite;
        imageCard.sprite = sprite;
        if (sprite == null) Events.Log("No hay asset para " + content[0]);
        AddButtons();

        StartCoroutine(RepeatAndSay());
        
    }
    IEnumerator RepeatAndSay()
    {
        Events.PlaySound("voices", "genericTexts" + Utils.GetLangFolder() + "/" + "toca_dice", false);
        yield return new WaitForSeconds(1.5f);
        Say(ok_word);
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
            if (mayus)
                sb.Init(0, null, content[id], OnClicked, true);
            else
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
        Debug.Log("ok word" + ok_word + "button word" + button.text);
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
