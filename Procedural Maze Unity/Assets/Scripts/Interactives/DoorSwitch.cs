using UnityEngine;

public class DoorSwitch : MonoBehaviour, IInteractive
{
    const float DOOR_COOLDOWN_TIME = 1;

    [SerializeField] private DoorController doorController;
    [SerializeField] private Transform interactiveIcon;
    private bool doorIsClose = true;

    private float timer;

    private void Update()
    {
        timer -= Time.deltaTime;
    }
    public void HideIcon()
    {
        interactiveIcon.gameObject.SetActive(false);
    }

    public void Interact()
    {
        if (timer > 0) return;
        timer = DOOR_COOLDOWN_TIME;
        doorIsClose = !doorIsClose;
        if(doorIsClose)
        {
            doorController.Close();
        } else
        {
            doorController.Open();
        }
        SoundSystem.Instance.PlayClip(AudioClipDef.clipsName.Stone_Wall);
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
