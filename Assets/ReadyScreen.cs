using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyScreen : MonoBehaviour
{
    public GameObject panel;
    System.Action OnDone;
    bool clicked;

    void Start()
    {
        SetOff();
        Events.SetReadyButton += SetReadyButton;
        Events.OnGoto += Goto;
    }
    void OnDestroy()
    {
        Events.SetReadyButton -= SetReadyButton;
        Events.OnGoto -= Goto;
    }
    void SetReadyButton(System.Action OnDone)
    {
        clicked = false;
        this.OnDone = OnDone;
        panel.SetActive(true);
    }
    void Goto(bool n)
    {
        SetOff();
    }
    void SetOff()
    {
        panel.SetActive(false);
    }
    public void OnClicked()
    {
        if (clicked) return;
        clicked = true;
        OnDone();
        Invoke("SetOff", 0.2f);
    }
}
