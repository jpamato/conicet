using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragueableItemDestination : MonoBehaviour
{
    public states state;
    public enum states
    {
        IDLE,
        DONE
    }

    public void Reset()
    {
        state = states.IDLE;
    }
    public void SetDone()
    {
        state = states.DONE;
    }
}
