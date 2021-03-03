using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button back;
    public Button next;

    private void Start()
    {
        back.gameObject.SetActive(false);
        next.gameObject.SetActive(false);

        Events.SetBackButton += SetBackButton;
        Events.SetNextButton += SetNextButton;
    }
    private void OnDestroy()
    {
        Events.SetBackButton -= SetBackButton;
        Events.SetNextButton -= SetNextButton;
    }
    void SetBackButton(bool isOn)
    {
        back.gameObject.SetActive(isOn);
    }
    void SetNextButton(bool isOn)
    {
        next.gameObject.SetActive(isOn);
    }
    public void Back()
    {
        print("back");
        Data.Instance.userData.PrevActivity();
    }
    public void Next()
    {
        Data.Instance.userData.NextActivity();
    }
}
