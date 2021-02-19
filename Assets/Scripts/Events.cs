using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Events
{
    public static System.Action<AudioClip, List<TimelineTextData>, System.Action> SetAudioPlayer = delegate { };
    public static System.Action StopAudioPlayer = delegate { };
    public static System.Action<int> OnNewKeyframeReached = delegate { };
    public static System.Action<bool> SetBackButton = delegate { };
    public static System.Action OnBack = delegate { };

}
   
