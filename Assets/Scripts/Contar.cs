﻿using System.Collections;
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
    public override void Show(bool fromRight)
    {
        base.Show(fromRight);
        Utils.RemoveAllChildsIn(imagesContainer);
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
        container.gameObject.SetActive(false);
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
        StartCoroutine( AddImagesC());
        AddButtons();
    }
    void SetLetter(int id, string letter)
    {
        SimpleButton sb = Instantiate(card);
        sb.Init(id, null, letter, OnClicked);
        allButtons.Add(sb);
    }
    IEnumerator AddImagesC()
    {
        for (int a= 0; a<totalObjects; a++)
        {
            SimpleButton sb = Instantiate(imageCard, imagesContainer);
            sb.Init(a, sprite, "", null);
            sb.transform.localScale = Vector2.one;
            sb.image.transform.localPosition = new Vector2(0, UnityEngine.Random.Range(-90, 90));
            SayNumber(a + 1);
            yield return new WaitForSeconds(2);
        }
        container.gameObject.SetActive(true);
    }
    void SayNumber(int num)
    {
        string add = "";
        string suffix = "";
        if (Data.Instance.lang == Data.langs.QOM)
        {
            add = "_qom";
        }
        else
        {
            // Add string suffix to define audio file that should be played
            if (num == 1 && isFemenineNoun(content[0]))
                suffix = "a";
        }

        Events.PlaySound("voices", "genericTexts" + add + "/0" + num + suffix, false);
    }

    // Check if the word is a femenine or masculine noun
    // NOTE: No es 100% preciso a nivel de lenguaje español la relación entre género sustantivo y terminación de la palabra (-"a", -"e").
    bool isFemenineNoun(string _word)
    {
        // Cuando el tip es contar, la consigna define como sustantivo la palabra "objeto".
        if (tipContent.id == "tip_contar")
            _word = "objeto";

        string word = Data.Instance.assetsData.GetRealText(_word);

        // Take only the first word in cases where words are followed by adjectives (i.e. bolita verde)
        if (word.Contains(" "))
            word = word.Split(' ')[0];

        // Add femenine suffix if noun is femenine (defined by ending in "-a"
        if (word.EndsWith("a"))
            return true;

        // Add femenine suffix if noun is femenine (defined by ending in "-e")
        if (word.EndsWith("e"))
        {
            if (word.Equals("nube")) // only current exception in the game. New cases should be added here.
                return true;
        }

        return false;
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
        int num = 0;
        int.TryParse(button.field.text, out num);
        if(num != 0)
            SayNumber(num);

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
