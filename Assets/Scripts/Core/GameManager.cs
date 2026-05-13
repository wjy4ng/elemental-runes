using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Ready, Playing, Clear, Fail }
    public GameState CurrentState { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ChangeState(GameState.Ready);
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        Debug.Log($"게임 상태 변경: {newState}");
    }

    public void StartGame()
    {
        ChangeState(GameState.Playing);
    }
}