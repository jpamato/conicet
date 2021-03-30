using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayButtonSlot : MonoBehaviour
{
    public GameObject done;

    public void Init( bool played)
    {
        if (played)
            done.SetActive(true);
        else
            done.SetActive(false);
    }
}
