using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrastrarGrupos : ScreenMain
{
    string audioName;
    public float separation;
    GamesData.Content content;
    public GameObject intro;
    public FillAmountAnim introBar;
    public DragueableItemDestination slot;
    public DragueableItem dragueableItem;
    public Transform mainContainer;
    public Transform leftContainer;
    public Transform rightContainer;
    public List<DragueableItemDestination> slots;
    public List<DragueableItem> items;
    public SimpleFeedback simpleFeedback;
    bool gameReady;
    string ok_left;
    string ok_right;
    List<string> leftWords;
    List<string> rightWords;
    [SerializeField] Text leftField;
    [SerializeField] Text rightField;

    public override void OnEnable()
    {
        Events.OnDragDone += OnDragDone;
        base.OnEnable();
        intro.SetActive(true);
        introBar.Init();
    }
    public override void OnDisable()
    {
        Events.OnDragDone -= OnDragDone;
        base.OnDisable();
    }
    public override void OnOff()
    {
        Events.StopAudioPlayer();
        Events.OnCharacterSay(null, null, CharactersManager.types.Nasheli);
    }
    public override void Show(bool fromRight)
    {
        base.Show(fromRight);

        slots.Clear();
        items.Clear();
        gameReady = false;

        Utils.RemoveAllChildsIn(mainContainer);
        Utils.RemoveAllChildsIn(leftContainer);
        Utils.RemoveAllChildsIn(rightContainer);
    }
    public override void OnReady()
    {

        base.OnReady();
        StopAllCoroutines();
        intro.SetActive(true);

        content = Data.Instance.gamesData.activeContent;

        TextsData.Content tipContent = Data.Instance.daysData.GetTip("tip_ordenar");
        Events.OnCharacterSay(tipContent, OnTipDone, tipContent.character_type);


    }
    public override void Hide(bool toLeft)
    {
        base.Hide(toLeft);
        introBar.Init();
    }

    string storyID;
    void OnTipDone()
    {
        storyID = Data.Instance.storiesData.activeContent.id;
        content = Data.Instance.gamesData.GetContent(storyID);

        OnTextDone();
        introBar.AnimateOff(10);
    }
    public override void OnComplete()
    {
        base.OnComplete();
        Events.SetReadyButton(OnReadyClicked);
    }
    void OnTextDone()
    {
        int __x = 240;
        mainContainer.transform.localPosition = new Vector2(0, mainContainer.transform.localPosition.y);
        leftContainer.transform.localPosition = new Vector2(__x, leftContainer.transform.localPosition.y);
        rightContainer.transform.localPosition = new Vector2(-__x, rightContainer.transform.localPosition.y);
        int id = 0;
        bool left= true;
        GamesData.Content c = Data.Instance.gamesData.GetContent(storyID);
        List<string> arr = c.GetContentFor(type, gameID);
        int idReal = 0;
        foreach (string s in arr)
        {
            if (id == 0)
            {
                if(left)
                {
                    ok_left = s;
                }
                else
                {
                    ok_right = s;
                }                
            }
            else if(s == "-")
            {
                id = -1;
                left = false;
            }
            else
            {
                Sprite sprite = Data.Instance.assetsData.GetContent(s).sprite;
                DragueableItem item = Instantiate(dragueableItem, mainContainer);
                item.transform.localScale = Vector2.one;
                item.transform.localPosition = new Vector2(separation * (idReal), 0);
                item.Init(idReal, sprite, OnReleaseAdd1);
                items.Add(item);
                item.image.transform.localScale = Vector2.one;
                idReal++;
                item.isLeft = left;
                item.text = s;
            }
            id++;
        }

        //shuffle
        for (int a = 0; a < 10; a++)
        {
            int rand = Random.Range(1, items.Count);
            Vector3 pos1 = items[0].transform.localPosition;
            Vector3 pos2 = items[rand].transform.localPosition;
            items[0].transform.localPosition = pos2;
            items[rand].transform.localPosition = pos1;
        }

        float _x = -separation / 2 + ((separation * items.Count) / 2);
        mainContainer.transform.localPosition += new Vector3(-_x, 0, 0);


        id = 0;
        left = true;
        foreach (DragueableItem i in items)
        {
            if (id >= items.Count / 2 && left)
            {
                left = false;
                id = 0;
            }
            AddSlot(id, left);
            i.ResetOriginalPosition();
            id++;
        }

        foreach (DragueableItem i in items)
        {
            foreach (DragueableItemDestination rp in slots)
                i.SetDestiny(rp);
        }

        _x = -separation / 2 + ((separation * items.Count/2) / 2);
        leftContainer.transform.localPosition += new Vector3(-_x, 0, 0);
        _x = -separation / 2 + ((separation * items.Count / 2) / 2);
        rightContainer.transform.localPosition += new Vector3(-_x, 0, 0);

        leftField.text = ok_left.ToUpper();
        rightField.text = ok_right.ToUpper();

    }
    void AddSlot(int id, bool isLeft)
    {
        Transform c = leftContainer;
        if (!isLeft)  c = rightContainer;
        DragueableItemDestination rp = Instantiate(slot, c);
        rp.transform.localScale = Vector2.one;
        rp.transform.localPosition = new Vector2(separation * id, 0);
        // rp.Init(id, null, OnRelease);
        slots.Add(rp);
        rp.GetComponentInChildren<Text>().text = "";
       // rp.GetComponentInChildren<Text>().text = (id + 1).ToString();
    }


    private void OnDragDone()
    {
        if (!gameReady)
        {
            int totalOK = 0;
            foreach (DragueableItem di in items)
            {
                if(di.transform.position.y<200)
                {
                    //ignore:
                    di.GetComponentInChildren<SimpleFeedback>().SetOff();
                    di.isOk = false;
                    di.isDone = false;
                }
                else if ((di.isLeft && di.transform.position.x < 500) || (!di.isLeft && di.transform.position.x >500))
                {
                    di.isOk = true;
                    if (!di.isDone)
                    {
                        di.GetComponentInChildren<SimpleFeedback>().SetState(SimpleFeedback.states.OK, 1000);
                        di.isDone = true;
                        audioName = di.text;
                        Say();
                    }
                    totalOK++;
                } else
                {
                    di.GetComponentInChildren<SimpleFeedback>().SetState(SimpleFeedback.states.WRONG, 2);
                    print(di.transform.position.x);
                    di.isDone = false;
                }
            }
            if(totalOK>=items.Count)
            {
                OnComplete();
            }
        }
    }

    void OnRelease(int id)
    {
    }
    void OnReleaseAdd1(int id)
    {
    }
    public void Say()
    {
        Events.PlaySound("voices", "assets" + Utils.GetLangFolder() + "/audio/" + audioName, false);
    }
    void OnReadyClicked()
    {
        Events.OnGoto(true);
    }
}
