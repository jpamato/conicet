using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FallingObjects : ScreenMain
{
    GamesData.Content content;
    int ok = 0;
    public Text field;
    public Text numField;
    public SimpleButton card;
    public List<FallingCard> cards;
    public Transform container;
    public GameObject signal;
    public int cardID;
    public float fallingSpeed = 10;

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

    private void OnEnable()
    {
        cards.Clear();
        Utils.RemoveAllChildsIn(container);
        signal.SetActive(false);
    }
    public override void OnReady()
    {
        numField.text = "";
        base.OnReady();
        string story_id = Data.Instance.storiesData.activeContent.id;
        content = Data.Instance.gamesData.GetContent(story_id);
        if (content == null) return;
        field.text = "";

        TextsData.Content tipContent = Data.Instance.daysData.GetTip("tip_falling_objects");

        Events.OnCharacterSay(tipContent, OnTipDone);
        int id = 0;
        foreach (string text in content.fallingObjects)
        {
            SimpleButton sb = Instantiate(card, container);
            sb.transform.localScale = Vector2.one;
            Sprite sprite = Data.Instance.assetsData.GetContent(text).sprite;
            sb.Init(id, sprite, "", OnClicked);
            id++;
            FallingCard fc = new FallingCard();
            fc.asset = sb;
            cards.Add(fc);
            InitCard(fc);
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
    void OnClicked(SimpleButton button)
    {
        if (button.id == cardID)
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
        cardID = 0;// UnityEngine.Random.Range(0, content.fallingObjects.Count);
        SayWord();
    }
    public void SayWord()
    {
        string text_id = content.fallingObjects[cardID];
        Events.PlaySoundTillReady("voices", "assets/" + text_id, null);
        field.text = text_id;
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
