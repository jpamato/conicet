﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Labyrinth : ScreenMain
{
    int offset = 0;//100;
    public List<string> content;
    int id = 0;
    public Image image;
    LabyrinthSlot[] slots;
    public Color slotInactiveColor;
    public Color slotActiveColor;
    public Color slotWallColor;
    public List<LabyrinthSlot> slotsSelected;
    public states state;
    public GameObject asset_character;
    public GameObject asset_end_ok;
    public GameObject asset_end_bad;
    int errors;

    public Image[] images;

    public enum states
    {
        WAITING,
        PLAYING,
        DONE
    }

    public override void OnEnable()
    {
        base.OnEnable();
        slots = GetComponentsInChildren<LabyrinthSlot>();

        int[] arr1 = new int[] {
        1,1,1,1,1,1,1,
        1,2,0,0,1,0,3,
        1,0,1,1,1,0,1,
        1,0,1,0,0,0,1,
        1,0,1,0,1,0,1,
        1,0,0,0,1,0,4,
        1,1,1,1,1,1,1
        };

        int[] arr2 = new int[] {
        1,1,1,1,1,1,1,
        1,0,0,0,1,0,4,
        1,0,1,0,1,0,1,
        1,0,1,0,0,0,1,
        1,0,1,1,1,0,1,
        1,0,0,2,1,0,3,
        1,1,1,1,1,1,1
        };

        int[] arr3 = new int[] {
        1,1,1,1,1,1,1,
        1,1,1,1,1,0,4,
        1,1,1,1,1,0,1,
        1,0,0,2,0,0,1,
        1,0,1,1,1,1,1,
        1,0,0,0,0,0,3,
        1,1,1,1,1,1,1
        };

        int[] arr4 = new int[] {
        1,1,1,1,1,1,1,
        1,0,0,0,0,0,3,
        1,0,1,1,1,1,1,
        1,0,1,1,1,1,1,
        1,0,1,1,1,1,1,
        1,2,0,0,0,0,4,
        1,1,1,1,1,1,1
        };

        int[] arr5 = new int[] { //Este los 0 parecen una esvastica re turbio jajajajaj
        1,1,1,1,1,1,1,
        1,2,0,1,1,0,3,
        1,1,0,1,1,0,1,
        1,1,0,0,0,0,1,
        1,1,1,0,1,1,1,
        1,1,1,0,0,0,4,
        1,1,1,1,1,1,1
        };

        int[] arr6 = new int[] { 
        1,1,1,1,1,1,1,
        1,0,0,0,1,1,1,
        1,0,1,0,1,1,1,
        3,0,1,2,1,0,4,
        1,1,1,0,1,0,1,
        1,1,1,0,0,0,1,
        1,1,1,1,1,1,1
        };

        int[] arr7 = new int[] { 
        1,1,1,1,1,1,1,
        1,0,0,2,1,0,3,
        1,0,1,0,1,0,1,
        1,0,1,0,0,0,1,
        1,0,1,1,1,1,1,
        4,0,1,1,1,1,1,
        1,1,1,1,1,1,1
        };

        int[] arr;

        int rand = Random.Range(1, 7+1);
        switch(rand)
        {
            case 1:  arr = arr1; break;
            case 2: arr = arr2; break;
            case 3: arr = arr3; break;
            case 4: arr = arr4; break;
            case 5: arr = arr5; break;
            case 6: arr = arr6; break;
            default: arr = arr7; break;
        }

        int id = 0;
        foreach (LabyrinthSlot slot in slots)
        {
            if(id<arr.Length)
            slot.Init(this, arr[id]);
            id++;
        }
        SetInit();
    }
    public override void OnReady()
    {
        isReady = false;
        errors = 0;
        Reset();
        base.OnReady();
        string story_id = Data.Instance.storiesData.activeContent.id;
        GamesData.Content c = Data.Instance.gamesData.GetContent(story_id);
        content = c.GetContentFor(type, gameID);
        int num = 0;
        foreach (string imageName in content)
        {
            //images[num].sprite = Resources.Load<Sprite>("assets/" + imageName) as Sprite;

            images[num].sprite = Data.Instance.GetSprite("assets/images/" + imageName);
            num++;
        }
        if (content == null) return;
        TextsData.Content tipContent = Data.Instance.daysData.GetTip("tip_labyrinth");
        Events.OnCharacterSay(tipContent, OnTipDone, tipContent.character_type);

        asset_character.SetActive(true);
    }
    void SetInit()
    {
        asset_character.transform.SetParent( GetSlotByType(LabyrinthSlot.types.INIT).transform);
        asset_end_ok.transform.SetParent(GetSlotByType(LabyrinthSlot.types.END_OK).transform);
        asset_end_bad.transform.SetParent(GetSlotByType(LabyrinthSlot.types.END_BAD).transform);
        asset_character.transform.localPosition = Vector3.zero;
        asset_end_ok.transform.localPosition = new Vector3(offset, 0, 0);
        asset_end_bad.transform.localPosition = new Vector3(offset, 0, 0);
        asset_character.SetActive(false);
        asset_end_ok.SetActive(false);
        asset_end_bad.SetActive(false);
    }
    void SetSlotToCharacter()
    {
        asset_character.transform.SetParent(slotsSelected[id].transform);
        asset_character.transform.localPosition = Vector3.zero;
    }
    LabyrinthSlot GetSlotByType(LabyrinthSlot.types type)
    {
        foreach (LabyrinthSlot slot in slots)
            if (slot.type == type)
                return slot;
        return null;
    }
    void OnTipDone()
    {
        state = states.PLAYING;
        asset_character.SetActive(true);
        asset_end_ok.SetActive(true);
        asset_end_bad.SetActive(true);
    }
    public override void Hide(bool toLeft)
    {
        StopAllCoroutines();
        base.Hide(toLeft);
    }
    private void Reset()
    {
        state = states.WAITING;
        id = 0;
        CancelInvoke();
        foreach (LabyrinthSlot slot in slots)
        {
            if(slot.type != LabyrinthSlot.types.WALL)
                slot.SetColor(slotInactiveColor);
        }
        slotsSelected.Clear();
    }
    public void OnOver(LabyrinthSlot slot)
    {
        if (state == states.DONE)
            return;
        if (slot.type == LabyrinthSlot.types.WALL)
            OnWallOver();
        else if (slot.type == LabyrinthSlot.types.INIT && slotsSelected.Count == 0)
        {
            slot.SetColor(slotActiveColor);
            slotsSelected.Add(slot);
        }
        else if (slotsSelected.Count > 0)
        {
            slot.SetColor(slotActiveColor);
            slotsSelected.Add(slot);
        }

        if (slotsSelected.Count > 0 && slot.type == LabyrinthSlot.types.END_OK)
            OnSetResult(true);
        else if (slotsSelected.Count > 0 && slot.type == LabyrinthSlot.types.END_BAD)
            OnSetResult(false);
    }
    private void Loop()
    {
        if(id>=slotsSelected.Count)
        {
            asset_character.SetActive(false);
            if (isOk)
            {
                slotsSelected[slotsSelected.Count - 1].GetComponentInChildren<SimpleFeedback>().SetState(SimpleFeedback.states.OK, 2);
                Invoke("Win", 1);
            } else
            {
                slotsSelected[slotsSelected.Count - 1].GetComponentInChildren<SimpleFeedback>().SetState(SimpleFeedback.states.WRONG, 2);
                Invoke("Retry", 2);
            } 
            return;
        }
        SetSlotToCharacter();
        if (id > 0)
            Events.PlaySound("ui", "ui/step", false);
        asset_character.GetComponent<Animation>().Play();
        Invoke("Loop", 0.5f);
        id++;
    }
    bool isReady;
    void Win()
    {
        if (isReady) return;
        isReady = true;
        OnComplete();
        Events.SetReadyButton(OnReadyClicked);
    }
    void Retry()
    {
        SetInit();
        Reset();
        OnTipDone();
    }
    bool isOk;
    public void OnSetResult(bool isOk)
    {
        this.isOk = isOk;
        state = states.DONE;
        Loop();
    }
    public void OnWallOver()
    {
        errors++;
        if (slotsSelected.Count>0)
            Events.PlaySound("ui", "ui/feedback_bad", false);
        Reset();
        if (errors >2)
            Win();
    }
    void OnReadyClicked()
    {
        Events.OnGoto(true);
    }
}
