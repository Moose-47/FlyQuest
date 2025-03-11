using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections;
using System.Linq;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip playerDeath;

    private MenuController menuController;

    private static GameManager _instance;
    public static GameManager Instance => _instance;
    public event Action<PlayerController> OnPlayerSpawned;
    public UnityEvent<int> OnLifeValueChanged;
    #region Game Properties
    private int _score = 0;
    public int score
    {
        get => _score;
        set => _score = value;
    }

    [SerializeField] private int maxHP = 4;
    private int _hp = 4;
    public int hp
    {
        get => _hp;
        set
        {
            if (value <= 0)
            {     
                _playerInstance.Die();
                StartCoroutine(Death(1.5f));
                _hp = 0;
                OnLifeValueChanged?.Invoke(hp);
                return;
            }

            _hp = value;
            if (_hp > maxHP) _hp = maxHP;

            OnLifeValueChanged?.Invoke(_hp);
        }
    }
    #endregion
    #region Player Controller
    [SerializeField] private PlayerController playerPrefab;
    private PlayerController _playerInstance;
    public PlayerController PlayerInstance => _playerInstance;
    #endregion
    private Transform currentRespawn;


    public void SetMenuController(MenuController menuController) => this.menuController = menuController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(this);
            return;
        }
        Destroy(gameObject);
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (hp <= 0) hp = 4;
        //menuController = gameObject.AddComponent<MenuController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            string sceneName = (SceneManager.GetActiveScene().name.Contains("Level")) ? "Title" : "Level";
            SceneManager.LoadScene(sceneName);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        { score++; Debug.Log(_score); }

        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name == "GameOver")
            SceneManager.LoadScene("Title");
    }
    void gameOver()
    {
        Time.timeScale = 1f;
        menuController.ShowGameOverMenu();
    }
    public void InstantiatePlayer(Transform spawnLocation)
    {
        _playerInstance = Instantiate(playerPrefab, spawnLocation.position, Quaternion.identity);
        currentRespawn = spawnLocation;
        OnPlayerSpawned?.Invoke(_playerInstance);
    }
    public void UpdateRespawn(Transform updatedRespawn)
    {
        currentRespawn = updatedRespawn;
        Debug.Log("Checkpoint updated");
    }
    public void ChangeScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
    IEnumerator Death(float delay)
    {
        audioSource.PlayOneShot(playerDeath);
        yield return new WaitForSeconds(delay);

        gameOver();
    }
}