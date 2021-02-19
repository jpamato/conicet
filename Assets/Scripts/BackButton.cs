using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    public GameObject panel;

    void Start()
    {
        Events.SetBackButton += SetBackButton;
    }
    void OnDestroy()
    {
        Events.SetBackButton -= SetBackButton;
    }
    void SetBackButton(bool isOn)
    {
        panel.SetActive(isOn);
    }
    public void OnClicked()
    {
        Events.OnBack();
    }
}
