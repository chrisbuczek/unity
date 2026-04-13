using UnityEngine;

public class GameManager : MonoBehaviour
{
    //there are two approaches here. 
    //1. GameManager listens for events and then calls AddScore();
    //2. With singleton pattern in Lander.cs
    // both of those approaches are correct, because Lander shouldn't work if we deleted GameManager. GameManager is necessary for the game to work.

    // Singleton Pattern - when you need to access global reference

    // [SerializeField] private Lander lander; - used in Approach 1
    private int score;

    private void Start()
    {
        // Approach 1. event listener
        // lander.OnCoinPickup += Lander_OnCoinPickup;
        // Approach 2. singleton
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Instance.OnLanded += Lander_OnLanded;
    }

    private void Lander_OnCoinPickup(object sender, System.EventArgs e)
    {
        AddScore(500);
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        AddScore(e.score);
    }

    public void AddScore(int addScoreAmount)
    {
        score += addScoreAmount;
        Debug.Log(score);
    }
}
