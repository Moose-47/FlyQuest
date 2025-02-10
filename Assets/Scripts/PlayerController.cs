using System.Collections;
using System.Linq;
using UnityEngine;

//Creates rigidbody2d attached to whatever this script is attached to and prevents removal of rigidbody2d from object.
//ReuireComponent can only contain 3 in a single statement, additonal lines would be required for more.
[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
[RequireComponent(typeof(LineRenderer))]
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
    private LineRenderer line;

    //groundCheck variables
    [Range(0.01f, 1.0f)]
    private Vector2 groundCheckSize = new Vector2(0.5f, 0.04f);
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

    // GRAPPLE VARIABLES
    public LayerMask grappleLayer; // Layer where grapple points exist
    public float grappleRange = 10f;
    public float tongueSpeed = 15f; // Speed of tongue extension
    public float pullSpeed = 10f; // Speed of player movement toward grapple point
    private Vector2 grapplePoint;
    private bool isGrappling = false;
    // Parameters for the dome shape and raycasting
    public float grappleRadius = 5f; // The radius of the grapple area
    public int numRays = 10; // Number of rays to cast in the arc
    public float grappleAngle = 90f; // The maximum angle on each side (90 degrees = 180 degree dome)
    public Transform frogMouth;

    private Vector2 lastCheckpoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //frogMouth = transform.Find("FrogMouth");
        //Assigning unity Rigidbody2D to var.
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        line = GetComponent<LineRenderer>();

        line.sortingOrder = 10;
        rb.linearVelocity = Vector3.zero;
        lastCheckpoint = transform.position;

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
        
        if (canMove())
        {
            LRmovement();
        }
        jump();
        animations();

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isGrappling)
        {
            isGrappling = true;
            CastGrappleRays();                     
        }
    }
    bool canMove()
    {
        if (isGrappling) return false;

        return true;
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
        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTimer > 0)
            {
                rb.gravityScale = 23; // Increase gravity while holding
                jumpTimer -= Time.deltaTime; // Decrease jump time
            }
        }
        // If the player releases jump, cut velocity for a short hop
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.3f); // Reduce upward speed
            isJumping = false;
            rb.gravityScale = 2; // Lower gravity for smoother fall
        }
        // Reset when grounded
        if (isGrounded)
        {
            isJumping = false;
            rb.gravityScale = 2; // Reset gravity when landing
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
        anim.SetBool("isGrappling", isGrappling);
    }

    void CastGrappleRays()
    {
        bool grappleFound = false;
        float closestDistance = float.MaxValue;

        float direction = sr.flipX ? 1f : -1f;
        float startAngle = 90f;
        float endAngle = startAngle + (direction * 120f);
        float angleStep = 120f / (numRays - 1);

        float offsetX = sr.flipX ? -0.3f : 0.3f;
        frogMouth.localPosition = new Vector3(offsetX, frogMouth.localPosition.y, frogMouth.localPosition.z);

        for (int i = 0; i < numRays; i++)
        {
            float angle = Mathf.Lerp(startAngle, endAngle, i / (float)(numRays - 1));
            Vector2 directionVector = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            // Get all objects in the path
            RaycastHit2D[] hits = Physics2D.RaycastAll(frogMouth.position, directionVector, grappleRange);

            //Debug.DrawRay(frogMouth.position, directionVector * grappleRange, Color.green, 0.1f);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {
                    // Check if the hit is on the ground layer (blocking)
                    if (((1 << hit.collider.gameObject.layer) & isGroundLayer) != 0)
                    {
                        break; // Stop if ground is blocking
                    }

                    // Check if the hit is on the grapple layer
                    if (((1 << hit.collider.gameObject.layer) & grappleLayer) != 0)
                    {
                        float distance = Vector2.Distance(frogMouth.position, hit.point);
                        if (distance < closestDistance)
                        {
                            grapplePoint = hit.point;
                            closestDistance = distance;
                            grappleFound = true;
                        }
                    }
                }
            }
        }
        if (grappleFound)
        {
            StartCoroutine(GrappleTongue());
        }
        else
        {
            isGrappling = false;
            grapplePoint = Vector2.zero;
        }
    }
    IEnumerator GrappleTongue()
    {
        isGrappling = true;
        line.enabled = true;

        rb.gravityScale = 0;

        Vector2 startPos = frogMouth.position;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * tongueSpeed / Vector2.Distance(startPos, grapplePoint);

            // Keep start fixed at frogMouth
            line.SetPosition(0, frogMouth.position);
            line.SetPosition(1, Vector2.Lerp(startPos, grapplePoint, t));

            yield return null;
        }

        // After extending, transition into the pulling phase
        StartCoroutine(PullToGrapplePoint());
    }
    IEnumerator PullToGrapplePoint()
    {
        while ((Vector2)transform.position != grapplePoint)
        {
            transform.position = Vector2.MoveTowards(transform.position, grapplePoint, pullSpeed * Time.deltaTime);

            // Keep start at frogMouth, move end to player
            line.SetPosition(0, frogMouth.position);
            line.SetPosition(1, grapplePoint);

            yield return null;
        }
        //Apply boost in the direction the player was moving
        Vector2 boostDirection = (grapplePoint - (Vector2)transform.position).normalized;
        rb.linearVelocity = new Vector2(boostDirection.x * 7f, 7f);
        rb.gravityScale = 2;
        line.enabled = false;
        isGrappling = false;
    }

    public void SetCheckpoint(Vector2 newCheckpoint)
    {
        lastCheckpoint = newCheckpoint;
    }
    public void Respawn()
    {
        transform.position = lastCheckpoint;
        rb.linearVelocity = Vector2.zero; // Reset momentum
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