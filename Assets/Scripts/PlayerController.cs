using System.Collections;
using UnityEngine;

//Creates rigidbody2d attached to whatever this script is attached to and prevents removal of rigidbody2d from object.
//ReuireComponent can only contain 3 in a single statement, additonal lines would be required for more.
[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    /*public makes it available in unity to change from in engine and not just from script
      having [x] applies to the below var. In this case setting the range that the player speed can be.*/
    [Range(3, 10)]
    public float speed = 5.0f;
    [Range(10, 20)]
    public float jumpForce = 13f;
    
    public float maxJumpTime = 0.35f; //setting the maximum amount of time jump can be held to get the highest jump height

    private float hInput; //this is here as a global so my animations function can utilize it

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    //groundCheck variables
    [Range(0.01f, 1.0f)]
    private Vector2 groundCheckSize = new Vector2(0.8f, 0.04f);
    public LayerMask isGroundLayer;

    bool isGrounded = false;
    bool isJumping = false;
    private float jumpTimer = 0f;

    private Transform groundCheck;

    private int _score = 0;
    public int score
    {
        get => _score;
        set => _score = value;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Assigning unity Rigidbody2D to var.
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        rb.linearVelocity = Vector3.zero;

        //groundCheck init
        GameObject newGameObject = new GameObject();
        newGameObject.transform.SetParent(transform); //sets the position to the parents position
        newGameObject.transform.localPosition = Vector3.zero;
        newGameObject.name = "GroundCheck";
        groundCheck = newGameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        checkGrounded();
        LRmovement();
        jump();
        animations();

    }
    void LRmovement()
    {
        hInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(hInput * speed, rb.linearVelocity.y);
        if (rb.linearVelocity.x > 0) sr.flipX = false;
        if (rb.linearVelocity.x < 0) sr.flipX = true;
    }
    void jump()
    {
        //Variable jump based on length of time button is held
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
            jumpTimer = maxJumpTime;
            rb.gravityScale = 10;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (Input.GetButton("Jump") && rb.linearVelocity.y > 0 && isJumping && !isGrounded)
        {
            if (jumpTimer < maxJumpTime * 0.5f) rb.gravityScale = 23; //Gravity only increased further when button held down
            if (jumpTimer > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpTimer -= Time.deltaTime;
            }
            else isJumping = false;
        }

        if (rb.linearVelocity.y <= 0)
        {
            isJumping = false;
            rb.gravityScale = 2f;
        }
    }
    void checkGrounded()
    {
        if (!isGrounded)
        {
            if (rb.linearVelocity.y <= 0.1f) isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, isGroundLayer);
        }
        else isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, isGroundLayer);
    }
    void animations()
    {
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("speed", Mathf.Abs(hInput));
        anim.SetBool("isFalling", rb.linearVelocity.y < -0.1f);
    }

  

    //Detect Pickup
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Pickup pickup = collision.gameObject.GetComponent<Pickup>();
        if (pickup != null) pickup.Pickup(this);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Pickup pickup = collision.GetComponent<Pickup>();
        if (pickup != null) pickup.Pickup(this);
    }
}