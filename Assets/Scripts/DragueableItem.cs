﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragueableItem : MonoBehaviour
{
    public int id;
    public Image image;
    public states state;
    Vector3 originalPos;
    public List<DragueableItemDestination> destinations;
    Vector3  offset;
    public DragueableItemDestination itemDest;
    RandomRotation randomRotation;
    System.Action<int> OnRelease;
    bool isInactive;
    public enum states
    {
        IDLE,
        DRAGGING,
        NONE,
        DONE
    }
    // para el de los 2 grupos
    public bool isLeft;
    public bool isOk;
    public bool isDone;
    public string text = "";
    /// ///////////////////

    public void SetDestiny(DragueableItemDestination itemDest)
    {
        destinations.Add(itemDest);
    }
    public void Init(int id, Sprite sprite, System.Action<int> OnRelease)
    {
        itemDest = null;
        destinations.Clear();
        isInactive = false;
        this.OnRelease = OnRelease;
        //randomRotation = GetComponent<RandomRotation>();
        this.id = id;
        this.image.sprite = sprite;
        Invoke("ResetOriginalPosition", 0.5f); // Added delay before getting position since position on start was not correct.
    }

    public void ResetOriginalPosition()
    {
        originalPos = transform.position;
    }
    public void SetInactive()
    {
        isInactive = true;
    }
    public void StartDrag()
    {
        if (isInactive) return;
        if (itemDest != null)
        {
            itemDest.Reset();            
            itemDest = null;
        }
        if (text != "")
        {
            Say(text);
        }
        offset = Input.mousePosition - transform.position;
        state = states.DRAGGING;
     //   if(randomRotation)
       //     randomRotation.enabled = false;
        transform.localEulerAngles = Vector3.zero;
    }
    public void Say(string audioName)
    {
        Events.PlaySound("voices", "assets" + Utils.GetLangFolder() + "/audio/" + audioName, false);
    }
    public void EndDrag()
    {
        float distance = Mathf.Abs(Vector3.Distance(transform.position, originalPos));
        foreach (DragueableItemDestination idest in destinations)
        {
            float d = Mathf.Abs(Vector3.Distance(transform.position, idest.transform.position));
            if (d < distance && idest.state == DragueableItemDestination.states.IDLE)
            {   
                itemDest = idest;
                distance = d;
            }
        }
        if (itemDest != null)
        {
            itemDest.SetDone();
            itemDest.dragueableItemID = id;
            state = states.DONE;
        }
        else
        {
            state = states.NONE;
        }
    }
    void Update()
    {
        if (!isInactive) // dont allow dragging when inactive!
        {
            if (state == states.DRAGGING)
            {
                transform.position = Input.mousePosition - offset;
            }
            else if (state == states.DONE)
            {
                transform.position = Vector3.Lerp(transform.position, itemDest.transform.position, Time.deltaTime * 10);
                if (Vector3.Distance(transform.position, itemDest.transform.position) < 1)
                {
                    Events.OnDragDone();
                    state = states.IDLE;
                    transform.position = itemDest.transform.position;
                    transform.localEulerAngles = Vector3.zero;
                }
            }
            else if (state == states.NONE)
            {
                transform.position = Vector3.Lerp(transform.position, originalPos, Time.deltaTime * 10);
                if (Vector3.Distance(transform.position, originalPos) < 1)
                {
                    state = states.IDLE;
                    transform.position = originalPos;
                    if (randomRotation)
                        randomRotation.enabled = true;
                }
            }
        }
    }
    public void Reset()
    {
        state = states.NONE;

        foreach (DragueableItemDestination idest in destinations)
            idest.state = DragueableItemDestination.states.IDLE;

        itemDest = null;
    }
    public void OnReleaseDone()
    {
        OnRelease(id);
    }
}
