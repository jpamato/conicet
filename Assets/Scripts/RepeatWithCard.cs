using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepeatWithCard : ScreenMain
{
    public List<string> content;
    public SimpleButton simonCard;
    public Transform container;
    public Character character;
    [SerializeField] FillAmountAnim fillAmountAnim;

    public int done;
    int id;
    int lastcardID;
    public override void OnEnable()
    {
        base.OnEnable();
        fillAmountAnim.Init();
    }

    public override void OnReady()
    {
        id = 0;
        base.OnReady();

        string story_id = Data.Instance.storiesData.activeContent.id;
        GamesData.Content c = Data.Instance.gamesData.GetContent(story_id);
        content = c.GetContentFor(type, gameID);

        if (content == null) return;

        TextsData.Content tipContent = Data.Instance.daysData.GetTip("tip_repeat_with_card");
        print("________________" + tipContent.character_type);
        Events.OnCharacterSay(tipContent, OnTipDone, tipContent.character_type);
        done = 0;
    }
    public override void Show(bool fromRight)
    {
        id = 0;
        base.Show(fromRight);
        fillAmountAnim.Init();
        done = 0;
    }
    void OnTipDone()
    {
        fillAmountAnim.AnimateOff(10);
        AddCard();
    }
    public void Repeat()
    {
        if (audio_text != "")
        {
            string[] arr = audio_text.Split(":"[0]);
            if (arr.Length > 1)
                audio_text = arr[0];

            if (characterName == "silaba")
            {               
                audio_text = Data.Instance.assetsData.GetSoundForLoro(audio_text, AssetsData.loroWordsType.SILABA);
            }

            string s = "assets/audio" + Utils.GetLangFolder() + "/" + audio_text;

            if(characterName != "")
                character.Init(characterName);

            Events.PlaySoundTillReady("voices", s, WordSaid);
        }
    }
    public string characterName = "";
    public string audio_text = "";
    void AddCard()
    {
        print(content.Count);
        Utils.RemoveAllChildsIn(container);
        string textID = content[id];
        string[] arr = textID.Split("@"[0]);
        if(arr.Length>1)
        {
            textID = arr[0];
            characterName = arr[1];
        }



        arr = textID.Split(":"[0]);
        if (arr.Length > 1)
            textID = arr[0];


        print("ADD CARD: " + id + " textID: " + textID);

        SimpleButton sb = Instantiate(simonCard, container);
        sb.transform.localScale = Vector2.one;
        AssetsData.Content c = Data.Instance.assetsData.GetContent(textID);

        if (c == null)
            Events.Log("Falta Sprite para " + textID);
        else
        {
            Sprite sprite = c.sprite;
            sb.Init(id, sprite, "", null);
        }
        audio_text = textID;
        Repeat();

        done++;
        id++;
        if (id >= content.Count)
        { 
            id = 0;
        //if (done > 5)
        //{
            OnComplete();
            Events.SetReadyButton(OnReadyClicked);
        }
    }
    void OnReadyClicked()
    {
        Events.OnCharacterSay(null, null, CharactersManager.types.Dany);
        Utils.RemoveAllChildsIn(container);
        done = 0;
        OnComplete();
        Events.OnGoto(true);
    }
    IEnumerator SayNext()
    {
        yield return new WaitForSeconds(4);
        AddCard();
    }
    void WordSaid()
    {
        StartCoroutine(SayNext());
    }
}
