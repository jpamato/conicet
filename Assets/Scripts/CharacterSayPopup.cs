using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSayPopup : MonoBehaviour
{
    System.Action OnDone;
    public Text field;
    public GameObject panel;
    public Animation anim;

    bool isOn;
    void Start()
    {
        SetOff();
        Events.OnCharacterSay += OnCharacterSay;
        Events.OnGoto += OnGoto;
    }
    void OnDestroy()
    {
        Events.OnCharacterSay -= OnCharacterSay;
        Events.OnGoto -= OnGoto;
    }
    void OnGoto(bool next)
    {
        OnDone = null;
        if (isOn)
            StartCoroutine(AnimDone());
    }
    void SetOff()
    {
        panel.SetActive(false);
        isOn = false;
        OnDone = null;
    }
    void OnCharacterSay(TextsData.Content content, System.Action OnDone)
    {
        isOn = true;
        if (content == null)
        {
            OnReady();
            this.OnDone = null;
            Events.PlaySound("voices", "", false);
        }
        else
        {
            panel.SetActive(true);
            this.OnDone = OnDone;
            field.text = content.text;
            Events.PlaySoundTillReady("voices", "genericTexts/" + content.id, OnReady);
        }
    }
    public void OnReady()
    {
        Events.SetReadyButton(ReadyButtonClicked);
    }
    public void ReadyButtonClicked()
    {       
        StartCoroutine(AnimDone());
    }
    IEnumerator AnimDone()
    {
        yield return new WaitForSeconds(0.2f);
        anim.Play("off");
        yield return new WaitForSeconds(1);
        if (OnDone != null)
            OnDone();
       
        SetOff();
    }
}
