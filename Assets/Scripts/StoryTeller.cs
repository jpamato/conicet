using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryTeller : ScreenMain
{
    public Image image;
    public Text field;
    public Text[] titleField;
    StoriesData.Content content;
    public GameObject intro;
    public GameObject title;
    public FillAmountAnim introBar;
    public GameObject dialogueSignal;
    public Character character;
    public float characterBig = 0.7f;
    public float characterMedium = 0.55f;

    public override void OnEnable()
    {
        base.OnEnable();
        Events.OnNewKeyframeReached += OnNewKeyframeReached;
        intro.SetActive(true);
        title.SetActive(true);
        introBar.Init();
        SetTitle();
    }
    void SetTitle()
    {
        foreach(Text t in titleField)
            t.text = Data.Instance.storiesData.activeContent.name;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        Events.OnNewKeyframeReached -= OnNewKeyframeReached;
    }
    public override void OnOff()
    {
        print("on off");
        Events.StopAudioPlayer();
        Events.OnCharacterSay(null,null, CharactersManager.types.Brisa);
    }
    public override void OnReady()
    {
        base.OnReady();
        content = Data.Instance.storiesData.activeContent;
       
        if (content.varType == 1)
        {
            dialogueSignal.SetActive(false);
            character.transform.localScale = new Vector2(characterBig, characterBig);
        }            
        else
        {
            dialogueSignal.SetActive(true);
            character.transform.localScale = new Vector2(characterMedium, characterMedium);
        }
        character.Init(content.characterType);

        bool ignoreLang = false;
        if (Data.Instance.lang == Data.langs.QOM) ignoreLang = false;
        TextsData.Content tipContent = Data.Instance.textsData.GetContent("tip_read_automatic", ignoreLang);
        Events.OnCharacterSay(tipContent, OnTipDone, tipContent.character_type);
    }
    void OnTipDone()
    {
        introBar.AnimateOff(10);
        GetComponent<Animation>().Play("closeIntro");
        Events.SetAudioPlayer(content.audioClip, content.textsData, OnComplete);
        Invoke("CloseTitle", 4);
    }
    public override void OnComplete()
    {
        base.OnComplete();
        Events.SetReadyButton(OnNext);
    }
    void OnNext()
    {
        Events.OnGoto(true);
    }
    void CloseTitle()
    {
        title.SetActive(false);
    }
    private void Reset()
    {
        field.text = "";
        image.sprite = null;
    }
    void OnNewKeyframeReached(int id)
    {
        field.text = content.textsData[id].text;

        Sprite s = Resources.Load<Sprite>("stories/"+content.folder+"/images/"+(id+1));

        SetSprite(s);
    }
    void SetSprite(Sprite s)
    {
        image.sprite = s;

        float _w = s.texture.width;
        float _h = s.texture.height;

        float factor = _h / image.GetComponent<RectTransform>().sizeDelta.y;

        _w = _w / factor;

        RectTransform rTransform = image.GetComponent<RectTransform>();
        rTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _w);
        rTransform.anchoredPosition = new Vector3(-_w / 2, 0, 0);
        image.sprite = s;
    }
}
