using UnityEngine;
using System.Collections;

public class KillZone : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip playerHurt;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                GameManager.Instance.hp--;
                audioSource.PlayOneShot(playerHurt);
                if (GameManager.Instance.hp > 0)
                {
                    StartCoroutine(RespawnAfterDelay(player, 0.5f));
                }
            }
        }
    }
    IEnumerator RespawnAfterDelay(PlayerController player, float delay)
    {
        yield return new WaitForSeconds(delay);
        player.Respawn();
    }
}