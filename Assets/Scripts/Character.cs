using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public GameObject[] all;
    CharactersManager.types lastType;

    private void Reset()
    {
        foreach (GameObject go in all)
            go.SetActive(false);
    }
    public void Init(string name)
    {
        switch (name)
        {
            case "Nasheli": Init(CharactersManager.types.Nasheli); break;
            case "Oscarcito": Init(CharactersManager.types.Oscarcito); break;
            case "Mujer": Init(CharactersManager.types.Mujer); break;
            case "Hombre": Init(CharactersManager.types.Hombre); break;
            case "Dany": Init(CharactersManager.types.Dany); break;
            case "Uriel": Init(CharactersManager.types.Uriel); break;
            case "Loro": Init(CharactersManager.types.Loro); break;
            default: Init(CharactersManager.types.Nasheli); break;
        }
    }
    public void Init(CharactersManager.types type)
    {
        if (lastType == type)
            return;
        lastType = type;
        Reset();
        switch (type)
        {
            case CharactersManager.types.Nasheli:
                all[0].SetActive(true);
                break;
            case CharactersManager.types.Dany:
                all[1].SetActive(true);
                break;
            case CharactersManager.types.Oscarcito:
                all[2].SetActive(true);
                break;
            case CharactersManager.types.Mujer:
                all[3].SetActive(true);
                break;
            case CharactersManager.types.Loro:
                all[5].SetActive(true);
                break;
            case CharactersManager.types.Uriel:
                all[6].SetActive(true);
                break;
            default:
                all[4].SetActive(true);
                break;
        }
    }
    public void Idle()
    {
        GetComponent<Animation>().Play("idle");
    }
    public void Dance()
    {
        GetComponent<Animation>().Play("dance");
    }
    public void Appear()
    {
        GetComponent<Animation>().Play("appear");
    }
    public void Disapear()
    {
        GetComponent<Animation>().Play("disapear");
    }
    public void SetTalk(bool isOn)
    {
        GetComponentInChildren<AudioSpectrumView>().enabled = isOn;
    }
}
