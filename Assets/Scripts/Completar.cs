using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Completar : ScreenMain
{
    [SerializeField] FillAmountAnim fillAmountAnim;
    bool gameReady;
    int id = 0;
    public Character character;
    public Character[] characters; // para cuando hay 2

    public GameObject musicAsset;

    public int typeID;
    // 1: lee de corrido
    // 2: dice el character
    // 3: silencio

    public string folderName;
    public string characterName;
    public string prefix = "";
    public GameObject arrow;

    public GameObject doubleCharacters;
    public Image image;

    public override void OnEnable()
    {
        base.OnEnable();

        fillAmountAnim.Init();
        foreach (Character ch in characters)
            ch.gameObject.SetActive(false);
        arrow.SetActive(false);
    }
    public override void OnDisable()
    {
        base.OnDisable();
    }
    public override void OnOff()
    {
        Events.StopAudioPlayer();
        Events.OnCharacterSay(null, null, CharactersManager.types.Nasheli);
    }
    public override void OnReady()
    {
        foreach (Character ch in characters)
            ch.gameObject.SetActive(false);

        base.OnReady();
        TextsData.Content tipContent = Data.Instance.daysData.GetTip("tip_completar");
        Events.OnCharacterSay(tipContent, OnTipDone, tipContent.character_type);
    }
    public override void Hide(bool toLeft)
    {
        base.Hide(toLeft);
        fillAmountAnim.Init();
    }
    string imageName;
    string fileName;
    void OnTipDone()
    {
        print("Completar OnTipDone");
        prefix = "";
        id = 1;
        fillAmountAnim.AnimateOff(10);
        StoriesData.Content story_content = Data.Instance.storiesData.activeContent;

        GamesData.Content gameDataContent = Data.Instance.gamesData.GetContent(story_content.id);
        string content = gameDataContent.GetContentFor(type, gameID)[0];
        string[] rimaImageSplited = content.Split("@"[0]);
        fileName = content;
        if (rimaImageSplited.Length > 1)
        {
            fileName = rimaImageSplited[0];
            imageName = rimaImageSplited[1];
        }
        string[] arr = fileName.Split("_"[0]);
        typeID = int.Parse(arr[0]);
        folderName = arr[1];
        characterName = arr[2];

        SetCharacter();

        Next();
    }

    int doubleCharacterID = 0;
    void SetCharacter()
    {
        if (imageName != "")
        {
            Sprite sprite = Resources.Load<Sprite>("rimas/" + imageName);
            if(sprite == null)
                image.enabled = false;
            else
                image.sprite = sprite;
        }
        else
        {
            image.enabled = false;
            Debug.Log("Falta imagen para rima: " + fileName);
        }

        if (typeID == 1)
        {
            doubleCharacters.SetActive(true);

            //esto es viejo:
            foreach (Character ch in characters)
                ch.gameObject.SetActive(true);   

            CharactersManager.types characterType = (CharactersManager.types)System.Enum.Parse(typeof(CharactersManager.types), characterName);
            characters[1].Init(characterType);
            character.gameObject.SetActive(false);
            /////////////////////
        }
        else
        {
            doubleCharacters.SetActive(false);
            foreach (Character ch in characters)
                ch.gameObject.SetActive(false);
            arrow.SetActive(false);

            CharactersManager.types characterType = (CharactersManager.types)System.Enum.Parse(typeof(CharactersManager.types), characterName);
            character.Init(characterType);
            character.gameObject.SetActive(true);
        }
    }
    void Next()
    {
        if (prefix == "a")
        {
            prefix = "b";
            doubleCharacterID = 1;
            characters[0].SetTalk(false);
            characters[1].SetTalk(true);
        }
        else 
        {
            prefix = "a";
            doubleCharacterID = 0;
            characters[1].SetTalk(false);
            characters[0].SetTalk(true);
        }
        if (typeID == 1)
        {
            arrow.SetActive(true);
            arrow.transform.position = characters[doubleCharacterID].transform.position;
        }

        if (prefix == "a")
        {
            Events.ChangeVolume("voices", 1);
            if (typeID == 2) { 
                character.Appear();
                character.GetComponentInChildren<AudioSpectrumView>().enabled = true;
            }
            if (characterName == "cancion")
            {
                musicAsset.SetActive(true);
                character.GetComponentInChildren<AudioSpectrumView>().enabled = false;
                character.Dance();
            } else
            {
                musicAsset.SetActive(false);
            }
        }
        else
        {
            if (typeID == 1)
            {
                //character.Appear();
               // characters[doubleCharacterID].GetComponentInChildren<AudioSpectrumView>().enabled = true;
            }
            else
            {
                Events.ChangeVolume("voices", 0);
            }

            musicAsset.SetActive(false);
        }
        Invoke("Say", 0.5f);       
    }
    public void Say()
    {
        string text = id + prefix;
        string url = "completar/" + folderName + "/" + text;
        print("Completar Next: " + url);
        AudioClip ac = Resources.Load<AudioClip>(url);
        if (ac == null)
        {
            arrow.SetActive(false);
            OnComplete();
        }
        else
        {
            Events.PlaySoundTillReady("voices", url, SayNext);
        }
    }
    void SayNext()
    {
        if (prefix == "b")
        {
            id++;
            //if (typeID == 1)
            //    character.Disapear();
        }
        else
        {
           // if (typeID == 2)
                //character.Disapear();
        }
        CancelInvoke();

        Invoke("Next", 1);
    }
    public override void OnComplete()
    {
        base.OnComplete();


        Events.SetReadyButton(OnReadyClicked);
    }
    void OnTextDone()
    {
        OnComplete();
        Events.SetNextButton(true);
    }
    void OnReadyClicked()
    {
        foreach (Character ch in characters)
            ch.gameObject.SetActive(false);
        Events.OnGoto(true);
    }
}

