using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rimas : ScreenMain
{
    public Text field;
    GamesData.Content content;
    public GameObject intro;
    public FillAmountAnim introBar;
    public RimaPair pair;
    public DragueableItem dragueableItem;
    public Transform pairContainer;
    public Transform itemsContainer;


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
        Events.OnCharacterSay(null, null);
    }
    public override void OnReady()
    {
        Utils.RemoveAllChildsIn(pairContainer);
        Utils.RemoveAllChildsIn(itemsContainer);

        base.OnReady();
        content = Data.Instance.gamesData.activeContent;
        TextsData.Content tipContent = Data.Instance.textsData.GetContent("escucha_maestra");
        Events.OnCharacterSay(tipContent, OnTipDone);
    }
    void OnTipDone()
    {
        introBar.AnimateOff(10);
        string storyID = Data.Instance.storiesData.activeContent.id;
        content = Data.Instance.gamesData.GetContent(storyID);
        Events.PlaySoundTillReady("voices", "genericTexts/rima_" + storyID, OnTextDone);
    }
    public override void OnComplete()
    {
        base.OnComplete();
        Events.SetReadyButton(OnReadyClicked);
    }
    void OnTextDone()
    {
        int id = 0;
        foreach(string s in content.rimas)
        {
            Sprite sprite = Data.Instance.assetsData.GetContent(s).sprite;
            print(s + " Sprite: " + sprite);
            if (id %2==0)
            {
                RimaPair rp = Instantiate(pair, pairContainer);                
                rp.transform.localScale = Vector2.one;
                rp.Init(id, sprite) ;              
            }
            else
            {
                DragueableItem item = Instantiate(dragueableItem, itemsContainer);
                item.transform.localScale = Vector2.one;
                item.Init(id, sprite);
            }
            id++;
        }
    }
    void OnReadyClicked()
    {
        Events.OnGoto(true);
    }
}
