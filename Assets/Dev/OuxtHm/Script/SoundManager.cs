using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioMixer mixer;
    public AudioSource bgmPlayer;
    public AudioSource sfxPlayer;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
        bgmPlayer = transform.GetChild(0).GetComponent<AudioSource>();
        sfxPlayer = transform.GetChild(1).GetComponent<AudioSource>();
    }

    private void Start()
    {
        bgmPlayer.outputAudioMixerGroup = mixer.FindMatchingGroups("BGM")[0];
        sfxPlayer.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
    }

    public void MasterVolume(float val)
    {
        mixer.SetFloat("Master", Mathf.Log10(val) * 20);
    }
    public void BGMVolume(float val)
    {
        mixer.SetFloat("BGM", Mathf.Log10(val) * 20);

    }
    public void SFXVolume(float val)
    {
        mixer.SetFloat("SFX", Mathf.Log10(val) * 20);
    }
    public void BGMPlay(AudioClip clip)   // 배경음 재생
    {
        bgmPlayer.clip = clip;
        bgmPlayer.Play();
    }

    public void SFXPlay(AudioClip clip)     // 효과음 재생
    {
        sfxPlayer.clip = clip;
        sfxPlayer.Play();
    }

    public void SFXStop(AudioClip clip)
    {
        sfxPlayer.clip = clip;
        sfxPlayer.Stop();
    }
}
