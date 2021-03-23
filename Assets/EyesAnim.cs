using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyesAnim : MonoBehaviour
{
    public GameObject eyesOpen;
    public GameObject eyesClosed;

    void OnEnable()
    {
        Loop();
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
    void Loop()
    {
        eyesOpen.SetActive(true);
        eyesClosed.SetActive(false);
        CancelInvoke();
        Invoke("Close", Random.Range(2, 6));
    }
    void Close()
    {
        eyesOpen.SetActive(false);
        eyesClosed.SetActive(true);
        Invoke("Loop", (float)Random.Range(2, 4)/15f);
    }
}
