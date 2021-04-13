using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Paint : ScreenMain
{
    public DrawingScreen drawing;
    public FillAmountAnim introImage;
    public SimpleButton deleteButton;

    private void Awake()
    {
        deleteButton.Init(0, null, "", OnClicked);
    }
    void OnClicked(SimpleButton sb)
    {
        if (sb.id == 0)
            drawing.Undo();
    }
    public override void OnEnable()
    {
        introImage.Init();
        base.OnEnable();
        drawing.SetOff();
    }
    public override void OnDisable()
    {
        base.OnDisable();
    }
    public override void OnOff()
    {
        Events.StopAudioPlayer();
        Events.OnCharacterSay(null, null, CharactersManager.types.Brisa);
        drawing.SetOff();
    }
    public override void OnReady()
    {
        base.OnReady();
        TextsData.Content tipContent = Data.Instance.daysData.GetTip("tip_paint");
        Events.OnCharacterSay(tipContent, OnTipDone, tipContent.character_type);        
    }
    public override void Hide(bool toLeft)
    {
        CancelInvoke();
        base.Hide(toLeft);        
    }
    void OnTipDone()
    {
        string story_id = Data.Instance.storiesData.activeContent.id;
        GamesData.Content c = Data.Instance.gamesData.GetContent(story_id);

        string content = c.GetContentFor(type, gameID)[0];
        Sprite sprite = Resources.Load<Sprite>("paintings/" + content);
        drawing.Init(sprite);
        introImage.Init();
        introImage.AnimateOff();

        Invoke("Done", 5);
    }
    public void Done()
    {
        OnComplete();
        Events.SetReadyButton(OnReadyClicked);
    }
    void OnReadyClicked()
    {
        Events.OnGoto(true);
    }
    
}
