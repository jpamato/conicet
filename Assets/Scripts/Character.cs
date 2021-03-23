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
    public void Init(CharactersManager.types type)
    {
        if (lastType == type)
            return;
        lastType = type;
        Reset();
        print("character init " + type);
        switch (type)
        {
            case CharactersManager.types.Brisa:
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
            case CharactersManager.types.Hombre:
                all[4].SetActive(true);
                break;
        }
    }
}
