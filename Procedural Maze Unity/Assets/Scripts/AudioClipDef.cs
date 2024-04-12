using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioClipDef", menuName = "SO/AudioClipDef")]
public class AudioClipDef : ScriptableObject
{

    public enum clipsName{
        MainTheme,
        Adrenaline_Low,
        Adrenaline_High,
        Adrenaline_Mid,
        Stone_Wall

    }

    public clipsName clipName;
    public AudioClip clip;
    public bool loop;
}
