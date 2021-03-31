using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayButton : MonoBehaviour
{
    public Image bgImage;

    public Color color_playedBG;
    public Color color_activeBG;
    public Color color_blockedBG;

    public Color color_playedField;
    public Color color_activeField;
    public Color color_blockedField;

    public GameObject done;
    public GameObject blocked;

    StoriesData.Content storyContent;
    Days manager;
    public Text dayField;
    DaysData.Content content;

    public Transform slotsContainer;
    public DayButtonSlot dayButtonSlot;

    
    public bool allPlayed = true;
    public states state;
    public enum states
    {
        READY,
        ACTIVE,
        BLOCKED
    }

    public void Init(Days manager, DaysData.Content content, states state)
    {
        this.manager = manager;
        this.content = content;
        storyContent = Data.Instance.storiesData.GetContent(content.story_id);
        dayField.text = content.day.ToString();
        int id = 0;
        foreach(GameData gd in content.games)
        {
            string savedValue = Data.Instance.lang.ToString() + "_" + content.day + "_" + id;
            int playedID = Data.Instance.userData.GetValue(savedValue);
            if (playedID > 0) gd.SetPlayed(true);
            print(savedValue + " playedID: " + playedID + " : " + gd.played);
            id++;
            DayButtonSlot slot = Instantiate(dayButtonSlot, slotsContainer);
            slot.Init(gd.played);
            if (!gd.played)
                allPlayed = false;
        }

        done.SetActive(false);

        this.state = state;
        if(state == states.ACTIVE)
        {
            bgImage.color = color_activeBG;
            dayField.color = color_activeField;
            blocked.SetActive(false);
            if (!allPlayed)
                GetComponent<Animation>().Play();
        }
        else if (state == states.BLOCKED)
        {
            GetComponent<Button>().interactable = false;
            blocked.SetActive(true);
            bgImage.color = color_blockedBG;
            dayField.color = color_blockedField;
        }
        if(allPlayed)
        {
            done.SetActive(true);
            bgImage.color = color_playedBG;
            dayField.color = color_activeField;
        }
    }
    public void Clicked()
    {
        manager.OnSelected(content);
    }
}
