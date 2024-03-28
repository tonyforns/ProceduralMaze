using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
[Serializable]
[CreateAssetMenu(fileName = "DungeonSegmentDef", menuName = "SO/DungeonSegmentDef")]
public class DungeonSegmentDef : ScriptableObject
{
    public string _name;
    public string id;
    public Transform segment;
    public int lightRotationOffset;

}