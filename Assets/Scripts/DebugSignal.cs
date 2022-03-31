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
#if UNITY_EDITOR
        Events.Log += Log;
#endif
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
        Debug.Log(text);
    }
    public void Close()
    {
        panel.SetActive(false);
    }
}
