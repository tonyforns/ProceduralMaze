using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Enemy : MonoBehaviour
{
    public static EventHandler OnLookingAtCharacter;
    public static EventHandler OnStopLookingAtCharacter;
}
