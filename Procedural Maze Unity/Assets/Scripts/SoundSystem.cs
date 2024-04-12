using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SoundSystem : Singleton<SoundSystem>
{
    [SerializeField] private List<AudioClipDef> clipList;

    [SerializeField] private AudioClipDef mainTheme;

    [SerializeField] private AudioSource mainAudioSource;

    [SerializeField] private AudioSource adrenalineAudioSource;

    [SerializeField] private List<AudioSource> audioSourceList;

    [SerializeField] private Dictionary<AudioClipDef.clipsName, AudioClipDef> audioClipDefDictionary;

    private void Start()
    {
        CreateDictionary();
        CreateMainTheme();
        CreateAdrenalineTheme();
    }

    private void CreateDictionary()
    {
        audioClipDefDictionary = new Dictionary<AudioClipDef.clipsName, AudioClipDef>();
        foreach (AudioClipDef audioClip in clipList)
        {
            audioClipDefDictionary.Add(audioClip.clipName, audioClip);
        }
    }

    public void PlayAdrenalineClip()
    {
        adrenalineAudioSource.Play();
    }

    public void StopAdrenalineClip()
    {
        StartFadeOut(adrenalineAudioSource, 0.5f);
    }

    public void PlayClip(AudioClipDef.clipsName clipName)
    {
        AudioSource audioSource = GetAudioSource();
        if(!audioClipDefDictionary.TryGetValue(clipName, out AudioClipDef audioClipDef))
        {
            Debug.LogError("AudioClip doesn't exist or load in the script: " + nameof(clipName));
            return;
        }
        SetAudioSource(audioSource, audioClipDef);
        audioSource.Play();
    }

    private void CreateMainTheme()
    {
        mainAudioSource = CreateAudioSource();
        SetAudioSource(mainAudioSource, mainTheme);
        mainAudioSource.Play();
    }

    private void CreateAdrenalineTheme()
    {
        audioClipDefDictionary.TryGetValue(AudioClipDef.clipsName.Adrenaline_High, out AudioClipDef  audioClipDef);
        adrenalineAudioSource = CreateAudioSource();
        SetAudioSource(adrenalineAudioSource, audioClipDef);
    }


    private void StartFadeOut(AudioSource audioSource, float fadeOutDuration)
    {
         StartCoroutine(FadeOutCoroutine(audioSource, fadeOutDuration));
    }

    private AudioSource CreateAudioSource()
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        return audioSource;
    }

    private void SetAudioSource(AudioSource audioSource, AudioClipDef audioClipDef)
    {
        audioSource.loop = audioClipDef.loop;
        audioSource.clip = audioClipDef.clip;
    }

    private AudioSource GetAudioSource()
    {
        foreach (AudioSource audio in audioSourceList)
        {
            if (audio.isPlaying) continue;
            return audio;
        }
        AudioSource audioSource = CreateAudioSource();
        audioSourceList.Add(audioSource);
        return audioSource;
    }
    

    private IEnumerator FadeOutCoroutine(AudioSource audioSource, float fadeOutDuration)
    {
        float timer = 0.0f;
        while (timer < fadeOutDuration)
        {
            Debug.Log("fadeOut");
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(1, 0.0f, timer / fadeOutDuration);
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = 1;
    }
}
