using UnityEngine;

public class StartLevel : MonoBehaviour
{
    [SerializeField] private Transform startPos;
    //Expression body syntax or function that can be one line of code
    void Start() => GameManager.Instance.InstantiatePlayer(startPos);
}