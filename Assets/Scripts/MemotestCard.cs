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
    System.Action<MemotestCard> SetSelected;

    public void Init(System.Action<MemotestCard> SetSelected, AssetsData.Content content)
    {
        this.SetSelected = SetSelected;
        this.content = content;
        anim = GetComponent<Animation>();
        if (content.sprite == null)
        {
            Events.Log("Falta asset: " + content.name);
        }
        else
        {
            image.sprite = content.sprite;
        }
        SetOn();
        //field.text = Utils.ParseText( content.name );

        field.text = Data.Instance.assetsData.GetRealText(content.name);
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

        SetSelected(this);
    }
}
