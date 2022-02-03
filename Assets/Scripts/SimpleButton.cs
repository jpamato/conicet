using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleButton : MonoBehaviour
{
    public int id;
    public Image image;
    public Text field;
    public System.Action OnClicked;
    Button button;
    public string text;

    public void Init(int id, Sprite sprite, string text, System.Action<SimpleButton> OnClicked, bool forceUpperCase = true)
    {
        button = GetComponent<Button>();
        this.id = id;

        this.text = text;

        if (image != null)
        {
            if (sprite != null) image.sprite = sprite; else image.enabled = false;
        }
        if (text != "" && field != null)
        {
            if(forceUpperCase)
                field.text = Data.Instance.assetsData.GetRealText(text).ToUpper();
            else
                field.text = Data.Instance.assetsData.GetRealText(text);
        }
        else text = "";
        if(OnClicked != null)
            button.onClick.AddListener(() => OnClicked(this));
    }
    private void OnDestroy()
    {
        if(button != null)
            button.onClick.RemoveAllListeners();
        CancelInvoke();
    }
    public void InactivateFor(float timeToRestart)
    {
        Invoke("Reset", timeToRestart);
        button.interactable = false;
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
    private void Reset()
    {
        button.interactable = true;
    }
}
