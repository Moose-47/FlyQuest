using UnityEngine;
using UnityEngine.UI;

public class HuD : MonoBehaviour
{
    public Image healthBarImage;
    public Sprite[] healthSprites;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLifeValueChanged.AddListener(UpdateHealthUI);
            UpdateHealthUI(GameManager.Instance.hp);
        }
    }

    private void UpdateHealthUI(int currentHP)
    {
        if (healthSprites != null && healthSprites.Length == 5)
        {
           int index = Mathf.Clamp(currentHP, 0, 4);
            healthBarImage.sprite = healthSprites[index];
        }
    }
}
