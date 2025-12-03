using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerContents : MonoBehaviour
{
    [SerializeField] private Collider drawerTrigger;
    [SerializeField] private Transform drawerObject;

    private void OneTriggerEnter(Collider other)
    {
        if (other == drawerObject) return; //ignore self

        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
        {
            //parent it to drawer so it moves with it
            other.transform.SetParent(drawerObject != null ? drawerObject : transform, true);
        }
    }

    private void OnTriggerExit (Collider other)
    {
        if (other == drawerObject) return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
        {
            //unparent when leaving drawer
            other.transform.SetParent(null, true);
        }
    }
}
