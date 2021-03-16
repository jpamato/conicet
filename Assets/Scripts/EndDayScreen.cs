using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDayScreen : ScreenMain
{
    public override void OnReady()
    {
        base.OnReady();
        Events.SetNextButton(false);
        Events.SetReadyButton(OnClicked);
    }
    void OnClicked()
    {
        Data.Instance.userData.BackToMainMenu(true);
    }
}
