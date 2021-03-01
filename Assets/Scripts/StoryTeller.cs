using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryTeller : ScreenMain
{
    public Image image;
    public Text field;
    StoriesData.Content content;

    public override void OnEnable()
    {
        base.OnEnable();
        Events.OnNewKeyframeReached += OnNewKeyframeReached;
        
    }
    public override void OnDisable()
    {
        base.OnDisable();
        Events.OnNewKeyframeReached -= OnNewKeyframeReached;
    }
    public override void OnBack()
    {
        Events.StopAudioPlayer();
        Open(types.DAY);
    }
    public override void Show()
    {
        base.Show();
        Events.SetBackButton(true);
        content = Data.Instance.storiesData.activeContent;
        TextsData.Content tipContent = Data.Instance.textsData.GetContent("escucha_maestra");
        Events.OnCharacterSay(tipContent, OnTipDone);
    }
    void OnTipDone()
    {
        Events.SetAudioPlayer(content.audioClip, content.textsData, OnReady);
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
    void OnReady()
    {
        Open(types.DAYS_SELECTOR);
    }
}
