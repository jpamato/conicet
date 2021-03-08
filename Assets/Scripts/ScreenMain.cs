using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMain : MonoBehaviour
{
    float speed = 5;
    ScreensManager manager;
    public GameData.types type;

    public virtual void OnEnable()  {   }
    public virtual void OnDisable()  {   }

    public virtual void Init(ScreensManager manager)
    {
        this.manager = manager;
    }
    public virtual void OnReady()
    {

        GameData gd = Data.Instance.userData.GetActualActivity();
        print(gd.type + " played: " + gd.played);

        if(gd.type != GameData.types.all_days)
            Events.SetBackButton(true);

        if (gd.played)
            Events.SetNextButton(true);
        else
            Events.SetNextButton(false);
    }
    public virtual void OnOff() { }

    public void ForceOpen()
    {
        gameObject.SetActive(true);
        gameObject.transform.localPosition = Vector3.zero;
        OnReady();
    }
    public void Show(bool fromRight)
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        StartCoroutine(AnimateIn(fromRight));
    }
    public virtual void Hide(bool toLeft)
    {
        OnOff();
        StopAllCoroutines();
        StartCoroutine(AnimateOut(toLeft));
    }
    public virtual void OnComplete()
    {
        Data.Instance.userData.OnCompleteActivity();
    }
    IEnumerator AnimateIn(bool fromRight)
    {
        float init_x = Screen.width + Screen.width/2;
        float _y = Screen.height/2;
        float to = Screen.width / 2;
        if (!fromRight)
        {
            init_x = -Screen.width/2;
        }
        transform.position = new Vector2(init_x, _y);

        float _x = transform.position.x;

        if (fromRight)
        {
            while (transform.position.x - 1 > to)
            {
                yield return new WaitForEndOfFrame();
                transform.position = Vector3.Lerp(transform.position, new Vector2(to, _y), Time.deltaTime*speed);
            }
        }
        else
        {
            while (transform.position.x + 1 < to)
            {
                yield return new WaitForEndOfFrame();
                transform.position = Vector3.Lerp(transform.position, new Vector2(to, _y), Time.deltaTime * speed);
            }
        }
        transform.localPosition = Vector2.zero;
        OnReady();
    }
    IEnumerator AnimateOut(bool toLeft)
    {
        float _y = Screen.height / 2;
        float to = -Screen.width/2;
        if (!toLeft)
        {
            to = Screen.width + Screen.width / 2;
        }
        float _x = transform.position.x;
        yield return new WaitForEndOfFrame();

        if (toLeft)
        {
            while (transform.position.x - 1 > to)
            {
                yield return new WaitForEndOfFrame();
                transform.position = Vector3.Lerp(transform.position, new Vector2(to, _y), Time.deltaTime * speed);
            }
        }
        else
        {
            while (transform.position.x + 1 < to)
            {
                yield return new WaitForEndOfFrame();
                transform.position = Vector3.Lerp(transform.position, new Vector2(to, _y), Time.deltaTime * speed);
            }
        }
        gameObject.SetActive(false);
    }
}
