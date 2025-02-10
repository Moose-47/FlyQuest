using UnityEngine;

public class Score : MonoBehaviour, Pickup
{
    public int addScore;
    public void Pickup(PlayerController player)
    {
        player.score += addScore;
        Destroy(gameObject);
    }
}
