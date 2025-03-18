using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
public class GameOverMenu : BaseMenu
{    
    public Button tryAgainBtn;
    public Button mainMenuBtn;
    public Button quitBtn;

    public CanvasGroup canvasGroup;

    public float fadeInDuration = 1f;
    public override void Init(MenuController context)
    {
        base.Init(context);
        state = MenuStates.GameOver;

        if (tryAgainBtn) tryAgainBtn.onClick.AddListener(() => GameManager.Instance.ChangeScene("Level"));
        if (mainMenuBtn) mainMenuBtn.onClick.AddListener(() => GameManager.Instance.ChangeScene("Title"));
        if (quitBtn) quitBtn.onClick.AddListener(QuitGame);
    }

    public void StartFadeIn()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
            StartCoroutine(FadeIn());
        }
    }

    private IEnumerator FadeIn()
    {
        float timeElapsed = 0f;

        while (timeElapsed < fadeInDuration)
        {
            timeElapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(timeElapsed / fadeInDuration);
            yield return null;
        } 
        canvasGroup.alpha = 1;
        GameManager.Instance.hp = 4;
        GameManager.Instance.score = 0;
    }
}