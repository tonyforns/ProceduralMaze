using UnityEngine;

public class DoorSwitch : MonoBehaviour, IInteractive
{
    [SerializeField] private DoorController doorController;
    [SerializeField] private Transform interactiveIcon;
    private bool doorIsClose = true;

    public void HideIcon()
    {
        interactiveIcon.gameObject.SetActive(false);
    }

    public void Interact()
    {
        doorIsClose = !doorIsClose;
        if(doorIsClose)
        {
            doorController.Close();
        } else
        {
            doorController.Open();
        }
    }

    public void Interact(Character character)
    {
        Interact();
    }

    public void ShowIcon()
    {
        interactiveIcon.gameObject.SetActive(true);
    }
}
