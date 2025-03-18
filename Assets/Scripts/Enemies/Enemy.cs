using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class Enemy : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip squish;
    public AudioClip hurt;

    protected SpriteRenderer sr;
    protected Animator anim;

    protected int health;
    [SerializeField] protected int maxHealth;

    [SerializeField] protected int knockbackForce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {       
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (maxHealth <= 0) maxHealth = 1;
        health = maxHealth;
    }
    public virtual void takeDamage(int DamageValue, DamageType damageType = DamageType.Default)
    {
        health -= DamageValue;
        if (health > 0 && CompareTag("mush"))
        {
            anim.SetTrigger("hurt");
            audioSource.PlayOneShot(hurt);
        }
        if (health <= 0)
        {
            GameManager.Instance.score++;
            GameManager.Instance.OnScoreGained?.Invoke(GameManager.Instance.score);
            anim.SetTrigger("death");
            audioSource.PlayOneShot(squish);
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            Destroy(gameObject, 0.5f);           
        }
    }
    public enum DamageType
    {
        Default
    }
}