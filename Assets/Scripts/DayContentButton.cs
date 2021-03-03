using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayContentButton : MonoBehaviour
{
    public Text title;
    public Text field;
    DayScreen dayScreen;
    GameData gameData;
    DaysData.Content content;

    public void Init(DayScreen dayScreen, DaysData.Content content, GameData gameData)
    {
        this.content = content;
        this.gameData = gameData;
        this.dayScreen = dayScreen;
        StoriesData.Content storiesData = Data.Instance.storiesData.GetContent(content.story_id);
        title.text = storiesData.name;
        field.text = gameData.type.ToString();
    }
    public void Clicked()
    {
      //  dayScreen.OnContentClicked(content, gameData);
    }
}
