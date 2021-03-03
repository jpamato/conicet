using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioPlayerManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    List<TimelineTextData> timelineTextData;
    [SerializeField] Slider slider;
    [SerializeField] GameObject panel;

    float duration;
    System.Action OnDone;    
    bool playing;
    int id;
    int nextKeyframe;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Events.SetAudioPlayer += SetAudioPlayer;
        Events.StopAudioPlayer += StopAudioPlayer;
        panel.SetActive(false);
    }
    private void OnDestroy()
    {
        Events.SetAudioPlayer -= SetAudioPlayer;
        Events.StopAudioPlayer -= StopAudioPlayer;
    }
    void StopAudioPlayer()
    {
        Reset();
        audioSource.Stop();
        panel.SetActive(false);
    }
    private void Reset()
    {
        id = 0;
        playing = false;
    }
    public void SetAudioPlayer(AudioClip clip, List<TimelineTextData> timelineTextData, System.Action OnDone)
    {
        Reset();
        panel.SetActive(true);
        this.OnDone = OnDone;
        print(clip);
        audioSource.clip = clip;
        duration = clip.length;
        this.timelineTextData = timelineTextData;
        audioSource.Play();
        Init();
        Events.OnNewKeyframeReached(id);
    }
    private void Init()
    {
        Reset();
        playing = true;
        nextKeyframe = timelineTextData[id].seconds;
    }
    void Update()
    {
        if (!playing)
            return;
      
        
        float timer = audioSource.time;
        slider.value = timer / duration;
        CheckKeyFrame(timer);

        if (!audioSource.isPlaying && OnDone != null)
        {
            OnDone();
            StopAudioPlayer();
        }
       
    }
    void CheckKeyFrame(float timer)
    {
        if (nextKeyframe == 0) return; //el ultimo seconds es zero, no se carga:
        if (timer > nextKeyframe)
        {
            id++;
            nextKeyframe = timelineTextData[id].seconds;
            Events.OnNewKeyframeReached(id);
        }
    }
}
