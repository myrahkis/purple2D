using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float movementSpeed = 5.0f;

    private float inputDirection;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isDead = false;
    
    // runnung and turning
    private bool isRunning;
    private bool isFacingRight = true;

    // jumping
    public float jumpForce = 8.0f;
    public float fallMultiplier;
    public float lowJustMultiplier;

    public int availableJumps = 1;
    private int availableJumpsLeft;

    private bool canJump;

    // jump sound
    AudioSource jumpSound;

    // ground check
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float groundCheckCircle;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        availableJumpsLeft = availableJumps;
        jumpSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        if (isDead) {
            return;
        }

        CheckInput();
        CheckMovementDirection();
        UpdateAnimation();
        CheckIfCanJump();

        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJustMultiplier - 1) * Time.deltaTime;
        }
    }

    private void FixedUpdate() {
        ApplyMovement(); // в одно время
        CheckEnvironment();
    }

    private void CheckInput() {
        inputDirection = Input.GetAxisRaw("Horizontal"); //a and d

        if (Input.GetButtonDown("Jump")) {
            Jump();
        }
    }

    private void Jump() {
        if (canJump) {
            jumpSound.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            availableJumpsLeft--;
        }
    } 

    private void CheckIfCanJump() {
        if (isGrounded && rb.velocity.y <= 3) {
            availableJumpsLeft = availableJumps;
            
        }
        if (availableJumpsLeft <= 0) {
            canJump = false;
        } else {
            canJump = true;
        }
    }

    private void ApplyMovement() {
        rb.velocity = new Vector2(movementSpeed * inputDirection, rb.velocity.y);
    }

    //private void Turn() {
    //    isFacingRight = !isFacingRight;
    //    transform.Rotate(0.0f, 180.0f, 0.0f);
    //}

    private void CheckMovementDirection() {
        if (isFacingRight && inputDirection < 0) { // moving left
            transform.localScale = new Vector3(-5, 5, 1);
            isFacingRight = false;
        }
         if (!isFacingRight && inputDirection > 0 ) { // moving right
            transform.localScale = new Vector3(5, 5, 1);
            isFacingRight = true;
        }

        if (rb.velocity.x <= -0.5f | rb.velocity.x >= 0.5f) {
            isRunning = true;
        } else {
            isRunning = false;
        }
    }
    private void UpdateAnimation() {
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isGrounded", isGrounded);
    }

    private void CheckEnvironment() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckCircle, whatIsGround);
    }
        
    public void Die() {
        isDead = true;
        FindObjectOfType<LevelManager>().Restart();
    } 

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckCircle);
    }
}
