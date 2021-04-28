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
    public Character character;

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
        StopAllCoroutines();
        if (isOn)
        {
            StartCoroutine(AnimDone());
        }
    }
    void SetOff()
    {
        panel.SetActive(false);
        isOn = false;
        OnDone = null;
    }
    void OnCharacterSay(TextsData.Content content, System.Action OnDone, CharactersManager.types type)
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
            StopAllCoroutines();
            character.Init(type);
            AudioSource audioSource = Data.Instance.GetComponent<AudioManager>().GetAudioSource("voices");
            Data.Instance.audioSpectrum.SetAudioSource(audioSource);
            panel.SetActive(true);
            this.OnDone = OnDone;
            field.text = content.text;
            Events.PlaySoundTillReady("voices", "genericTexts/" + content.id, ReadyButtonClicked);
            anim.Play("on");
        }
    }
    public void OnReady()
    {
        Events.SetReadyButton(ReadyButtonClicked);
    }
    public void ReadyButtonClicked()
    {
        StopAllCoroutines();
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
