using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Contar : ScreenMain
{
    public List<string> content;
    int ok = 0;
    public SimpleButton imageCard;
    public SimpleButton card;
    public List<SimpleButton> allButtons;
    public Transform imagesContainer;
    public Transform container;
    public Sprite sprite;
    int totalObjects;
    bool clicked;

    public override void Hide(bool toLeft)
    {
        base.Hide(toLeft);
        Utils.RemoveAllChildsIn(container);
    }
    private void OnEnable()
    {
        Utils.RemoveAllChildsIn(imagesContainer);
        Utils.RemoveAllChildsIn(container);
    }
    TextsData.Content tipContent;
    public override void OnReady()
    {

        Utils.RemoveAllChildsIn(imagesContainer);
        Utils.RemoveAllChildsIn(container);
        base.OnReady();
        string story_id = Data.Instance.storiesData.activeContent.id;

        GamesData.Content c = Data.Instance.gamesData.GetContent(story_id);
        content = c.GetContentFor(type, gameID);

        if (content == null) return;
        clicked = false;
        tipContent = Data.Instance.daysData.GetTip("tip_contar");

        Events.OnCharacterSay(tipContent, OnTipDone, tipContent.character_type);
    }
    void OnTipDone()
    {
        allButtons.Clear();
        int id = 0;
        foreach (string text in content)
        {
            if (id == 0)
            {
                sprite = Data.Instance.assetsData.GetContent(text).sprite;
                if (sprite == null)
                    Events.Log("No hay asset para " + text);
            }
            else if (id == 1)
            {
                string[] arr = text.Split(","[0]);
                int id2 = 1;
                foreach (string s in arr)
                {
                    if (id2 == 1)
                        totalObjects = int.Parse(s);
                    SetLetter(id2, s);
                    id2++;
                }
            }
            else if (id == 2)
            {
              
            }
            id++;
        }
        AddImages();
        AddButtons();
    }
    void SetLetter(int id, string letter)
    {
        SimpleButton sb = Instantiate(card);
        sb.Init(id, null, letter, OnClicked);
        allButtons.Add(sb);
    }
    void AddImages()
    {
        for (int a= 0; a<totalObjects; a++)
        {
            SimpleButton sb = Instantiate(imageCard, imagesContainer);
            sb.Init(a, sprite, "", null);
            sb.transform.localScale = Vector2.one;
            sb.image.transform.localPosition = new Vector2(0, UnityEngine.Random.Range(-90, 90));
        }
    }
    void AddButtons()
    {
        Utils.Shuffle(allButtons);
        foreach (SimpleButton sb in allButtons)
        {
            sb.transform.SetParent(container);
            sb.transform.localScale = Vector2.one;
        }
    }
    bool IsOk(int id)
    {
        return false;
    }
    void OnClicked(SimpleButton button)
    {
        if (clicked) return;
        clicked = true;
        if (button.id == 1)
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
