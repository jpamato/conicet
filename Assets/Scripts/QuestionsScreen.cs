using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionsScreen : ScreenMain
{
    GamesData.Content content;
    int id = 0;
    public Text field;
    public Image image;
    public GameObject intro;
    public FillAmountAnim introImage;

    public override void OnEnable()
    {
        intro.SetActive(true);
        introImage.Init();
        base.OnEnable();
    }
    public override void OnReady()
    {
        base.OnReady();
        string story_id = Data.Instance.storiesData.activeContent.id;
        content = Data.Instance.gamesData.GetContent(story_id);
        if (content == null) return;
        field.text = "";
        TextsData.Content tipContent = Data.Instance.textsData.GetContent("tip_questions");
        Events.OnCharacterSay(tipContent, OnTipDone);        
    } 
    void OnTipDone()
    {
        introImage.Init();
        introImage.AnimateOff();
        id = 0;
        SetCard();
        Loop();
    }
    int imageID = 0;
    void Loop()
    {
        StoriesData.Content sContent = Data.Instance.storiesData.activeContent;
        if (imageID >= sContent.textsData.Count) imageID = 0;
        Sprite sprite = Resources.Load<Sprite>("stories/" + sContent.folder + "/images/" + (imageID + 1));
        image.sprite = sprite;
        Invoke("Loop", 4);
        imageID++;
    }
    void SetCard()
    {
        string text_id = content.questions[id];
        Events.PlaySoundTillReady("voices", "genericTexts/" + text_id, OnTextDone);
        field.text = (id+1) + ". " + Data.Instance.textsData.GetContent(text_id, true).text;
    }
    void OnTextDone()
    {
        Events.SetReadyButton(ButtonClicked);
    }
    void ButtonClicked()
    {
        id++;
        if (id >= content.questions.Count)
        {
            OnComplete();
            Events.OnGoto(true);
        } else
        {            
            SetCard();
        }
    }
}
