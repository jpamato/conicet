using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugSignal : MonoBehaviour
{
    public GameObject panel;
    public Text field;

    void Start()
    {
        Events.Log += Log;
        Close();
    }
     void OnDestroy()
    {
        Events.Log -= Log;    
    }

    void Log(string text)
    {
        panel.SetActive(true);
        field.text = text;
    }
    public void Close()
    {
        panel.SetActive(false);
    }
}
