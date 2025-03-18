using UnityEngine;
using UnityEngine.Audio;

public class Mushroom : Enemy
{
    [SerializeField] private float fireRate = 2.0f;
    [SerializeField] private float atkRange = 5.0f;
    private float timeSinceLastFire = 0;

    private Transform player;

    AudioSource audioSource;
    public AudioClip playerHurt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
        if (fireRate <= 0) fireRate = 2.0f;
    }
    private void OnEnable()
    {
        GameManager.Instance.OnPlayerSpawned += OnPlayerSpawnedCallback;
    }
    private void OnDisable()
    {
        GameManager.Instance.OnPlayerSpawned -= OnPlayerSpawnedCallback;
    }
    private void OnPlayerSpawnedCallback(PlayerController controller)
    {
        player = controller.transform;
    }
    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        facePlayer();

        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);

        if (curPlayingClips[0].clip.name.Contains("idle")) checkFire();
    }
    void facePlayer()
    {
        if (player == null) return;

        if (player.position.x < transform.position.x)
            sr.flipX = true;
        else
            sr.flipX = false;
    }
    void checkFire()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= atkRange && Time.time >= timeSinceLastFire + fireRate)
        {
            anim.SetTrigger("fire");
            timeSinceLastFire = Time.time;
        }
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