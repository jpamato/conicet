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

    void Start()
    {
        SetOff();
        Events.OnCharacterSay += OnCharacterSay;
    }
    void OnDestroy()
    {
        Events.OnCharacterSay -= OnCharacterSay;
    }
    void SetOff()
    {
        panel.SetActive(false);
    }
    void OnCharacterSay(TextsData.Content content, System.Action OnDone)
    {
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
        anim.Play("off");
        Invoke("AnimDone", 1);
    }
    void AnimDone()
    {
        if (OnDone != null)
            OnDone();
        SetOff();
    }
}
