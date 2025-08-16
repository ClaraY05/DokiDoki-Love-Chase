using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask groundLayer;
    private Animator animate;
    private Rigidbody body;
    private BoxCollider boxCollider;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        //animate = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        float horizontalIn = Input.GetAxis("Horizontal");
        body.velocity = new Vector3(horizontalIn * speed, body.velocity.y, body.velocity.z);

        if (horizontalIn > 0.01f)
        {
            // Face right (world forward)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (horizontalIn < -0.01f)
        {
            // Face left (rotate 180 on Y)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded())
            Jump();

        //sets animation parameters
        //animate.SetBool("run", horizontalIn != 0);
        //animate.SetBool("grounded", isGrounded);
    }

    private void Jump()
    {
        body.velocity = new Vector3(body.velocity.x, speed*2, body.velocity.z);
        //animate.SetTrigger("jump");

    }

    private bool isGrounded()
    {
        float distanceToGround = 0.1f; // small buffer
        Vector3 origin = boxCollider.bounds.center;
        float halfHeight = boxCollider.bounds.extents.y;

        bool grounded = Physics.Raycast(origin, Vector3.down, halfHeight + distanceToGround, groundLayer);
        Debug.Log("IsGrounded: " + grounded);
        return grounded;
    }
}