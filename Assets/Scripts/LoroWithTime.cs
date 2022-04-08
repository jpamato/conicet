using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoroWithTime : ScreenMain
{
    GamesData.Content content;
    int id = 0;
    public Text field;
    public SimpleButton simonCard;
    public List<SimpleButton> cards;
    public Transform container;
    public GameObject signal;
    public int cardID;
    bool canSelect;
    int ok;
    public AssetsData.loroWordsType loroWordsType;

    private void OnEnable()
    {
        field.text = "";
        cards.Clear();
        Utils.RemoveAllChildsIn(container);
        signal.SetActive(false);
    }
    public override void OnReady()
    {
        Utils.RemoveAllChildsIn(container);
        ok = 0;
        base.OnReady();
        string story_id = Data.Instance.storiesData.activeContent.id;
        content = Data.Instance.gamesData.GetContent(story_id);
        if (content == null) return;
        field.text = "";
        TextsData.Content tipContent = Data.Instance.daysData.GetTip("tip_loro_time");
        Events.OnCharacterSay(tipContent, OnTipDone, tipContent.character_type);
        int id = 0;
        foreach (string text in content.loro_time)
        {
            SimpleButton sb = Instantiate(simonCard, container);
            sb.transform.localScale = Vector2.one;
            string _text = GetParsedString(text);
            Sprite sprite = Data.Instance.assetsData.GetContent(_text).sprite;

            sb.Init(id, sprite, "", OnClicked);
            id++;
            cards.Add(sb);
        }
    }
    void Animate(string clipName)
    {
        foreach (SimpleButton sb in cards)
            sb.GetComponent<Animation>().Play(clipName);
    }
    void OnClicked(SimpleButton button)
    {
        if (!canSelect) return;
        canSelect = false;
        if (button.id == cardID)
        {
            button.GetComponent<SimpleFeedback>().SetState(SimpleFeedback.states.OK, 2);
            SetResults(true);
        }
        else
        {
            button.GetComponent<SimpleFeedback>().SetState(SimpleFeedback.states.WRONG, 2);
            SetResults(false);
        }
        Animate("allOn");
    }
    void SetResults(bool isOk)
    {
        if (isOk)
        {
            ok++;
            if (ok > 2)
                Events.SetReadyButton(OnReadyClicked);
        }
        id++;

        StartCoroutine(CheckResults());
        signal.SetActive(false);
    }
    void OnReadyClicked()
    {
        OnComplete();
        Events.OnGoto(true);
    }
    IEnumerator CheckResults()
    {
        yield return new WaitForSeconds(1);
        SetCard();
    }
    void OnTipDone()
    {
        id = 0;
        SetCard();
    }
    void SetCard()
    {
        signal.SetActive(true);
        id++;
        cardID = Random.Range(0, content.loro_time.Count);
        SayWord();
    }
    public void SayWord()
    {
        string text_id = content.loro_time[cardID];

        text_id = GetParsedString(text_id);

        string assetRealName = Data.Instance.assetsData.GetAssetRealName(text_id);
        assetRealName = Data.Instance.assetsData.GetSoundForLoro(assetRealName, loroWordsType);
        Events.PlaySoundTillReady("voices", "assets/audio" + Utils.GetLangFolder() + "/loro_" + assetRealName, null);

        text_id = Data.Instance.assetsData.GetRealText(text_id);
        field.text = text_id;
        Invoke("CanSelect", 0.5f);
    }
    void CanSelect()
    {
        if (gameObject.activeSelf)
            Animate("rotateRightLeft");
        canSelect = true;
    }
    ////////////////// por si la palabra del loro es default, inicio o final:
    string GetParsedString(string text_id)
    {        
        string[] arr = text_id.Split("@"[0]);
        if (arr.Length > 1)
        {
            text_id = arr[0];
            loroWordsType = Data.Instance.assetsData.SetTypeByText(arr[1]);
        }
        return text_id;
    }
}
