using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //How GameManage can get Landers events? there are two approaches here. 
    //1. GameManager listens for events and then calls AddScore();
    //2. With singleton pattern in Lander.cs
    // both of those approaches are correct, because Lander shouldn't work if we deleted GameManager. GameManager is necessary for the game to work.
    public static GameManager Instance { get; private set; } // Approach 2 - Singleton Pattern - when you need to access global reference // Static fields belong to the class itself, not any instance

    // [SerializeField] private Lander lander; - used in Approach 1 (listening for events)
    private int score;
    private float time;

    private void Awake()
    {
        //runs first, even if the component is disabled in the Inspector. Use it for self-initialization, like getting references.
        //when class becomes undisabled it has already run the Awake() when the Scene was initialized
        // Warning: Can instance be null? Yes, another script calls GameManager.Instance before GameManager.Awake() has run (very rare, but possible if you access it in another object's Awake()). 
        Instance = this;
    }

    private void Start()
    {
        // Approach 1. event listener
        // lander.OnCoinPickup += Lander_OnCoinPickup; //
        // Approach 2. singleton
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup; //get refernce to the Lander via singleton pattern, access the event declared on that Lander object
        Lander.Instance.OnLanded += Lander_OnLanded;
    }

    private void Update()
    {
        time += Time.deltaTime;
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

    public int GetScore()
    {
        return score;
    }

    public float GetTime()
    {
        return time;
    }
}

// [SerializeField] is better when:
// The relationship is scene-specific (e.g. this UI belongs to this particular lander)
// You want the connection visible and configurable in the Inspector
// You might have multiple instances (e.g. 2 landers — which one do you wire up?)

// Singleton is better when:
// The object is truly global and there's only ever one (GameManager, AudioManager, ScoreSystem)
// You need to access it from many scripts — with SerializeField you'd have to wire it up manually in every single one
// You're accessing it from code that has no natural Inspector presence (e.g. a static utility)