using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ordenar : ScreenMain
{
    string audioName;
    public float separation;
    GamesData.Content content;
    public GameObject intro;
    public FillAmountAnim introBar;
    public DragueableItemDestination slot;
    public DragueableItem dragueableItem;
    public Transform pairContainer;
    public Transform itemsContainer;
    public Transform allContainers;
    public List<DragueableItemDestination> slots;
    public List<DragueableItem> items;
    public SimpleFeedback simpleFeedback;
    bool gameReady;
    bool startInOrder = false; // Empezar ordenado automaticamente en la actividad de tip conta cuento.
    public GameObject repeatButton;
    TextsData.Content tipContent;

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

        Utils.RemoveAllChildsIn(pairContainer);
        Utils.RemoveAllChildsIn(itemsContainer);
    }
    public override void OnReady()
    {

        base.OnReady();
        StopAllCoroutines();
        intro.SetActive(true);

        content = Data.Instance.gamesData.activeContent;

        tipContent = Data.Instance.daysData.GetTip("tip_ordenar");
        Events.OnCharacterSay(tipContent, OnTipDone, tipContent.character_type);

        // checkear si tiene que empezar ordenado
        if ((tipContent.id == "tip_conta_cuento") || (tipContent.id == "tip_conta_cuentoL2"))
        {
            Debug.Log("Tip: " + tipContent.id);
            startInOrder = true;
            repeatButton.SetActive(false);
        }
        else
        {
            startInOrder = false;
            repeatButton.SetActive(true);
        }
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
    List<string> arr;
    void OnTextDone()
    {
        // intro.SetActive(false);
        int id = 0;
        GamesData.Content c = Data.Instance.gamesData.GetContent(storyID);
        arr = c.GetContentFor(type, gameID);

        foreach (string s in arr)
        {

            if (id == 0)
            {
                audioName = s;
                Say();
            }
            else
            {
                string[] arr = s.Split(","[0]);
                int id2 = 0;
                foreach (string itemName in arr)
                {

                    Sprite sprite = Data.Instance.assetsData.GetContent(itemName).sprite;

                    DragueableItem item = Instantiate(dragueableItem, itemsContainer);
                    item.transform.localScale = Vector2.one;
                    item.transform.localPosition = new Vector2(separation * (id2), 0);
                    item.Init(id2, sprite, OnReleaseAdd1);
                    items.Add(item);
                    item.image.transform.localScale = Vector2.one;
                    id2++;
                }
            }
            id++;
        }

        //shuffle
        if (startInOrder == false) { 
        for (int a = 0; a < 10; a++)
        {
            int rand = Random.Range(1, items.Count);
            Vector3 pos1 = items[0].transform.localPosition;
            Vector3 pos2 = items[rand].transform.localPosition;
            items[0].transform.localPosition = pos2;
            items[rand].transform.localPosition = pos1;

        }
        }
        float _x = -separation/2 + ((separation * items.Count) / 2);
        allContainers.transform.localPosition = new Vector3(-_x, allContainers.transform.localPosition.y, 0);


        id = 0;
        foreach (DragueableItem i in items)
        {
            AddSlot(id);
            i.ResetOriginalPosition();
            id++;
        }

        foreach (DragueableItem i in items)
        { 
            foreach (DragueableItemDestination rp in slots)
                i.SetDestiny(rp);
        }

        // Settear cada item en su slot correspondiente automaticamente 
        if (startInOrder)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].transform.position = slots[i].transform.position;
                items[i].SetInactive();

                StartCoroutine(EsperarContarCuento());
            }
        }
    }
    void AddSlot(int id)
    {
        DragueableItemDestination rp = Instantiate(slot, pairContainer);
        rp.transform.localScale = Vector2.one;
        rp.transform.localPosition = new Vector2(separation * id, 0);
        // rp.Init(id, null, OnRelease);
        slots.Add(rp);
        rp.GetComponentInChildren<Text>().text = (id + 1).ToString();
    }

    public List<int> allSlotsIDS;

    private void OnDragDone()
    {
       
        if (!gameReady)
        {
            int id = 0;
            bool mistake = false;
            if (slots.Count > 0)
            {
                allSlotsIDS.Clear();
                foreach (DragueableItemDestination di in slots)
                {
                    if(di.dragueableItemID != -1)
                        allSlotsIDS.Add(di.dragueableItemID);
                    if (di.dragueableItemID != id)
                    {
                        mistake = true;
                    }
                    id++;
                }

                if (!mistake)
                {
                    gameReady = true;
                    OnComplete();
                    simpleFeedback.SetState(SimpleFeedback.states.OK, 5);
                    id = 0;
                    slots.Clear();
                    foreach (DragueableItem di in items)
                        di.SetInactive();
                    return;
                } else if(allSlotsIDS.Count == slots.Count)
                {
                    simpleFeedback.SetState(SimpleFeedback.states.WRONG, 2);
                }
            }
        }
    }
   
    void OnRelease(int id)
    {
    }
    void OnReleaseAdd1(int id)
    {
    }
    // Gets called when the activity first starts. Desambiguated from repeat to allow activity to start with silence but allow repeat button to always play a sound event
    public void Say()
    {
        Events.PlaySound("voices", "ordenar"  + Utils.GetLangFolder() + "/" + audioName, false);
    }
    // Repeat Button
    public void Repeat()
    {
        // En caso de que sea silencio, reproducir el audio del tip
        if (audioName == "ordenarMensaje")
            Events.PlaySound("voices", "genericTexts" + Utils.GetLangFolder() + "/" + tipContent.id, false);
        // En caso contrario, reproducir el audio especificado en la database
        else
            Events.PlaySound("voices", "ordenar" + Utils.GetLangFolder() + "/" + audioName, false);
    }
    void OnReadyClicked()
    {
        Events.OnGoto(true);
    }

    // Terminar ejercicio despues de determinado tiempo, ya que se resuelve sola la actividad.
    private IEnumerator EsperarContarCuento()
    {
        yield return new WaitForSeconds(20);
        OnComplete();
    }
}
