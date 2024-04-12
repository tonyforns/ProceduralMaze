using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour, IInteractive
{
    [SerializeField] private Transform icon;
    [SerializeField] private Transform fountainAura;
    public void HideIcon()
    {
        icon.gameObject.SetActive(false);
    }

    public void Interact()
    {
        HideIcon();

    }

    public void Interact(Character character)
    {
        Interact();
    }

    public void ShowIcon()
    {
        icon.gameObject.SetActive(true);
    }
}
