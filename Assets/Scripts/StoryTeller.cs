using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryTeller : ScreenMain
{
    public Image image;
    public Text field;
    StoriesData.Content content;

    private void OnEnable()
    {
        Events.OnNewKeyframeReached += OnNewKeyframeReached;
        Events.OnBack += OnBack;
    }
    private void OnDisable()
    {
        Events.OnNewKeyframeReached -= OnNewKeyframeReached;
        Events.OnBack -= OnBack;
    }
    void OnBack()
    {
        Events.StopAudioPlayer();
        Open(types.STORIES_SELECTOR);
    }
    public override void Show()
    {
        base.Show();
        Events.SetBackButton(true);
        content = Data.Instance.storiesData.activeContent;
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
        Open(types.STORIES_SELECTOR);
    }
}
