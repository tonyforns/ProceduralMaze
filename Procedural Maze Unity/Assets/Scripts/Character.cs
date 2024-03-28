using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private IInteractive interactiveObject;

    private List<string> bag = new List<string>(); 
    void Start()
    {
        
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
                interactiveObject.ShowIcon();
                return;
            }
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
