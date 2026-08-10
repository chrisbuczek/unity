using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    private static int score = 0;

    private void Awake()
    {
        Instance = this;
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
