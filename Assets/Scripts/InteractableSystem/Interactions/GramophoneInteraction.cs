using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GramophoneInteraction : MonoBehaviour, IInteractable, IObjectHolder
{
    [Header("Gramophone Setup")]
    [SerializeField] private Transform discPosition; //where disc sits in gramophone

    [Header("Sounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource discFXSource;
    [SerializeField] private AudioClip startSound;
    [SerializeField] private AudioClip endSound;
    [SerializeField] private AudioClip crackleSound;

    private GameObject currentDiscObject;
    private DiscData currentDiscData;
    private bool isPlaying = false;

    public bool CanInteract()
    {
        //get pick up system
        PlayerPickUpDrop pickUpSystem = FindObjectOfType<PlayerPickUpDrop>();

        //Case 1: player is holding disc
        if (pickUpSystem != null && pickUpSystem.HeldObject != null)
        {
            if(pickUpSystem.HeldObject.GetComponent<DiscData>() != null)
            {
                return true;
            }
        }

        //Case 2: disc already in gramophone
        if (currentDiscObject != null)
        {
            return true;
        }

        //otherwise, no valid interaction
        return false;
    }

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Interacted with gramophone!");
        PlayerPickUpDrop pickUpSystem = interactor.GetComponentInParent<PlayerPickUpDrop>();

        //Case 1: insert disc
        if (currentDiscObject == null && pickUpSystem != null && pickUpSystem.HeldObject != null)
        {
            ObjectGrabbable held = pickUpSystem.HeldObject;
            DiscData discData = held.GetComponent<DiscData>();

            if (discData != null)
            {
                held.Drop();
                AddObject(held.gameObject);
                pickUpSystem.ForceClearHeldObject();
                return true;
            }
        }

        //Case 2: toggle play/stop
        else if (currentDiscObject != null)
        {
            if (!isPlaying) StartPlayback();
            else StopPlayback();
            return true;
        }

        return false;
    }

    private void StartPlayback()
    {
        if (currentDiscData != null && currentDiscData.TrackClip() != null) {

            //start sound
            discFXSource.PlayOneShot(startSound);

            //start crackle loop
            discFXSource.clip = crackleSound;
            discFXSource.loop = true;
            discFXSource.PlayDelayed(startSound.length);
            isPlaying = true;

            //play disc track
            audioSource.clip = currentDiscData.TrackClip();
            audioSource.Play();

            //start spinning
            DiscSpin spin = currentDiscObject.GetComponent<DiscSpin>();
            if (spin != null) spin.StartSpinning();
        }
    }

    private void StopPlayback()
    {
        //stop music
        audioSource.Stop();

        // stop crackle loop (but don't Stop() the source yet)
        if (discFXSource.isPlaying && discFXSource.clip == crackleSound)
        {
            discFXSource.loop = false;
            discFXSource.Stop();
        }

        //play stop sound once
        discFXSource.PlayOneShot(endSound);

        isPlaying = false;

        if (currentDiscObject != null)
        {
            //stop spinning
            DiscSpin spin = currentDiscObject.GetComponent<DiscSpin>();
            if (spin != null) spin.StopSpinning();
        }
    }

    public void AddObject(GameObject discObject)
    {
        currentDiscObject = discObject;
        currentDiscData = discObject.GetComponent<DiscData>();

        //move disc to gramphone disc slot
        discObject.transform.SetParent(discPosition);
        discObject.transform.localPosition = Vector3.zero;
        discObject.transform.localRotation = Quaternion.identity;

        ObjectGrabbable discGrabbable = discObject.GetComponent<ObjectGrabbable>();
        discGrabbable.ReturnToLayer();
    }

    public void ReleaseObject(GameObject discObject)
    {
        if (currentDiscObject == discObject)
        {
            if (isPlaying)
            {
                StopPlayback();
            }
            currentDiscObject.transform.SetParent(null);
            currentDiscObject = null;
            currentDiscData = null;
            isPlaying = false;

            Debug.Log("Disc released from gramophone");
        }
    }

    private void Update()
    {
        if (isPlaying && audioSource.clip != null && !audioSource.isPlaying)
        {
            StopPlayback();
        }
    }
}
