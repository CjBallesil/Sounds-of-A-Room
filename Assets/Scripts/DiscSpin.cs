using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscSpin : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 180f; //degrees per second
    private bool spinning = false;

    public void StartSpinning()
    {
        spinning = true;
    }

    public void StopSpinning()
    {
        spinning = false;
    }

    private void Update()
    {
        if (spinning)
        {
            transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.Self);
        }
        if (gameObject.layer == LayerMask.NameToLayer("Held Object"))
        {
            spinning = false;
        }
    }
}
