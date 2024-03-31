using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractive
{
    public void ShowIcon();
    public void HideIcon();
    public void Interact();
    public void Interact(Character character);
}
