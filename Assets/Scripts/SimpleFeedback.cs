using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFeedback : MonoBehaviour
{
    public GameObject ok;
    public GameObject wrong;
    public enum states
    {
        OK, 
        WRONG
    }
    private void Start()
    {
        SetOff();
    }
    public void SetState(states STATE, int duration)
    {
        SetOff();
        switch (STATE)
        {
            case states.OK:
                ok.gameObject.SetActive(true);
                break;
            case states.WRONG:
                wrong.gameObject.SetActive(true);
                break;
        }
        Invoke("SetOff", duration);
    }
    public void SetOff()
    {
        ok.gameObject.SetActive(false);
        wrong.gameObject.SetActive(false);
    }

}
