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
    public List<RimaPair> pairs;
    public List<DragueableItem> items;
    public SimpleFeedback simpleFeedback;
    bool gameReady;

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
        intro.SetActive(true);
        pairs.Clear();
        items.Clear();

        Utils.RemoveAllChildsIn(pairContainer);
        Utils.RemoveAllChildsIn(itemsContainer);

        base.OnReady();
        content = Data.Instance.gamesData.activeContent;

        bool lang = false;
        if (Data.Instance.lang == Data.langs.QOM) lang = true;
        TextsData.Content tipContent = Data.Instance.textsData.GetContent("tip_unir", lang);

        Events.OnCharacterSay(tipContent, OnTipDone);

        UpdateLoop();
    }
    private void UpdateLoop()
    {
        if (!gameReady)
        {
            int id = 0;
            int corrects = 0;
            int done = 0;
            foreach (RimaPair rimaPair in pairs)
            {
                if (rimaPair.dragueableItemDestination.state == DragueableItemDestination.states.DONE)
                {
                    if (rimaPair.id == rimaPair.dragueableItemDestination.dragueableItemID)
                        corrects++;
                    done++;
                }
                id++;
            }
            if (done >= 2 && done >= pairs.Count)
            {
                print("RIMAS corrects: " + corrects + " done: " + done + " pairs.Count: " + pairs.Count);
                gameReady = true;
                if (corrects == done)
                    simpleFeedback.SetState(SimpleFeedback.states.OK, 1);
                else
                    simpleFeedback.SetState(SimpleFeedback.states.WRONG, 1);
                StartCoroutine(CheckResults(corrects == done));
            }
        }
        Invoke("UpdateLoop", 0.5f);
    }
    IEnumerator CheckResults(bool isOk)
    {
        yield return new WaitForSeconds(1);
        if (isOk)
        {
            OnComplete();
        }
        else
        {
            foreach (DragueableItem di in items)
                di.Reset();
            gameReady = false;
        }
    }
    void OnTipDone()
    {
        introBar.AnimateOff(10);
        string storyID = Data.Instance.storiesData.activeContent.id;
        field.text = Data.Instance.textsData.GetContent("rima_" + storyID).text;
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
        intro.SetActive(false);
        int id = 0;
        foreach(string s in content.unir)
        {
            Sprite sprite = Data.Instance.assetsData.GetContent(s).sprite;
            print(s + " Sprite: " + sprite);
            if (id %2==0)
            {
                RimaPair rp = Instantiate(pair, pairContainer);                
                rp.transform.localScale = Vector2.one;
                rp.transform.localPosition = new Vector2(0, 300 * id);
                rp.Init(id, sprite);
                pairs.Add(rp);
            }
            else
            {
                DragueableItem item = Instantiate(dragueableItem, itemsContainer);
                item.transform.localScale = Vector2.one;
                item.transform.localPosition = new Vector2(0, 300*(id-1));
                item.Init(id-1, sprite);
                items.Add(item);
            }
            id++;
        }
        foreach(DragueableItem i in items)
            foreach (RimaPair rp in pairs)
                i.SetDestiny(rp.dragueableItemDestination);
           
    }
    void OnReadyClicked()
    {
        Events.OnGoto(true);
    }
}
