using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private int STARTING_LIVES = 3;

    public static GameManager Instance {get; private set;}

    [SerializeField] Ball ball;
    [SerializeField] DeathTriggerZone deathTriggerZone;

    private int score;

    private int lives;
    
    private GameState gameState;

    public enum GameState
    {
        WaitingToStart,
        Playing,
        Paused,
        GameOver
    }
    public event EventHandler<GameStateChangedEvent> OnGameStateChanged;

    public class GameStateChangedEvent : EventArgs
    {
        public GameState currentGameState = GameState.WaitingToStart;
    }

    private void Awake()
    {
        Instance = this;
        score = 0;
        lives = STARTING_LIVES;
        gameState = GameState.WaitingToStart;

    }

    private void Start()
    {
        ball.OnBrickDestroyed += Ball_OnBrickDestroyed;
        ball.OnDeathTriggerEntered += Ball_OnDeathTriggerEntered;
        LevelManager.Instance.OnSpawnCurrentLevel += LevelManager_OnSpawnCurrentLevel;
    }

    private void LevelManager_OnSpawnCurrentLevel(object sender, EventArgs e)
    {
        ChangeGameState(GameState.WaitingToStart);
    }

    private void Update()
    {
        if(gameState == GameState.WaitingToStart)
        {
            if(Keyboard.current.leftArrowKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
                ChangeGameState(GameState.Playing);
            }
        }
    }

    private void Ball_OnDeathTriggerEntered(object sender, EventArgs e)
    {
        RemoveLives();
        if(lives <= 0)
        {
            ChangeGameState(GameState.GameOver);
            return;
        }
        ChangeGameState(GameState.WaitingToStart);
    }

    private void Ball_OnBrickDestroyed(object sender, Ball.BrickDestroyedEventArgs e)
    {
        AddScore(e.points);
        Debug.Log("points add " + score.ToString());
    }

    public int GetScore()
    {
        return score;
    }

    public void AddScore(int points = 1)
    {
        score += points;
    }

    public int GetLives()
    {
        return lives;
    }

    public void RemoveLives(int amount = 1)
    {
        lives -= amount;
    }

    private void ChangeGameState(GameState newGameState)
    {
        gameState = newGameState;
        OnGameStateChanged.Invoke(this, new GameStateChangedEvent
        {
            currentGameState = newGameState
        });
    }
}
