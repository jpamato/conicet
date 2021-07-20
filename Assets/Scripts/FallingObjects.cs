using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FallingObjects : ScreenMain
{
    public List<string> content;
    int ok = 0;
    public Text field;
    public Text numField;
    public SimpleButton card;
    public List<FallingCard> cards;
    public Transform container;
    public GameObject signal;
    public List<int> correctCards;
    public float fallingSpeed = 10;
    public types typeOfGame;
    public enum types
    {
        SINGLE,
        MULTIPLE
    }
    [Serializable]
    public class FallingCard
    {
        public SimpleButton asset;
        public float speed;
        public float direction;
        public bool clicked;
        System.Action<FallingCard> OnDone;

        public IEnumerator Clicked(System.Action<FallingCard> OnDone)
        {
            clicked = true;
            yield return new WaitForSeconds(2);
            OnDone(this);
        }
    }
    public override void Hide(bool toLeft)
    {
        signal.SetActive(false);
        base.Hide(toLeft);
        cards.Clear();
        correctCards.Clear();
        Utils.RemoveAllChildsIn(container);
    }
    private void OnEnable()
    {
        cards.Clear();
        Utils.RemoveAllChildsIn(container);
        signal.SetActive(false);
    }
    TextsData.Content tipContent;
    public override void OnReady()
    {
        Utils.RemoveAllChildsIn(container);
        correctCards.Clear();
        numField.text = "";
        base.OnReady();
        string story_id = Data.Instance.storiesData.activeContent.id;

        GamesData.Content c = Data.Instance.gamesData.GetContent(story_id);
        content = c.GetContentFor(type, gameID);


        if (content == null) return;
        field.text = "";

        tipContent = Data.Instance.daysData.GetTip("tip_falling_objects");

        Events.OnCharacterSay(tipContent, OnTipDone, tipContent.character_type);
        int id = 0;
        bool isCorrect = true;
        foreach (string text in content)
        {
            if (text == "-")
            {
                typeOfGame = types.MULTIPLE;
                isCorrect = false;
            }
            else {
                if (isCorrect)
                    correctCards.Add(id);
                print(text + " isCorrect: " + isCorrect);
                SimpleButton sb = Instantiate(card, container);
                sb.transform.localScale = Vector2.one;
                Sprite sprite = Data.Instance.assetsData.GetContent(text).sprite;
                sb.Init(id, sprite, text, OnClicked);
                id++;
                FallingCard fc = new FallingCard();
                fc.asset = sb;
                cards.Add(fc);
                InitCard(fc);
            }
        }
    }
   
    void Animate(string clipName)
    {
        foreach (FallingCard fc in cards)
            fc.asset.GetComponent<Animation>().Play(clipName);
    }
    FallingCard GetFallingObject(SimpleButton asset)
    {
        foreach (FallingCard fc in cards)
            if (fc.asset == asset)
                return fc;
        return null;
    }
    bool IsOk(int id)
    {
        if (typeOfGame == types.SINGLE)
        {
            if (id == 0)
                return true;
            else
                return false;
        }
        foreach (int thisID in correctCards)
            if (id == thisID)
                return true;
        return false;
    }
    void OnClicked(SimpleButton button)
    {
        string assetRealName = Data.Instance.assetsData.GetAssetRealName(button.text);
        Events.PlaySoundTillReady("voices", "assets/audio/" + assetRealName, null);
        button.transform.localEulerAngles = new Vector3(0, 0, 0);
        button.InactivateFor(3.5f);
        if (IsOk(button.id))
        {
            button.GetComponent<SimpleFeedback>().SetState(SimpleFeedback.states.OK, 2);            
            SetResults(true);
        }
        else
        {
            button.GetComponent<SimpleFeedback>().SetState(SimpleFeedback.states.WRONG, 2);
            SetResults(false);
        }
        StartCoroutine( GetFallingObject(button).Clicked(OnDoneFallingObjectClicked) );
    }
    void OnDoneFallingObjectClicked(FallingCard fc )
    {
        fc.clicked = false;
        InitCard(fc);
        fc.asset.GetComponent<SimpleFeedback>().SetOff();
    }
    void SetResults(bool isOk)
    {
        if(isOk)
            ok++;
        numField.text = ok.ToString();
        if (ok == 5)
            Events.SetReadyButton(OnReadyClicked);
    }

    void OnReadyClicked()
    {
        OnComplete();
        Events.OnGoto(true);
    }
    void OnTipDone()
    {
        ok = 0;
        SetCard();
    }
    void SetCard()
    {
        signal.SetActive(true);
        SetText();
    }
    public void SayWord()
    {
        if(typeOfGame == types.MULTIPLE)
            Events.PlaySoundTillReady("voices", "genericTexts/" + tipContent.id, null);
        else
        {
            string assetRealName = Data.Instance.assetsData.GetAssetRealName(content[0]);
            Events.PlaySoundTillReady("voices", "assets/audio/" + assetRealName, null);
        }
        SetText();
    }
    void SetText()
    {
        signal.SetActive(true);
        if (typeOfGame == types.MULTIPLE)
            field.text = tipContent.text;
        else
            field.text = content[0];
    }
    private void Update()
    {
        foreach (FallingCard fc in cards)
            UpdatePos(fc);
    }
    void UpdatePos(FallingCard fc)
    {
        if (fc.clicked)
            return;
        Vector2 pos = fc.asset.transform.localPosition;
        pos.y -= fc.speed*Time.deltaTime * fallingSpeed;
        fc.asset.transform.localPosition = pos;
        if (fc.asset.transform.localPosition.y < -400)
            InitCard(fc);
    }
    void InitCard(FallingCard card)
    {
        card.speed = UnityEngine.Random.Range(8,12);
        card.direction = UnityEngine.Random.Range(-10, 10);
        card.asset.transform.localPosition = new Vector2(UnityEngine.Random.Range(-450, 450), UnityEngine.Random.Range(400, 550));
    }
}
