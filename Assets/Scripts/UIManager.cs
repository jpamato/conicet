using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button back;
    public Button next;
    public GameObject hamburguer;

    private void Start()
    {
        back.gameObject.SetActive(false);
        next.gameObject.SetActive(false);
        hamburguer.SetActive(false);

        Events.SetBackButton += SetBackButton;
        Events.SetNextButton += SetNextButton;
        Events.ShowHamburguer += ShowHamburguer;
    }
    private void OnDestroy()
    {
        Events.SetBackButton -= SetBackButton;
        Events.SetNextButton -= SetNextButton;
        Events.ShowHamburguer -= ShowHamburguer;
    }
    void ShowHamburguer(bool isOn)
    {
        hamburguer.SetActive(isOn);
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
        Events.PlaySound("ui", "ui/click", false);
        Events.OnGoto(false);
    }
    public void Next()
    {
        Events.PlaySound("ui", "ui/click", false);
        Events.OnGoto(true);
    }
    public void ChengeLang()
    {
        Events.ResetApp();
        Data.Instance.LoadScene("Splash");
    }
}
