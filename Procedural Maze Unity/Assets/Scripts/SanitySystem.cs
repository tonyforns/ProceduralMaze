using System;
using System.Collections.Generic;
using UnityEngine;

public class SanitySystem : Singleton<SanitySystem>
{

    public EventHandler OnNoSanity;

    [SerializeField] private float maxSanity;

    [SerializeField] private float sanityAmount;

    private bool isSanityDecreasing;
    private bool isNoSanity;
    private List<Enemy> enemyList;

    private void Start()
    {
        sanityAmount = maxSanity;
        isNoSanity = false;
        enemyList = new List<Enemy>();

        Enemy.OnLookingAtCharacter += Stalker_OnLookingAtCharacter;
        Enemy.OnStopLookingAtCharacter += Stalker_OnStopLookingAtCharacter;
    }

    private void Update()
    {
        if (isNoSanity) return;
        UpdateSanity();
    }

    private void Stalker_OnStopLookingAtCharacter(object sender, EventArgs e)
    {
        enemyList.Remove(sender as Enemy);
        if(enemyList.Count == 0)
        {
            SoundSystem.Instance.StopAdrenalineClip();
        }
    }

    private void Stalker_OnLookingAtCharacter(object sender, EventArgs e)
    {
        if(enemyList.Count == 0)
        {
            SoundSystem.Instance.PlayAdrenalineClip();
        }
        enemyList.Add(sender as Enemy);
    }

    private bool IsSanityDecreasing()
    {
        return enemyList.Count > 0;
    }

    private void UpdateSanity()
    {
        if (IsSanityDecreasing())
        {
            sanityAmount -= Time.deltaTime * enemyList.Count;
        }
        if(sanityAmount < 0)
        {
            OnNoSanity?.Invoke(this, EventArgs.Empty);
            isNoSanity = true;
        } 
    }

    public float GetSanityNormalize()
    {
        return sanityAmount / maxSanity;
    }

    public void IncreaseSanity()
    {
        sanityAmount += maxSanity * 0.2f;
        if(sanityAmount > maxSanity) sanityAmount = maxSanity;
    }
}
