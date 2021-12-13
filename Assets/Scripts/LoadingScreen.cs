using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loading;
    public GameObject playButton;

    void Start()
    {
        playButton.SetActive(false);
        loading.gameObject.SetActive(true);
        Invoke("AllDataLoaded", 4);
    }
    void AllDataLoaded()
    {
        playButton.gameObject.SetActive(true);
        loading.gameObject.SetActive(false);
    }
    public void GotoSplash()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Splash");
    }
}
