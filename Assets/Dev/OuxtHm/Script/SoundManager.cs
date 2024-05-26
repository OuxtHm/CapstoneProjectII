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

    public AudioClip boss_stage1;
    public AudioClip boss_stage2;
    public AudioClip boss_stage3;
    public AudioClip nomal_stage1;
    public AudioClip nomal_stage2;
    public AudioClip nomal_stage3;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        bgmPlayer = transform.GetChild(0).GetComponent<AudioSource>();
        sfxPlayer = transform.GetChild(1).GetComponent<AudioSource>();
    }

    private void Start()
    {
        bgmPlayer.outputAudioMixerGroup = mixer.FindMatchingGroups("BGM")[0];
        sfxPlayer.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        boss_stage1 = Resources.Load<AudioClip>("Sound/1stageBoss");
        boss_stage2 = Resources.Load<AudioClip>("Sound/2stageBoss");
        boss_stage3 = Resources.Load<AudioClip>("Sound/3stageBoss");
        nomal_stage1 = Resources.Load<AudioClip>("Sound/1stageNomal");
        nomal_stage2 = Resources.Load<AudioClip>("Sound/2stageNomal");
        nomal_stage3 = Resources.Load<AudioClip>("Sound/3stageNomal");
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
        bgmPlayer.loop = true;
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
