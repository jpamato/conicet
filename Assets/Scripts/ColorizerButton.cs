using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorizerButton : MonoBehaviour
{
    public Color color;
    public Image image;

    public void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => OnClicked());
        color = image.color;    
    }
    void OnClicked()
    {
        Events.ChangeColor(color);
    }
}
