using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentScreen : MonoBehaviour
{
    [SerializeField] GameObject panel;

    void Awake()
    {
        Close();
    }
    public void Init()
    {
        panel.SetActive(true);
    }
    public void Close()
    {
        panel.SetActive(false);
    }
}
