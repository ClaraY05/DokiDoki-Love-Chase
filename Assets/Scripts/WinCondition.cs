using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    [SerializeField] private LayerMask mintLayer;
    private Rigidbody body;
    private BoxCollider boxCollider;
    private UiManager uiManager;


    // Start is called before the first frame update
    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        //animate = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        uiManager = FindObjectOfType<UiManager>();
    }



    // Update is called once per frame
    void Update()
    {
        withMint();
    }

    private bool withMint()
    {
        float distanceToGround = 0.1f; // small buffer
        Vector3 origin = boxCollider.bounds.center;
        float halfHeight = boxCollider.bounds.extents.y;

        bool withMint = Physics.Raycast(origin, new Vector3(transform.localScale.x, 0, 0), halfHeight + distanceToGround, mintLayer);
        Debug.Log("Winner" + withMint);
        uiManager.win();
        return withMint;
    }

}
