using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Animator animate;
    private Rigidbody body;
    private bool isGrounded;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        //animate = GetComponent<Animator>();
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

        if (Input.GetKey(KeyCode.Space) && isGrounded)
            Jump();

        //sets animation parameters
        //animate.SetBool("run", horizontalIn != 0);
        //animate.SetBool("grounded", isGrounded);
    }

    private void Jump()
    {
        body.velocity = new Vector3(body.velocity.x, speed, body.velocity.z);
        //animate.SetTrigger("jump");
        isGrounded = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
            isGrounded = true;
    }
}