using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionsScreen : ScreenMain
{
    QuestionsManager.Content content;
    int id = 0;
    public Text field;
    public Image image;

    public override void OnReady()
    {
        base.OnReady();
        string story_id = Data.Instance.storiesData.activeContent.id;
        content = Data.Instance.questionsManager.GetContent(story_id);
        if (content == null) return;
        field.text = "";
        TextsData.Content tipContent = Data.Instance.textsData.GetContent("questions_tip");
        Events.OnCharacterSay(tipContent, OnTipDone);
    }
    void OnTipDone()
    {
        id = 0;
        SetCard();
    }
    void SetCard()
    {
        string text_id = content.texts[id];
        Events.PlaySoundTillReady("voices", "genericTexts/" + text_id, OnTextDone);
        field.text = (id+1) + ". " + Data.Instance.textsData.GetContent(text_id).text;
    }
    void OnTextDone()
    {
        Events.SetReadyButton(ButtonClicked);
    }
    void ButtonClicked()
    {
        id++;
        if (id >= content.texts.Count)
        {
            OnComplete();
            Events.OnGoto(true);
        } else
        {            
            SetCard();
        }
    }
}
