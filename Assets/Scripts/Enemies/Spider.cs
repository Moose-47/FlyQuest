using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Spider : Enemy
{
    AudioSource audioSource;
    public AudioClip playerHurt;

    Rigidbody2D rb;
    [SerializeField] private float xVel;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.5f;
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] private float groundCheckOffsetX = 0.1f;
    [SerializeField] private float wallCheckDistance = 0.3f;
    [SerializeField] private Transform wallCheck;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        if (xVel <= 0) xVel = 1.5f;
    }

    private void Update()
    {
        bool isGroundAhead = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        Vector2 wallCheckDirection = sr.flipX ? Vector2.left : Vector2.right;
        bool isWallAhead = Physics2D.Raycast(wallCheck.position, wallCheckDirection, wallCheckDistance, groundLayer);
        // Flip direction if no ground is detected or if a wall is detected
        if (!isGroundAhead || isWallAhead)
        {
            Flip();
        }
        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);
        if (curPlayingClips[0].clip.name.Contains("walk"))
            rb.linearVelocity = (sr.flipX) ? new Vector2(xVel, rb.linearVelocityY) : new Vector2(-xVel, rb.linearVelocityY);
        else rb.linearVelocityX = 0.0f;
    }
    private void Flip()
    {
        sr.flipX = !sr.flipX;        
        gcFlip();
    }
    private void gcFlip()
    {
        if (groundCheck != null)
        {
            float direction = sr.flipX ? 1 : -1;
            groundCheck.localPosition = new Vector2(direction * groundCheckOffsetX / 1.5f, groundCheck.localPosition.y);
            wallCheck.localPosition = new Vector2(direction * groundCheckOffsetX / 1.5f, wallCheck.localPosition.y);
        }
    }
    public override void takeDamage(int DamageValue, DamageType damageType = DamageType.Default)
    {
        base.takeDamage(DamageValue, damageType);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {       
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.hp--;
            audioSource.PlayOneShot(playerHurt);
            Debug.Log(GameManager.Instance.hp);

            Rigidbody2D playerRB = GameManager.Instance.PlayerInstance.GetComponent<Rigidbody2D>();
            if (playerRB != null && GameManager.Instance.hp > 0)
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.dmgTaken();
                Vector2 knockbackDirection = (GameManager.Instance.PlayerInstance.transform.position - transform.position).normalized;
                playerRB.linearVelocity = knockbackDirection * knockbackForce;
            }
        }
    }
}