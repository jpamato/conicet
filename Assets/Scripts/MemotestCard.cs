using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemotestCard : MonoBehaviour
{
    public states state;
    public enum states
    {
        ON,
        OFF,
        DONE
    }
    [SerializeField] Image image;
    Animation anim;
    Memotest memotest;
    [HideInInspector] public AssetsData.Content content;
    [SerializeField] Text field;

    public void Init(Memotest memotest, AssetsData.Content content)
    {
        this.memotest = memotest;
        this.content = content;
        anim = GetComponent<Animation>();
        image.sprite = content.sprite;
        SetOn();
        field.text = content.name;
    }
    public void SetDone()
    {
        state = states.DONE;
        anim.Play("card_win");  
    }
    public void SetWrong()
    {
        anim.Play("card_wrong");
    }
    public void SetOn()
    {
        state = states.ON;
        anim.Play("card_on");
    }
    public void SetOff()
    {
        state = states.OFF;
        anim.Play("card_off");
    }
    public void Clicked()
    {
        if (state != states.OFF) return;

        memotest.SetSelected(this);
    }
}
