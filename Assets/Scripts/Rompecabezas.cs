using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rompecabezas : ScreenMain
{
    GamesData.Content content;
    public GameObject intro;
    public FillAmountAnim introBar;
    public DragueableItemDestination[] slots;
    public DragueableItem[] items;
    public SimpleFeedback simpleFeedback;
    bool gameReady;
    public Animation anim;
    public List<Vector2> itemsPositions;

    private void Start()
    {
        foreach (DragueableItem di in GetComponentsInChildren<DragueableItem>())
            itemsPositions.Add(di.transform.localPosition);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        intro.SetActive(true);
        introBar.Init();
    }
    public override void OnDisable()
    {
        base.OnDisable();
    }
    public override void OnOff()
    {
        Events.StopAudioPlayer();
        Events.OnCharacterSay(null, null, CharactersManager.types.Brisa);
    }
    public override void OnReady()
    {
        slots = GetComponentsInChildren<DragueableItemDestination>();
        items = GetComponentsInChildren<DragueableItem>();

        foreach (DragueableItemDestination di in slots)
            di.Reset();

        int id = 0;
        foreach (DragueableItem di in items)
        {
            di.transform.localPosition = itemsPositions[id];
            id++;
        }

        intro.SetActive(true);
        gameReady = false;
        anim.Play("idle");

        base.OnReady();
        content = Data.Instance.gamesData.activeContent;

        TextsData.Content tipContent = Data.Instance.daysData.GetTip("tip_unir");
        Events.OnCharacterSay(tipContent, OnTipDone, tipContent.character_type);

        UpdateLoop();
    }
    public override void Hide(bool toLeft)
    {
        print("Hide");
        base.Hide(toLeft);
        introBar.Init();
    }
    private void UpdateLoop()
    {
        int id = 0;
        bool mistake = false;
        foreach (DragueableItemDestination di in slots)
        {
            if(di.dragueableItemID != id)
            {
                mistake = true;
                break;
            }
            id++;
        }
        if (!mistake)
        {
            gameReady = true;
            OnComplete();
            anim.Play("ready");
            simpleFeedback.SetState(SimpleFeedback.states.OK, 5);
            id = 0;
            foreach (DragueableItem di in items)
            {
                di.SetInactive();
            }
        }
        else
        {
            Invoke("UpdateLoop", 0.5f);
        }
    }
    string storyID;
    void OnTipDone()
    {
        storyID = Data.Instance.storiesData.activeContent.id;
        //    field.text = Data.Instance.textsData.GetContent("rima_" + storyID).text;
        content = Data.Instance.gamesData.GetContent(storyID);

        OnTextDone();
        introBar.AnimateOff(10);

        //Events.PlaySoundTillReady("voices", "genericTexts/rima_" + storyID, OnTextDone);
    }
    public override void OnComplete()
    {
        base.OnComplete();
        Events.SetReadyButton(OnReadyClicked);
    }
    List<string> arr;
    void OnTextDone()
    {
        // intro.SetActive(false);
        int id = 0;
        GamesData.Content c = Data.Instance.gamesData.GetContent(storyID);
        arr = c.GetContentFor(type, gameID);
        string spriteName = arr[0];
        Sprite sprite = Data.Instance.assetsData.GetContent(spriteName).sprite;
       
        id = 0;
        int row = 1;
        int col = 1;
        int _w = 150;
       
        foreach (DragueableItem di in items)
        {
            di.Init(id, sprite, OnRelease);
            foreach (DragueableItemDestination dd in slots)
            {
                di.SetDestiny(dd);
            }
            id++;
            int _x = 0;
            int _y = 0;
            if (col == 1)
                _x = _w;
            else if (col ==3)
                _x = -_w;
            if (row == 1)
                _y = -_w;
            else if (row == 3)
                _y = _w;
            di.image.transform.localPosition = new Vector2(_x, _y);
            col++;
            if(col>3)
            {
                col = 1;
                row++;
            }
        }
    }
    void OnRelease(int id)
    {
    }
    void OnReleaseAdd1(int id)
    {
    }
    void Say(string word)
    {
        print(word);
        Events.PlaySound("voices", "assets/" + word, false);
    }
    void OnReadyClicked()
    {
        Events.OnGoto(true);
    }
}
