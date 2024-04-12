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


    [SerializeField] private Dictionary<string, AudioClipDef> audioClipDefDictionary;

    private void Start()
    {
        CreateDictionary();
        CreateMainTheme();
        CreateAdrenalineTheme();
    }

    private void CreateDictionary()
    {
        audioClipDefDictionary = new Dictionary<string, AudioClipDef>();
        foreach (AudioClipDef audioClip in clipList)
        {
            audioClipDefDictionary.Add(audioClip.name, audioClip);
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

    private void CreateMainTheme()
    {
        mainAudioSource = CreateAudioSource(mainTheme);
        mainAudioSource.Play();
    }

    private void CreateAdrenalineTheme()
    {
        audioClipDefDictionary.TryGetValue("Adrenaline_Hight", out AudioClipDef  audioClipDef);
        adrenalineAudioSource = CreateAudioSource(audioClipDef);
    }

    private AudioSource CreateAudioSource(AudioClipDef audioClipDef)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = audioClipDef.loop;
        audioSource.clip = audioClipDef.clip;
        return audioSource;
    }

    public void StartFadeOut(AudioSource audioSource, float fadeOutDuration)
    {
         StartCoroutine(FadeOutCoroutine(audioSource, fadeOutDuration));
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
