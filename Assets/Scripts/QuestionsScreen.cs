using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionsScreen : ScreenMain
{
    QuestionsManager.Content content;
    int id = 0;
    public Text field;
    public Image image;

    public override void OnReady()
    {
        string story_id = Data.Instance.storiesData.activeContent.id;
        content = Data.Instance.questionsManager.GetContent(story_id);
        if (content == null) return;
        Init();
    }
    private void Init()
    {

    }
    void SetCard()
    {
        field.text = content.texts[id];
    }
    void Next()
    {
        id++;
    }
}
