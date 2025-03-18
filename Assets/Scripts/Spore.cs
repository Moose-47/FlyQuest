using UnityEngine;

public class Spore : MonoBehaviour
{
    [SerializeField] private float lifetime = 2.0f;
    [SerializeField] private float knockbackForce = 5.0f;
    AudioSource audioSource;
    public AudioClip frogHurt;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.CompareTag("spore") && collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.hp -= 2;
            GetComponent<CircleCollider2D>().enabled = false;
            audioSource.PlayOneShot(frogHurt);

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
