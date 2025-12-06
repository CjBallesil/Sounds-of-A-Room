using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private Light lightComponent;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip lightOffSound;
    [SerializeField] private AudioClip lightOnSound;

    public bool CanInteract()
    {
        return true;
    }

    public bool Interact(Interactor interactor)
    {
        if (lightComponent != null)
        {
            if (lightComponent.enabled == true)
            {
                //light is on, turn off
                audioSource.clip = lightOffSound;
            }
            if(lightComponent.enabled == false)
            {
                //light is off, turn on
                audioSource.clip = lightOnSound;
            }
            audioSource.Play();
            lightComponent.enabled = !lightComponent.enabled;
            Debug.Log($"{gameObject.name} toggled {lightComponent.name}");
            return true;
        }
        Debug.LogWarning($"{gameObject.name} has no light assigned!");
        return false;
    }
        
}
