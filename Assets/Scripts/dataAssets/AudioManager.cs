using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public AudioSourceManager[] all;
    [Serializable]
    public class AudioSourceManager
    {
        public string sourceName;
        [HideInInspector] public AudioSource audioSource;
        public float volume = 1;
    }
    void Start()
    {
        Events.PlaySoundTillReady += PlaySoundTillReady;
        Events.PlaySound += PlaySound;
        Events.ChangeVolume += ChangeVolume;
        Events.OnGoto += OnGoto;
        foreach (AudioSourceManager m in all)
        {
            m.audioSource = gameObject.AddComponent<AudioSource>();
            m.audioSource.volume = m.volume;
        }
    }
    private void OnDestroy()
    {
        Events.PlaySoundTillReady -= PlaySoundTillReady;
        Events.ChangeVolume -= ChangeVolume;
        Events.PlaySound -= PlaySound;
        Events.OnGoto -= OnGoto;
    }
    void OnGoto(bool b)
    {
        OnDone = null;
    }
    void ChangeVolume(string sourceName, float volume)
    {
        AudioSource aSource = GetAudioSource(sourceName);
        aSource.volume = volume;
    }
    public AudioSource GetAudioSource(string sourceName)
    {
        foreach (AudioSourceManager m in all)
        {
            if (m.sourceName == sourceName)
                return m.audioSource;
        }
        return null;
    }
    void PlaySound(string sourceName, string audioName, bool loop)
    {
        PlaySoundAndReturn(sourceName, audioName, loop);
    }
    AudioSource PlaySoundAndReturn(string sourceName, string audioName, bool loop)
    {
        foreach(AudioSourceManager m in all)
        {
            if(m.sourceName == sourceName)
            {
                m.audioSource.Stop();
                m.audioSource.clip = Resources.Load<AudioClip>(audioName) as AudioClip;
                m.audioSource.Play();
                m.audioSource.loop = loop;
                return m.audioSource;
            }
        }
        return null;
    }
    bool playing;
    System.Action OnDone;
    AudioSource playingSource;
    void PlaySoundTillReady(string sourceName, string audioName, System.Action OnDone)
    {
        //Debug.Log("Play soung: " + sourceName + " audioName: " + audioName);
        playingSource = PlaySoundAndReturn(sourceName, audioName, false);
        this.OnDone = OnDone;
        playing = true;
    }
    void Update()
    {
        if (!playing)
            return;

        float timer = playingSource.time;
        if (!playingSource.isPlaying && OnDone != null && timer >0.1f)
        {
            OnDone();
            playing = false;
            playingSource = null;
        }

    }
}
