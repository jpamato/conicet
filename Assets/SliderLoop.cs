using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SliderLoop : MonoBehaviour
{
    int id;
    Image image;
    string folderName;
    Sprite[] all;

    private void OnDisable()
    {
        CancelInvoke();
    }
    public void Init(string fileName)
    {
        CancelInvoke();
        image = GetComponent<Image>();

        id = 0;

        
        print("folderName: " + fileName);
        string[] arr = fileName.Split("_"[0]);
        print("arr: " + arr.Length);
        if (arr.Length > 1)
        {
            folderName = arr[0].ToString();
            all = Resources.LoadAll<Sprite>("specialLoops/" + folderName + "/");
        }
       
        if (all != null && all.Length > 1)
            LoopFromSpecialFolder();
        else
            Loop();
       
    }
    void LoopFromSpecialFolder()
    {
        if (id >= all.Length) id = 0;
        image.sprite = all[id];
        Invoke("LoopFromSpecialFolder", 4);
        id++;
    }
    void Loop()
    {
        StoriesData.Content sContent = Data.Instance.storiesData.activeContent;
        if (id >= sContent.textsData.Count) id = 0;
        string s = "stories/" + sContent.folder + "/images/" + (id + 1);
        print(s);
        Sprite sprite = Resources.Load<Sprite>(s);
        image.sprite = sprite;
        Invoke("Loop", 4);
        id++;
    }
}
