using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour, IInteractable
{
    [SerializeField] private Light lightComponent;

    public bool CanInteract()
    {
        return true;
    }

    public bool Interact(Interactor interactor)
    {
        if (lightComponent != null)
        {
            lightComponent.enabled = !lightComponent.enabled;
            Debug.Log($"{gameObject.name} toggled {lightComponent.name}");
            return true;
        }
        Debug.LogWarning($"{gameObject.name} has no light assigned!");
        return false;
    }
        
}
