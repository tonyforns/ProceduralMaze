using System;
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