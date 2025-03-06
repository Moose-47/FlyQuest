using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
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
                //gameOver();
                return;
            }
            //if (_hp > value) Respawn();

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
        if (hp <= 0) hp = 5;
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
        SceneManager.LoadScene("GameOver");
        Debug.Log("Game Over goes here");
        hp = 4;
        score = 0;
    }
    void Respawn()
    {
        _playerInstance.transform.position = currentRespawn.position;
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
}