using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapImages : MonoBehaviour
{
    [SerializeField] Sprite[] all;
    Image image;
    void Start()
    {
        image = GetComponent<Image>();
        image.sprite = all[Random.Range(0, all.Length)];
    }
}
