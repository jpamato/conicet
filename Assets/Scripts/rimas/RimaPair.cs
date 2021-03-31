using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RimaPair : MonoBehaviour
{
    public Image image;
    public Image image2;
    public GameObject button2;
    public int id;
    public DragueableItemDestination dragueableItemDestination;
    System.Action<int> OnRelease;

    public void Init(int id, Sprite sprite, System.Action<int> OnRelease)
    {
        this.OnRelease = OnRelease;
        this.id= id;
        image.sprite = sprite;
        button2.SetActive(false);
        image2.enabled = false;
        dragueableItemDestination = GetComponent<DragueableItemDestination>();
    }
    public void SetDone(Sprite sprite)
    {
        button2.SetActive(true);
        image2.enabled = true;
        image2.sprite = sprite;
    }
    void OnDone()
    { 
    }
    public void OnReleaseDone()
    {
        OnRelease(id);
    }
}
