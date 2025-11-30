using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    Vector2 rotation;
    public Camera cam;
    public float lookSensitivity;
    public float moveSpeed;
    public float jumpHeight;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Movement input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // Mouse look
        rotation.y += Input.GetAxis("Mouse X") * lookSensitivity;
        rotation.x -= Input.GetAxis("Mouse Y") * lookSensitivity;
        rotation.x = Mathf.Clamp(rotation.x, -80f, 80f);

        //rotate player + camera
        rb.MoveRotation(Quaternion.Euler(0f, rotation.y, 0f));
        cam.transform.localRotation = Quaternion.Euler(rotation.x, 0f, 0f);

        //jump
        if (Input.GetButtonDown("Jump")) Jump();

        //apply movement
        Vector3 moveDir = transform.TransformDirection(new Vector3(moveX, 0, moveZ).normalized);
        Vector3 targetVelocity = moveDir * moveSpeed;
        rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);

        //camera follow
        cam.transform.position = transform.position + new Vector3(0, 1.5f, 0);

    }

    void Jump()
    {
        if (Physics.Raycast(transform.position, Vector3.down , 1.2f))
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
    }

}
