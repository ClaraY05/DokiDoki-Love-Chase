using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    private Animator animate;
    private Rigidbody body;
    private BoxCollider boxCollider;
    private float wStickCD;


    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        //animate = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        float horizontalIn = Input.GetAxis("Horizontal");
        /*
        if (horizontalIn > 0.01f)
        {
            // Face right (world forward)
            transform.rotation = Quaternion.Euler(0, 0, 0);
            body.velocity = new Vector3(horizontalIn * speed, body.velocity.y, body.velocity.z);
        }
        else if (horizontalIn < -0.01f)
        {
            // Face left (rotate 180 on Y)
            transform.rotation = Quaternion.Euler(0, 180, 0);
            body.velocity = new Vector3(-1 * horizontalIn * speed, body.velocity.y, body.velocity.z);
        }*/

        if (Input.GetKey(KeyCode.Space) && isGrounded())
            Jump();

        // wall jump logic
        if (wStickCD > 0.2f)
        {
            if (Input.GetKey(KeyCode.Space))
                Jump();
            body.velocity = new Vector3(horizontalIn * speed, body.velocity.y, body.velocity.z);
            if (onWall() && !isGrounded())
            {
                body.useGravity = false;
                body.velocity = Vector3.zero;
            }
            else
                body.useGravity = true;
        }
        else
            wStickCD += Time.deltaTime;

        //sets animation parameters
        //animate.SetBool("run", horizontalIn != 0);
        //animate.SetBool("grounded", isGrounded);
    }

    private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector3(body.velocity.x, 8, body.velocity.z);
            //animate.SetTrigger("jump");
        }
        else if (onWall() && !isGrounded()) {
            wStickCD = 0;
            body.velocity = new Vector3(-Mathf.Sign(transform.localScale.x) * 3, 6, body.velocity.z);
        }
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

    private bool onWall()
    {
        float distanceToGround = 0.1f; // small buffer
        Vector3 origin = boxCollider.bounds.center;
        float halfHeight = boxCollider.bounds.extents.y;

        bool onWall = Physics.Raycast(origin, new Vector3(transform.localScale.x,0,0), halfHeight + distanceToGround, wallLayer);
        Debug.Log("IsOnWall: " + onWall);
        return onWall;
    }
}