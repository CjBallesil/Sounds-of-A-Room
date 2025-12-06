using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickUpLayerMask;
    public AudioSource pickUpAudioSource;

    private ObjectGrabbable objectGrabbable;
    public ObjectGrabbable HeldObject => objectGrabbable;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objectGrabbable == null)
            {
                //not carrying object, try to grab
                float pickUpDistance = 5f;
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask))
                {
                    Debug.Log("Pickup ray hit: " + raycastHit.collider.name);
                    if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                    {
                        //check if object is inside holder
                        IObjectHolder holder = raycastHit.transform.GetComponentInParent<IObjectHolder>();
                        if (holder != null)
                        {
                            holder.ReleaseObject(raycastHit.transform.gameObject);
                        }

                        Debug.Log("Hit: " + raycastHit.collider.name);
                        objectGrabbable.Grab(objectGrabPointTransform);
                    }
                }
            }
            else
            {
                //currently carrying something, drop
                objectGrabbable.Drop();
                objectGrabbable = null;
            }
        }
    }

    public void ForceClearHeldObject()
    {
        objectGrabbable = null;
    }
}
