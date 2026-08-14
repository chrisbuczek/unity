using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int STARTING_LIVES = 3;

    public static GameManager Instance {get; private set;}

    [SerializeField] Ball ball;
    [SerializeField] DeathTriggerZone deathTriggerZone;

    private int score;

    private int lives;
    private static bool isGameOver = false;

    private void Awake()
    {
        Instance = this;
        score = 0;
        lives = STARTING_LIVES;

    }

    private void Start()
    {
        ball.OnBrickDestroyed += Ball_OnBrickDestroyed;
        ball.OnDeathTriggerEntered += Ball_OnDeathTriggerEntered;
    }

    private void Ball_OnDeathTriggerEntered(object sender, EventArgs e)
    {
        RemoveLives();
        if(lives <= 0)
        {
        Debug.Log("GAME OVER!!!");
        Time.timeScale = 0f;
        //TODO: Move to GameOver scene  
        }
        
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
}
