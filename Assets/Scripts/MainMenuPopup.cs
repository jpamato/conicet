using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPopup : MonoBehaviour
{
    public GameObject panel;
    [SerializeField] ContentScreen instrucciones;
    [SerializeField] ContentScreen creditos;

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
    public void Credits()
    {
        creditos.Init();
        Close();
    }
    public void Instrucciones()
    {
        instrucciones.Init();
        Close();
    }
}

