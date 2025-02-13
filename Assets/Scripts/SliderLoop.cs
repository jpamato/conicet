﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SliderLoop : MonoBehaviour
{
    int id;
    Image image;
    string folderName;
    public Sprite[] all;

    private void OnDisable()
    {
        CancelInvoke();
    }
    public void Init(string fileName)
    {
        all = null;
        CancelInvoke();
        image = GetComponent<Image>();

        id = 0;

        
        print("folderName: " + fileName);
        string[] arr = fileName.Split("_"[0]);
       // print("arr: " + arr.Length);
        if (arr.Length > 1)
        {
            folderName = arr[0].ToString();
            all = Resources.LoadAll<Sprite>("specialLoops/" + folderName + "/");
        }

            if (all != null && all.Length > 0)
            LoopFromSpecialFolder();
        else
            Loop();       
    }
    void LoopFromSpecialFolder()
    {
        if (id >= all.Length) id = 0;
        SetSprite( all[id] );
        Invoke("LoopFromSpecialFolder", 4);
        id++;
    }
    void Loop()
    {
        StoriesData.Content sContent = Data.Instance.storiesData.activeContent;
        Sprite sprite = GetNextImage(sContent.folder);
        //  Sprite sprite = Resources.Load<Sprite>(s);
        if (sprite == null)
        {
            id = 0;
            Invoke("Loop", 0.1f);
            return;
        }
        SetSprite( sprite );
        Invoke("Loop", 4);
        id++;
    }
    Sprite GetNextImage(string url)
    {
        if (id >30)
            id = 0;
        string s = "stories/" + url + "/images/" + (id + 1);
        print("___slider loop: " + s);
        Sprite sprite = Data.Instance.GetSprite(s);
        if (sprite != null)
            return sprite;
        else
        {
            id++;
            return GetNextImage(url);
        }
    }
    void SetSprite(Sprite s)
    {
        image.sprite = s;

        float _w = s.texture.width;
        float _h = s.texture.height;

        float factor = _h / image.GetComponent<RectTransform>().sizeDelta.y;

        _w = _w / factor;

        RectTransform rTransform = image.GetComponent<RectTransform>();
        rTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _w);
       // rTransform.anchoredPosition = new Vector3(-_w / 2, 0, 0);
        image.sprite = s;
    }
}
