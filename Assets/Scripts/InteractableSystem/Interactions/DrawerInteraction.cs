using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerInteraction : MonoBehaviour, IInteractable
{

    public enum SlideAxis { Forward, Back, Right, Left, Up, Down}

    [SerializeField] private float openDistance = 0.0005f;
    [SerializeField] private float moveDuration = .5f;
    [SerializeField] private SlideAxis slideAxis = SlideAxis.Forward;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;

    private bool isOpen = false;
    private Vector3 closedLocalPosition;
    private Coroutine moveRoutine;

    private void Awake()
    {
        closedLocalPosition = transform.localPosition;
    }

    private Vector3 GetOpenOffset()
    {
        switch(slideAxis)
        {
            case SlideAxis.Forward: return transform.forward * openDistance;
            case SlideAxis.Back: return -transform.forward * openDistance;
            case SlideAxis.Right: return transform.right * openDistance;
            case SlideAxis.Left: return -transform.right * openDistance;
            case SlideAxis.Up: return transform.up * openDistance;
            case SlideAxis.Down: return -transform.up * openDistance;
            default: return Vector3.zero;
        }
    }

    public bool CanInteract()
    {
        return true;
    }

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Drawer Interact called!");
        //stop any ongoing animation before starting a new one
        if(moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
        }

        //decide target based on open/closed state
        Vector3 targetPos = isOpen ? closedLocalPosition
            : closedLocalPosition + GetOpenOffset();

        moveRoutine = StartCoroutine(MoveDrawer(transform.localPosition, targetPos));

        //determine sound clip based on open/closed state
        if (isOpen == true)
        {
            audioSource.clip = closeSound;
        }
        if (isOpen == false)
        {
            audioSource.clip = openSound;
        }
        audioSource.Play();

        isOpen = !isOpen;
        return true;
    }

    private IEnumerator MoveDrawer(Vector3 startPos, Vector3 targetPos)
    {
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);

            transform.localPosition = Vector3.Lerp(startPos, targetPos, Mathf.SmoothStep(0f, 1f, t));

            yield return null;
        }

        transform.localPosition = targetPos; //snap to final position
    }
}
