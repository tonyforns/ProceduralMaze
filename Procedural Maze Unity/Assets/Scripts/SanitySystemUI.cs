using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SanitySystemUI : MonoBehaviour
{
    [SerializeField] private Image sanityBar;
    [SerializeField] private Transform noSanityMessage;

    private void Start()
    {
        SanitySystem.Instance.OnNoSanity += SanitySystem_OnNoSanity;
        noSanityMessage.gameObject.SetActive(false);
    }

    

    private void Update()
    {
        UpdateSanity();
    }

    private void UpdateSanity()
    {
        if (sanityBar != null)
        {
            sanityBar.fillAmount = SanitySystem.Instance.GetSanityNormalize();
        }
    }

    private void SanitySystem_OnNoSanity(object sender, EventArgs e)
    {
        noSanityMessage.gameObject.SetActive(true);
    }

}
