using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionsScreen : ScreenMain
{
    public List<string> content;
    int num = 0;
    public Text field;
    public Image image;
    public GameObject intro;
    public FillAmountAnim introImage;
    public Character character;

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
        GamesData.Content c = Data.Instance.gamesData.GetContent(story_id);
        content = c.GetContentFor(type, gameID);
        if (content == null) return;
        field.text = "";

        bool lang = false;
        if (Data.Instance.lang == Data.langs.QOM) lang = true;
        TextsData.Content tipContent = Data.Instance.textsData.GetContent("tip_questions", lang);
        character.Init(tipContent.character_type);
        Events.OnCharacterSay(tipContent, OnTipDone);        
    } 
    void OnTipDone()
    {
        introImage.Init();
        introImage.AnimateOff();
        num = 0;
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
        string text_id = content[num];
        Events.PlaySoundTillReady("voices", "genericTexts/" + text_id, OnTextDone);
        field.text = Data.Instance.textsData.GetContent(text_id, false).text;
    }
    void OnTextDone()
    {
        Events.SetReadyButton(ButtonClicked);
    }
    void ButtonClicked()
    {
        num++;
        if (num >= content.Count)
        {
            OnComplete();
            Events.OnGoto(true);
        } else
        {            
            SetCard();
        }
    }
}
