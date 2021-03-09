using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragueableItemDestination : MonoBehaviour
{
    public int dragueableItemID = -1;
    public states state;
    public enum states
    {
        IDLE,
        DONE
    }

    public void Reset()
    {
        dragueableItemID = -1;
        state = states.IDLE;
    }
    public void SetDone()
    {
        state = states.DONE;
    }
}
