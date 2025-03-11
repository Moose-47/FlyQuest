using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class Enemy : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip squish;
    protected SpriteRenderer sr;
    protected Animator anim;
    protected int health;
    [SerializeField] protected int maxHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (maxHealth <= 0) maxHealth = 1;
        health = maxHealth;
    }
    public virtual void takeDamage(int DamageValue, DamageType damageType = DamageType.Default)
    {
        health -= DamageValue;
        
        if (health <= 0)
        {
            anim.SetTrigger("death");
            audioSource.PlayOneShot(squish);
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            if (transform.parent != null) Destroy(transform.parent.gameObject, squish.length);
            else Destroy(gameObject, 0.5f);
        }
    }
    public enum DamageType
    {
        Default
    }
}
