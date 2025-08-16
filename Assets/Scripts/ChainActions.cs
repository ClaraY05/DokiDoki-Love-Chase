using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LineRenderer))]
public class ChainActions : MonoBehaviour
{
    [Header("Grapple Settings")]
    public float maxDistance = 50f;
    public float pullSpeed = 15f;     
    public float swingSpring = 4.5f;   
    public float swingDamper = 7f;
    public LayerMask grappleLayer;

    private Rigidbody rb;
    private LineRenderer line;
    private Vector3 grapplePoint;
    private bool grappling = false;

    private float grappleCD = 1f;
    private float lastGrapple = -Mathf.Infinity;

    private SpringJoint springJoint;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        line = GetComponent<LineRenderer>();
        line.startWidth = 0.05f;
        line.endWidth = 0.05f;
        line.positionCount = 0;
    }

    void Update()
    {
        HandleInput();
        DrawRope();
    }

    private void HandleInput()
    {
        // Shoot grapple
        if (Input.GetMouseButtonDown(0) && Time.time >= lastGrapple + grappleCD)
        {
            TryGrapple();
            lastGrapple = Time.time;
        }

        // Release grapple
        if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }

        // Pull toward grapple point while holding left-click
        if (grappling && Input.GetMouseButton(0))
        {
            rb.useGravity = false;

            // Move player toward grapple point
            Vector3 direction = (grapplePoint - transform.position).normalized;
            rb.velocity = direction * pullSpeed;

            // Stop pulling if very close
            if (Vector3.Distance(transform.position, grapplePoint) < 1f)
            {
                StopGrapple();
            }
        }
        else
        {
            rb.useGravity = true;

            // If grapple is still active and released, create spring joint for swinging
            if (grappling && springJoint == null)
            {
                CreateSwingSpring();
            }
        }
    }

    private void TryGrapple()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, grappleLayer))
        {
            grappling = true;
            grapplePoint = hit.point;

            // Start drawing the rope
            line.positionCount = 2;
        }
    }

    private void StopGrapple()
    {
        grappling = false;
        line.positionCount = 0;

        if (springJoint != null)
        {
            Destroy(springJoint);
            springJoint = null;
        }
    }

    private void CreateSwingSpring()
    {
        springJoint = gameObject.AddComponent<SpringJoint>();
        springJoint.autoConfigureConnectedAnchor = false;
        springJoint.connectedAnchor = grapplePoint;

        float distance = Vector3.Distance(transform.position, grapplePoint);

        springJoint.maxDistance = distance * 0.8f;
        springJoint.minDistance = distance * 0.25f;
        springJoint.spring = swingSpring;
        springJoint.damper = swingDamper;
        springJoint.massScale = 4.5f;
    }

    private void DrawRope()
    {
        if (!grappling) return;

        line.SetPosition(0, transform.position);
        line.SetPosition(1, grapplePoint);
    }
}
