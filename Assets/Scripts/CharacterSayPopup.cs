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
    public GameObject goBtn;
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
        if(isOn)
            StartCoroutine(AnimDone());
        goBtn.SetActive(false);
    }
    void SetOff()
    {
        panel.SetActive(false);
        isOn = false;
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
            goBtn.SetActive(false);
            panel.SetActive(true);
            this.OnDone = OnDone;
            field.text = content.text;
            Events.PlaySoundTillReady("voices", "genericTexts/" + content.id, OnReady);
        }
    }
    public void OnReady()
    {
        goBtn.SetActive(true);
    }
    public void OnClicked()
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
