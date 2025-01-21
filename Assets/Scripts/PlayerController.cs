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
    public float jumpForce = 10.3f;
    [Range(1, 10)]
    public float fallSpeed = 1.0f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    //groundCheck variables
    [Range(0.01f, 1.0f)]
    private Vector2 groundCheckSize = new Vector2(0.8f, 0.04f);
    public LayerMask isGroundLayer;

    bool isGrounded = false;

    private Transform groundCheck;

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
        //Movement for left and right.
        float hInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(hInput * speed, rb.linearVelocity.y);
        if (rb.linearVelocity.x > 0) sr.flipX = false;       
        if (rb.linearVelocity.x < 0) sr.flipX = true;
        
   //Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        //This makes the player fall faster over time so the jump feels less floaty
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallSpeed - 1) * Time.deltaTime;
        }

        //animation tags
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("speed", Mathf.Abs(hInput));
        anim.SetBool("isFalling", rb.linearVelocity.y < -0.1f);
    }
    void checkGrounded()
    {
        if (!isGrounded)
        {
            if (rb.linearVelocity.y <= 0) isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, isGroundLayer);
        }
        else isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, isGroundLayer);
    }
}