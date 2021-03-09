using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleButton : MonoBehaviour
{
    public int id;
    public Image image;
    public Text field;
    public System.Action OnClicked;

    public void Init(int id, Sprite sprite, string text, System.Action<SimpleButton> OnClicked)
    {
        this.id = id;
        if (sprite != null)  image.sprite = sprite; else  image.enabled = false;
        if (text != "")   field.text = text;   else text = "";
        if(OnClicked != null)
            GetComponent<Button>().onClick.AddListener(() => OnClicked(this));
    }
}
