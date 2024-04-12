using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioClipDef", menuName = "SO/AudioClipDef")]
public class AudioClipDef : ScriptableObject
{
    public string clipName;
    public AudioClip clip;
    public bool loop;
}
