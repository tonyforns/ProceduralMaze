
using UnityEngine;

public class SanityFountain : MonoBehaviour, IInteractive
{

    [SerializeField] private Transform icon;
    [SerializeField] private Transform fountainAura;
    

    public void HideIcon()
    {
        icon.gameObject.SetActive(false);
    }

    public void Interact()
    {
        SanitySystem.Instance.IncreaseSanity();
        HideIcon();
        Collider collider = GetComponent<Collider>();
        collider.enabled = false;

        fountainAura.gameObject.SetActive(false);
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
