using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPopup : MonoBehaviour
{
    public GameObject panel;

    void Start()
    {
        Close();
    }
    public void Open()
    {
        panel.SetActive(true);
    }
    public void Close()
    {
        panel.SetActive(false);
    }
    public void ExitApp()
    {
        Application.Quit();
    }
}

