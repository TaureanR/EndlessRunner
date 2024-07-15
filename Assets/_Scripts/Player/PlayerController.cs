using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //the distance between two lanes
    [SerializeField] private float laneDistance = 4;
    private int desiredLane = 1; // 0:Left 1:Mid 2:Right

    private CharacterController controller;
    private PlayerManager playerManager;
    private Vector3 direction;

    private Animator animator;
    private bool isTripping;
    private int movementParamId;
    private int isJumpingParamId;
    //private int isTrippingParamId;

    // Speed of the player character
    private float forwardSpeed = 20.0f;
    private float gravity;
    private float jumpForce = 8.5f;
    private float maxSpeed = 65.0f;

    public float Gravity
    {
        get { return gravity; }
        set { gravity = value; }
    }
    public float JumpForce
    {
        get { return jumpForce; }
        set { jumpForce = value; }
    }

    public float ForwardSpeed
    {
        get { return forwardSpeed; }
        set { forwardSpeed = value; }
    }

    private void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        Time.timeScale = 1;

        animator.SetBool("isDead", false);
        movementParamId = Animator.StringToHash("Movement");
        isJumpingParamId = Animator.StringToHash("isJumping");
        //isTrippingParamId = Animator.StringToHash("isTripping");

        gravity = -22.0f;
    }

    
    void Update()
    {
        //handles what lane we should be in (Left 0, Middle 1, Right 2)
        LaneHandler();
        
        PlayerMovement();

        QuickDrop(); // Drop fast on swipe down

        IncreaseSpeed(); // Slowly increases speed every frame

        GroundedCheck(); // Checks if player is on the ground

        if (!PlayerManager.isGameStarted)
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isRunning", false);
            return;
        }
        animator.SetBool("isIdle", false);

        controller.Move(direction * Time.deltaTime);
    }

    void PlayerMovement()
    {
        if(PlayerManager.gameOver == true)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isDead", true);
            direction.z = 0;
            return;
        }

        direction.z = forwardSpeed;

        // Figure out what lane we should be in by input
        if (SwipeManager.swipeRight)
        {
            animator.SetFloat(movementParamId, 1.0f);
            Debug.Log("I moved right");

            desiredLane++;
            if (desiredLane == 3)
            {
                desiredLane = 2;
            }
        }

        else if (SwipeManager.swipeLeft)
        {
            animator.SetFloat(movementParamId, -1.0f);
            Debug.Log("I moved left");

            desiredLane--;
            if (desiredLane == -1)
            {
                desiredLane = 0;
            }
        }
        else
        {
            animator.SetFloat(movementParamId, 0.0f);
        }
    }

    void LaneHandler()
    {
        // Calculate where we should be in the future
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        // Determines which lane we should be in
        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }


        // Smooth movemment left and right
        if (transform.position == targetPosition)
            return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;

        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);
    }

    public void IncreaseSpeed()
    {
        if(forwardSpeed < maxSpeed)
        {
            forwardSpeed += 0.1f * Time.deltaTime;
        }
        
    }
    void Jump()
    {
        direction.y = jumpForce;
    }

    void GroundedCheck()
    {
        if (controller.isGrounded && PlayerManager.isGameStarted
            && PlayerManager.gameOver == false)
        {
            animator.SetBool("isRunning", true);

            if(SwipeManager.swipeUp)
            {
                animator.SetBool("isRunning", false);
                animator.SetBool(isJumpingParamId, true);
                Jump();
                Debug.Log("I jumped");
            }
        }
        else
        {
            // Apply gravity
            direction.y += gravity * Time.deltaTime;
            animator.SetBool(isJumpingParamId, false);
        }
    }

    void QuickDrop()
    {
        if (controller.isGrounded)
        {
            return;
        }
        else if (SwipeManager.swipeDown)
        {
            direction.y += gravity * 40 * Time.deltaTime;
            animator.SetBool(isJumpingParamId, false);
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "HardObstacle")
        {
            PlayerManager.gameOver = true;
            playerManager.TakeFatalDamage();
            animator.SetBool("isDead", true);
            animator.SetBool("isRunning", false);
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Obstacle")
        {
            playerManager.TakeDamage();

            isTripping = true;
            animator.SetBool("isTripping", true);
            animator.SetBool("isRunning", false);

            Debug.Log("I just tripped D:");
            StartCoroutine(ResetTrippingFlag());
        }
    }

    private IEnumerator ResetTrippingFlag()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        isTripping = false;
        animator.SetBool("isTripping", false);
        animator.SetBool("isRunning", true);
    }
}
