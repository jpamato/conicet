using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillAmountAnim : MonoBehaviour
{
    public Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void Init()
    {
        image.fillAmount = 1;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        image.fillAmount = 0;
    }
    public void AnimateOff(float speed)
    {
        StopAllCoroutines();
        StartCoroutine(Anim(speed));
    }
    IEnumerator Anim(float speed)
    {
        while (image.fillAmount > 0)
        {
            image.fillAmount -= (speed / 10) * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
