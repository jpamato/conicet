using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rimas : ScreenMain
{
    public float separation;
    public Text field;
    GamesData.Content content;
    public GameObject intro;
    public FillAmountAnim introBar;
    public RimaPair pair;
    public DragueableItem dragueableItem;
    public Transform pairContainer;
    public Transform itemsContainer;
    public Transform allContainers;
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
        Events.OnCharacterSay(null, null, CharactersManager.types.Nasheli);
    }
    public override void OnReady()
    {
        intro.SetActive(true);
        pairs.Clear();
        items.Clear();
        gameReady = false;

        Utils.RemoveAllChildsIn(pairContainer);
        Utils.RemoveAllChildsIn(itemsContainer);

        base.OnReady();
        content = Data.Instance.gamesData.activeContent;

        TextsData.Content tipContent = Data.Instance.daysData.GetTip("tip_unir");
        Events.OnCharacterSay(tipContent, OnTipDone, tipContent.character_type);

        UpdateLoop();
    }
    public override void Hide(bool toLeft)
    {
        base.Hide(toLeft);
        introBar.Init();
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
    string storyID;
    void OnTipDone()
    {
        storyID = Data.Instance.storiesData.activeContent.id;
    //    field.text = Data.Instance.textsData.GetContent("rima_" + storyID).text;
        content = Data.Instance.gamesData.GetContent(storyID);

        OnTextDone();
        introBar.AnimateOff(10);
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

        float _x = (((separation * arr.Count) / 2) * itemsContainer.transform.localScale.y) - (separation / 4);
        allContainers.transform.localPosition = new Vector3(-_x, allContainers.transform.localPosition.y, 0);

        foreach (string s in arr)
        {
            Sprite sprite = Data.Instance.assetsData.GetContent(s).sprite;
            
            if (id %2==0)
            {
                RimaPair rp = Instantiate(pair, pairContainer);                
                rp.transform.localScale = Vector2.one;
                rp.transform.localPosition = new Vector2(separation * id, 0);
                rp.Init(id, sprite, OnRelease);
                pairs.Add(rp);
            }
            else
            {
                DragueableItem item = Instantiate(dragueableItem, itemsContainer);
                item.transform.localScale = Vector2.one;
                item.transform.localPosition = new Vector2(separation * (id-1), 0);
                item.Init(id-1, sprite, OnReleaseAdd1);
                items.Add(item);
            }
            id++;
        }
        for (int a = 0; a < 5; a++)
        {
            int rand = Random.Range(1, items.Count);
            Vector3 pos1 = items[0].transform.localPosition;
            Vector3 pos2 = items[rand].transform.localPosition;
            items[0].transform.localPosition = pos2;
            items[rand].transform.localPosition = pos1;
        }




        //if (arr.Count < 5)
        //{
        //    _y = -65;
        //    allContainers.transform.localScale = new Vector2(1.3f, 1.3f);
        //}
        //else
        //    allContainers.transform.localScale = new Vector2(1f, 1f);



        foreach (DragueableItem i in items)
            foreach (RimaPair rp in pairs)
                i.SetDestiny(rp.dragueableItemDestination);



       

    }
    void OnRelease(int id)
    {
        Say(arr[id]);
    }
    void OnReleaseAdd1(int id)
    {
        Say(arr[id + 1]);
    }
    void Say(string word)
    {
        print(word);
        Events.PlaySound("voices", "assets/audio" + Utils.GetLangFolder() + "/" + word, false);
    }
    void OnReadyClicked()
    {
        Events.OnGoto(true);
    }
}
