using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private IInteractive interactiveObject;

    private IInteractive interactiveObjectShow;
    private List<string> bag = new List<string>(); 
    void Start()
    {
        SanitySystem.Instance.OnNoSanity += SanitySystem_OnNoSanity;
    }

    private void SanitySystem_OnNoSanity(object sender, EventArgs e)
    {
        gameObject.SetActive(false);
    }

    public void AddItemToBag(string item)
    {
        bag.Add(item);
    }

    public bool TryToGetItemFromBag(string itemName,out string itemOutput)
    {
        itemOutput = null;
        bool itemFound = false;
        foreach(string item in bag) {
            if (itemName == item)
            {
                itemFound = true;
                itemOutput = item;
                break;
            }
        }
        if(itemFound)
        {
            bag.Remove(itemOutput);
        }
        return itemFound;

    }

    void Update()
    {
        LookForInteractiveObjects();
        HandleInteractiveAction();
    }

    private void LookForInteractiveObjects()
    {
        float range = 1.5f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, range);

        foreach (Collider collider in colliderArray)
        {
            if (collider.gameObject.TryGetComponent<IInteractive>(out interactiveObject))
            {
                interactiveObjectShow = interactiveObject;
                interactiveObjectShow.ShowIcon();
                return;
            }
        }
        if (interactiveObjectShow != null)
        {
            interactiveObjectShow.HideIcon();
            interactiveObjectShow = null;
        } 
    }

    public void HandleInteractiveAction()
    {
        if(Input.GetKeyDown(KeyCode.E) && interactiveObject != null)
        {
            interactiveObject.Interact(this);
        }
    }

}
