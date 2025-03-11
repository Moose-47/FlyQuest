using UnityEngine;

public class Score : MonoBehaviour, Pickup
{
    AudioSource audioSource;
    public AudioClip flyCollect;
    public float bobSpeed = 2f;
    public float bobAmount = 0.2f;

    public int addScore;

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
        GameManager.Instance.score++;
        audioSource.PlayOneShot(flyCollect);
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, flyCollect.length);
    }
}
