using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public AudioSourceManager[] all;

    public bool playing;
    public System.Action OnDone;
    public AudioSource playingSource;

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
        playing = false;
        if (playingSource != null)
            playingSource.Stop();
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
                if (audioName != "" && m.audioSource.clip == null)
                {
                    Debug.LogError("No hay audio para " + audioName);
                    Events.Log("No hay audio para " + audioName);
                }
                    
                m.audioSource.Play();
                m.audioSource.loop = loop;
                return m.audioSource;
            }
        }
        return null;
    }
    float timeSinceStart;
    void PlaySoundTillReady(string sourceName, string audioName, System.Action OnDone)
    {
        timeSinceStart = 0;
        CancelInvoke();
        //Debug.Log("Play soung: " + sourceName + " audioName: " + audioName);
        playingSource = PlaySoundAndReturn(sourceName, audioName, false);
        this.OnDone = OnDone;
        playing = true;
    }
    void Update()
    {
        if (!playing)
            return;

        timeSinceStart += Time.deltaTime;
        if (timeSinceStart < 0.2f)
            return;

        float timer = playingSource.time;
        if (!playingSource.isPlaying && OnDone != null)
        {
            timeSinceStart = 0;
            OnDone();
            OnDone = null;
            playing = false;
            playingSource = null;
        }

    }
}
