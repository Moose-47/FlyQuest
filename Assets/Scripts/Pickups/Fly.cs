using UnityEngine;

public class Score : MonoBehaviour, Pickup
{
    public float bobSpeed = 2f;
    public float bobAmount = 0.2f;

    public int addScore;

    private Vector2 startPos;
    private void Start()
    {
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
        Destroy(gameObject);
    }
}
