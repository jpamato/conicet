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
        Events.OnCharacterSay(null,null);
    }
    public override void OnReady()
    {
        Events.SetBackButton(true);
        content = Data.Instance.storiesData.activeContent;
        TextsData.Content tipContent = Data.Instance.textsData.GetContent("escucha_maestra");
        Events.OnCharacterSay(tipContent, OnTipDone);
    }
    void OnTipDone()
    {
        introBar.AnimateOff(10);
        GetComponent<Animation>().Play("closeIntro");
        Events.SetNextButton(true);
        Events.SetAudioPlayer(content.audioClip, content.textsData, null);
        Invoke("CloseTitle", 4);
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
        image.sprite = Resources.Load<Sprite>("stories/"+content.folder+"/images/"+(id+1));
    }
}
