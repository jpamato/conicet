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
    public virtual void OnReady(){ }
    public virtual void OnOff() { }

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
    IEnumerator AnimateIn(bool fromRight)
    {
        float init_x = Screen.width + Screen.width/2;
        float _y = Screen.height/2;
        float to = Screen.width / 2;
        if (!fromRight)
        {
            init_x *= -1;
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
        float init_x = Screen.width / 2;
        float _y = Screen.height / 2;
        float to = -Screen.width/2;
        if (!toLeft)
        {
            init_x *= -1;
        }
        transform.position = new Vector2(init_x, _y);

        float _x = transform.position.x;

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
