using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragueableItem : MonoBehaviour
{
    public int id;
    public Image image;
    bool dragging;

    public void Init(int id, Sprite sprite)
    {
        this.id = id;
        this.image.sprite = sprite;
    }
    public void StartDrag()
    {
        dragging = true;
    }
    public void EndDrag()
    {
        dragging = false;
    }
    void Update()
    {
        if (!dragging) return;
        transform.localPosition = Input.mousePosition;
    }
}
