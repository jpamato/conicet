using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Events
{
    public static System.Action<AudioClip, List<TimelineTextData>, System.Action> SetAudioPlayer = delegate { };
    public static System.Action AllDataLoaded = delegate { };
    public static System.Action StopAudioPlayer = delegate { };
    public static System.Action<int> OnNewKeyframeReached = delegate { };
    public static System.Action<bool> ShowHamburguer = delegate { };
    public static System.Action<bool> SetBackButton = delegate { };
    public static System.Action<bool> SetNextButton = delegate { };
    public static System.Action<string> Log = delegate { };
    public static System.Action<System.Action> SetReadyButton= delegate { };

    public static System.Action<bool> OnGoto = delegate { };
    public static System.Action<TextsData.Content, System.Action, CharactersManager.types> OnCharacterSay = delegate { };

    public static System.Action ResetApp = delegate { };
    public static System.Action<string, string, System.Action> PlaySoundTillReady = delegate { };
    public static System.Action<string, string, bool> PlaySound = delegate { };
    public static System.Action<string, float> ChangeVolume = delegate { };
    public static System.Action<Color> ChangeColor = delegate { };
}
   
