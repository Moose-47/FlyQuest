using UnityEngine;

//Creates rigidbody2d attached to whatever this script is attached to and prevents removal of rigidbody2d from object.
//ReuireComponent can only contain 3 in a single statement, additonal lines would be required for more.
[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    /*public makes it available in unity to change from in engine and not just from script
      having [x] applies to the below var. In this case setting the range that the player speed can be.*/
    [Range(3, 10)]
    public float speed = 5.0f;
    public float jumpForce = 10.3f;
    [Range(1, 10)]
    public float fallSpeed = 1.0f;

    bool grounded;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Assigning unity Rigidbody2D to var.
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.linearVelocity = Vector3.zero;
        
    }

    // Update is called once per frame
    void Update()
    {
        //Movement for left and right.
        float hInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(hInput * speed, rb.linearVelocity.y);

        if (rb.linearVelocity.x > 0) {
            sr.flipX = false; }
        if (rb.linearVelocity.x < 0) {
            sr.flipX = true; }

        if (Input.GetButtonDown("Jump") && grounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

 //This makes the player fall faster over time so the jump feels less floaty
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallSpeed - 1) * Time.deltaTime;
        }
    }
    // Called when the player's collider starts touching another collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player is touching the ground
        if (collision.gameObject.CompareTag("ground"))
        {
            grounded = true;
        }
    }

    // Called while the player's collider stays in contact with another collider
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            grounded = true;
        }
    }

    // Called when the player's collider stops touching another collider
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            grounded = false;
        }
    }
}