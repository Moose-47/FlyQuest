using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.UpdateRespawn(transform);
        Physics2D.IgnoreCollision(collision, GetComponent<Collider2D>());
    }
}
