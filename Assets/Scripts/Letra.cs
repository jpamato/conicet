using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Letra : ScreenMain
{
    public List<string> content;
    int ok = 0;
    public Text field;
    string originalText;
    public SimpleButton card;
    public List<SimpleButton> allButtons;
    public Transform container;
    public Image image;
    bool clicked;
    string realWord;
    public AssetsData.loroWordsType loroWordsType;

    public override void Hide(bool toLeft)
    {
        base.Hide(toLeft);
        Utils.RemoveAllChildsIn(container);
    }
    private void OnEnable()
    {
        image.gameObject.SetActive(false);
        field.text = "";
        Utils.RemoveAllChildsIn(container);
    }
    TextsData.Content tipContent;
    public override void OnReady()
    {
        image.gameObject.SetActive(false);
        Utils.RemoveAllChildsIn(container);
        base.OnReady();
        string story_id = Data.Instance.storiesData.activeContent.id;

        GamesData.Content c = Data.Instance.gamesData.GetContent(story_id);
        content = c.GetContentFor(type, gameID);

        if (content == null) return;
        field.text = "";
        clicked = false;
        tipContent = Data.Instance.daysData.GetTip("tip_letra");

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
                originalText = text;
                SetOriginalText();
            }
            else if (id == 1)
            {
                string[] arr = text.Split(","[0]);
                int id2 = 1;
                foreach (string s in arr)
                {
                    SetLetter(id2, s);
                    id2++;
                }
            } else if(id==2)
            {
                ////////////////// por si la palabra del loro es default, inicio o final:
                string[] arr = text.Split("@"[0]);
                if (arr.Length > 1)
                {
                    realWord = arr[0];
                    loroWordsType = Data.Instance.assetsData.SetTypeByText(arr[1]);
                } else
                    realWord = text;
                //////////////////////////////////////////

                image.gameObject.SetActive(true);
                Sprite sprite = Data.Instance.assetsData.GetContent(realWord).sprite;
                if (sprite == null)
                    Events.Log("No hay asset para " + realWord);
                else
                    image.sprite = sprite;
                
            }
            id++;
        }
        AddButtons();
        SayWord();
    }
   
    public void SayWord()
    {
        string text_id = realWord;
        string assetRealName = Data.Instance.assetsData.GetAssetRealName(text_id);
        assetRealName = Data.Instance.assetsData.GetSoundForLoro(assetRealName, loroWordsType);
        Events.PlaySoundTillReady("voices", "assets/audio" + Utils.GetLangFolder() + "/loro_" + assetRealName, null);        
        Invoke("CanSelect", 0.5f);
    }
    void SetOriginalText()
    {
        field.text = originalText.ToUpper();
    }
    void SetTitle(string letter)
    {
        string st = originalText;
        string newWord = st.Replace("_", letter);
        field.text = newWord.ToUpper();
        field.GetComponent<Animation>().Play();
    }
    void SetLetter(int id, string letter)
    {
        SimpleButton sb = Instantiate(card);
        sb.Init(id, null, letter, OnClicked);
        allButtons.Add(sb);
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
        SetTitle(button.text);
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
        Invoke("SayWord", 0.5f);
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
        SetOriginalText();
    }
    void OnReadyClicked()
    {
        OnComplete();
        Events.OnGoto(true);
    }
   
}
