using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject fanObject;
    [SerializeField] private AudioSource fanAudioSource;
    [SerializeField] private AudioSource switchAudioSource;

    [SerializeField] private AudioClip switchOffSound;
    [SerializeField] private AudioClip switchOnSound;
    [SerializeField] private AudioClip fanSound;

    private bool isSpinning = false;

    public bool CanInteract()
    {
        return true;
    }

    public bool Interact(Interactor interactor)
    {
        if (fanObject != null)
        {
            if (!isSpinning)
            {
                TurnOn();
                return true;
            }
            if (isSpinning)
            {
                TurnOff();
                return true;
            }
        }
        return false;
    }

    public void TurnOn()
    {
        if (fanObject != null && switchAudioSource != null && fanAudioSource != null)
        {
            //start sound
            switchAudioSource.PlayOneShot(switchOnSound);
            fanAudioSource.clip = fanSound;
            fanAudioSource.loop = true;
            fanAudioSource.Play();

            //start spinning
            DiscSpin spin = fanObject.GetComponent<DiscSpin>();
            if (spin != null) spin.StartSpinning();
            isSpinning = true;
        }
    }

    private void TurnOff()
    {
        switchAudioSource.PlayOneShot(switchOffSound);
        fanAudioSource.loop = false;
        fanAudioSource.Stop();

        //stop spinning
        if (fanObject != null)
        {
            DiscSpin spin = fanObject.GetComponent<DiscSpin>();
            if (spin != null) spin.StopSpinning();
            isSpinning = false;
        }
    }
}
