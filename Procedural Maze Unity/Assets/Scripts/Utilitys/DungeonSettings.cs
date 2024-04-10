using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class DungeonSettings : MonoBehaviour
{
    public static LayerMask ObstacleMask;
    public static LayerMask CharacterMask;

    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask characterMask;

    private void Awake()
    {
        ObstacleMask = obstacleMask;
        CharacterMask = characterMask;
    }
}
