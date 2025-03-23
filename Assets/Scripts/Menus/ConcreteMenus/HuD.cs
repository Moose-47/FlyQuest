using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HuD : MonoBehaviour
{
    public Image healthBarImage;
    public Sprite[] healthSprites;

    public TMP_Text scoreTxt;
    public TMP_Text flyTxt;
    private void Start()
    {
        GameManager.Instance.OnLifeValueChanged.AddListener(UpdateHealthUI);
        GameManager.Instance.OnScoreGained.AddListener(updateScore);
        GameManager.Instance.OnFlyCollected.AddListener(updateFliesCollected);
        UpdateHealthUI(GameManager.Instance.hp);
        updateScore(GameManager.Instance.score);
        updateFliesCollected(GameManager.Instance.fly);
    }

    private void UpdateHealthUI(int currentHP)
    {
        if (healthSprites != null && healthSprites.Length == 5)
        {
           int index = Mathf.Clamp(currentHP, 0, 4);
           healthBarImage.sprite = healthSprites[index];
        }
    }
    private void updateScore(int score)
    {
        Debug.Log("Score: " + score);
        scoreTxt.SetText("Score: " + score);
    }
    private void updateFliesCollected(int fliesCollected)
    {
        flyTxt.text = ": " + fliesCollected;
    }
}