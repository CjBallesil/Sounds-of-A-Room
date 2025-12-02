using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private float castDistance = 5f;
    [SerializeField] private Vector3 raycastOffset = new Vector3(0, 0, 0);
    [SerializeField] private LayerMask interactLayerMask;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(DoInteractionTest(out IInteractable interactable))
            {
                if(interactable.CanInteract())
                {
                    interactable.Interact(this);
                }
            }
        }
    }

    private bool DoInteractionTest(out IInteractable interactable)
    {
        interactable = null;

        Ray ray = new Ray(transform.position + raycastOffset, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * castDistance, Color.red);

        if(Physics.Raycast(ray, out RaycastHit hitInfo, castDistance, interactLayerMask))
        {
            Debug.Log("Ray Hit: " + hitInfo.collider.name);

            interactable = hitInfo.collider.GetComponent<IInteractable>();
            if(interactable != null)
            {
                Debug.Log("found interactable: " + interactable);
                return true;
            }

            return false;
        }
        return false;
    }
}
