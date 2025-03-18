using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : BaseMenu
{
    public Button mainMenuBtn;
    public Button quitBtn;

    public TMP_Text fliesCollectedTxt;
    public TMP_Text beetlesCollectedTxt;
    public TMP_Text score;
    public override void Init(MenuController context)
    {
        base.Init(context);
        state = MenuStates.EndScreen;

        if (mainMenuBtn) mainMenuBtn.onClick.AddListener(() => GameManager.Instance.ChangeScene("Title"));
        if (quitBtn) quitBtn.onClick.AddListener(QuitGame);

        fliesCollectedTxt.SetText(GameManager.Instance.fly + " / 7");
        beetlesCollectedTxt.SetText(GameManager.Instance.beetles + " / 3");
        score.SetText("Score: " + GameManager.Instance.score);

        GameManager.Instance.hp = 4;
        GameManager.Instance.score = 0;
        GameManager.Instance.fly = 0;
        GameManager.Instance.beetles = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
