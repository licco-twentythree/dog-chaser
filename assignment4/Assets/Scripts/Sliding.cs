using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    public MeshRenderer playerObjMesh;
    
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    public KeyCode slideKey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;

    //private bool sliding;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();


        startYScale = playerObj.localScale.y;
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0)) {
            StartSlide();
        }

        if (Input.GetKeyUp(slideKey) && pm.sliding) {
            StopSlide();
        }
    }

    private void StartSlide() {
        pm.sliding = true;
        playerObjMesh.enabled = false;
        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        rb.AddForce(Vector3.down * 4f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }

    private void FixedUpdate()
    {
        if (pm.sliding) {
            SlidingMovement();
        }
    }

    private void StopSlide()
    {
        playerObjMesh.enabled = true;
        pm.sliding = false;
        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (!pm.OnSlope() || rb.velocity.y > -0.1f)
        {           
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
            slideTimer -= Time.deltaTime;
        }
        else {
            rb.AddForce(pm.GetSlopeMovement(inputDirection) * slideForce, ForceMode.Force);
        }
        
        if (slideTimer <= 0) {
            StopSlide();

        }
    }

    // Update is called once per frame
    
}
