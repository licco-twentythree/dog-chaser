using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float walkSpeed;
    public float groundDrag;
    public float slideSpeed;

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;


    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public float playerWidth = 2;
    public LayerMask whatIsGround;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;

    public enum MovementState {
        walking,
        air,
        crouching,
        sliding, 
        stopping
    }

    public bool sliding;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        
        if (HitWall()) Debug.Log("Wallhit");

        MyInput();
        SpeedControl();
        StateHandler();
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

   
    private void StateHandler() {
        //walking 

        if (sliding)
        {
            state = MovementState.sliding;

            if (OnSlope() && rb.velocity.y < 0.1f)
            {

                desiredMoveSpeed = slideSpeed;
            }
            else
            {
                desiredMoveSpeed = walkSpeed;
            }

        }
        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        else if (Input.GetKey(crouchKey) && grounded)
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }

        //air
        else { state = MovementState.air; }

        //lerp speed

        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        
        else
        {
            moveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded) {
            readyToJump = false;
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKeyDown(crouchKey) && grounded) {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 4f, ForceMode.Impulse);
        }

        if (Input.GetKeyUp(crouchKey)) {
            Debug.Log(startYScale + crouchYScale);
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

        //test functions

        if (Input.GetKeyDown(KeyCode.E))
        {
            StopAllCoroutines(); 
        }
    }

    private void MovePlayer() {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //on slop
        if (OnSlope() && !exitingSlope) {
            rb.AddForce(GetSlopeMovement(moveDirection).normalized * moveSpeed * 20f, ForceMode.Force);
            if (rb.velocity.y > 0) {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        if (grounded && !OnSlope())
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        rb.useGravity = !OnSlope();

    }

    private void SpeedControl() {

        //limit on slope

        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }

        else {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }

    }

    private void Jump() {

        exitingSlope = true;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    public bool OnSlope() {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            //float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            //return angle < maxSlopeAngle && angle != 0;
        }
        
        return false;
    }

    public Vector3 GetSlopeMovement(Vector3 direction) {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    private IEnumerator SmoothlyLerpMoveSpeed() {
        float time = 0;
        float differece = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < differece) {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / differece);

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else {
                time += Time.deltaTime * speedIncreaseMultiplier;
            }

            if (!PlayerMoving() || HitWall())
            {
                StopAllCoroutines();
                moveSpeed = walkSpeed;
                Debug.Log("stopped moving");
            }
            yield return null;
        }





        /* float time = 0;
         float differece = Mathf.Abs(desiredMoveSpeed - moveSpeed);
         float startValue = moveSpeed;

         while (time < differece) {
             moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / differece);
             time += Time.deltaTime;
             yield return null;
             //yield return null;
         }

         moveSpeed = desiredMoveSpeed; */
    }

    private bool PlayerMoving() {
        if (horizontalInput != 0 || verticalInput != 0) {
            return true;
        }
        return false;
    }

    private bool HitWall() {
        Vector3 playerPosition = transform.position;
        Vector3 playerDirection = orientation.forward;
        
        Ray hitWallRay = new Ray(playerPosition, playerDirection);
        Debug.DrawRay(playerPosition, playerDirection * playerWidth * 0.5f);
        return Physics.Raycast(hitWallRay, playerWidth * 0.5f + 0.2f);
    }

}
