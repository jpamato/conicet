using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDayScreen : ScreenMain
{
    public override void Show(bool fromRight)
    {
        base.Show(fromRight);
    }
    public override void OnReady()
    {
        base.OnReady();
        Events.SetNextButton(false);
        Events.SetReadyButton(OnClicked);

        Events.PlaySound("voices", "ui/win", false);
    }
    void OnClicked()
    {
        Data.Instance.userData.BackToMainMenu(true);
    }
}
