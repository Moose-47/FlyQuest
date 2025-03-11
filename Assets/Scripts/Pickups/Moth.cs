using UnityEngine;

public class Moth : MonoBehaviour, Pickup
{
    AudioSource audioSource;
    public AudioClip heal;
    public float bobSpeed = 2f;  
    public float bobAmount = 0.2f;

    public int addHP;

    private Vector2 startPos;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        startPos = transform.position;
    }

    private void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobAmount;
        transform.position = new Vector2(startPos.x, newY);
    }
    public void Pickup(PlayerController player)
    {
        GameManager.Instance.hp++;
        Debug.Log(GameManager.Instance.hp);
        audioSource.PlayOneShot(heal);
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, heal.length);
    }
}
