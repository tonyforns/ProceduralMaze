
using UnityEngine;

public class SanityFountain : MonoBehaviour, IInteractive
{

    [SerializeField] private Transform icon;
    public void HideIcon()
    {
        icon.gameObject.SetActive(false);
    }

    public void Interact()
    {
        SanitySystem.Instance.IncreaseSanity();
        HideIcon();
        gameObject.SetActive(false);
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
