using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    public BaseMenu[] allMenus;

    public MenuStates initState = MenuStates.MainMenu;

    private BaseMenu currentState;

    Dictionary<MenuStates, BaseMenu> menuDictionary = new Dictionary<MenuStates, BaseMenu>();
    Stack<MenuStates> menuStack = new Stack<MenuStates>();

    public string lvl = "Level";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (allMenus.Length <= 0)
        {
            allMenus = gameObject.GetComponentsInChildren<BaseMenu>(true);
        }

        foreach (BaseMenu menu in allMenus)
        {
            if (menu == null) continue;
            menu.Init(this);

            if (menuDictionary.ContainsKey(menu.state)) continue;

            menuDictionary.Add(menu.state, menu);
        }

        SetActiveState(initState);
    }

    public void JumpBack()
    {
        //in this instance- we should probably log the error.
        if (menuStack.Count <= 0) return;

        menuStack.Pop();
        if (menuStack.Count > 0)
        {
            SetActiveState(menuStack.Peek(), true);
        }
        else
        {
            //if stack empty this closes current menu
            if (currentState != null)
            {
                currentState.ExitState();
                currentState.gameObject.SetActive(false);
                currentState = null;
            }
        }
    }
    public void SetActiveState(MenuStates newState, bool isJumpingBack = false)
    {
        //if we don't have an active menu then we can't set the new state
        if (!menuDictionary.ContainsKey(newState)) return;
        //if we are already in the menu, exit the function
        if (currentState == menuDictionary[newState]) return;

        if (currentState != null)
        {
            currentState.ExitState();
            currentState.gameObject.SetActive(false);
        }

        currentState = menuDictionary[newState];
        currentState.gameObject.SetActive(true);
        currentState.EnterState();

        if (!isJumpingBack) menuStack.Push(newState);
    }

    private void TogglePauseMenu()
    {
        if (currentState != null && currentState.state == MenuStates.Pause)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    private void PauseGame()
    {
        SetActiveState(MenuStates.Pause);
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        JumpBack();
        if (menuDictionary.ContainsKey(MenuStates.Settings))
        {
            BaseMenu settingsMenu = menuDictionary[MenuStates.Settings];
            settingsMenu.ExitState();
            settingsMenu.gameObject.SetActive(false);
        }
        Time.timeScale = 1f;
    }
    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == lvl)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                TogglePauseMenu();
            }
        }
    }
}
