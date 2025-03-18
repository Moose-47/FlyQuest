using UnityEngine;
using UnityEngine.Audio;

public class Beetle : MonoBehaviour, Pickup
{
    AudioSource audioSource;
    public AudioClip beetleCollect;
    
    public int addScore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void Pickup(PlayerController player)
    {
        GameManager.Instance.beetles++;
        GameManager.Instance.score += addScore;
        GameManager.Instance.OnScoreGained?.Invoke(GameManager.Instance.score);
        audioSource.PlayOneShot(beetleCollect);
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, beetleCollect.length);
    }
}