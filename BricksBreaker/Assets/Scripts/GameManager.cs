using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    [SerializeField] Ball ball;
    [SerializeField] DeathTriggerZone deathTriggerZone;

    private static int score = 0;
    private static bool isGameOver = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ball.OnBrickDestroyed += Ball_OnBrickDestroyed;
        ball.OnDeathTriggerEntered += Ball_OnDeathTriggerEntered;
    }

    private void Ball_OnDeathTriggerEntered(object sender, EventArgs e)
    {
        isGameOver = true;
        Debug.Log("GAME OVER!!!");
        Time.timeScale = 0f;
        //TODO: Move to GameOver scene
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
}
