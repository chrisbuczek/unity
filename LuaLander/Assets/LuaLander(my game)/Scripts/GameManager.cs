using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    //How GameManage can get Landers events? there are two approaches here. 
    //1. GameManager listens for events and then calls AddScore();
    //2. With singleton pattern in Lander.cs
    // both of those approaches are correct, because Lander shouldn't work if we deleted GameManager. GameManager is necessary for the game to work.
    public static GameManager Instance { get; private set; } // Approach 2 - Singleton Pattern - when you need to access global reference // Static fields belong to the class itself, not any instance
    // [SerializeField] private Lander lander; - used in Approach 1 (listening for events)

    // [SerializeField] private int levelNumber; this will not persist between SceneManager.LoadScene() -> have to make it static
    private static int levelNumber = 1; //static doesn't belong to any specific object, it belongs to the class itself. It will be accessible in other scenes than GameScene.
    private static int totalScore = 0; //will persist across scenes (can be acessed in GameOver), even if GameManager is only created in GameScene.

    public static void ResetStaticData()
    {
        levelNumber = 1;
        totalScore = 0;
    }

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    [SerializeField] private List<GameLevel> gameLevelList;
    [SerializeField] private CinemachineCamera cinemachineCamera;

    private int score;
    private float time;

    private bool isTimerActive = false;

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
        Lander.Instance.OnStateChanged += Lander_OnStateChanged;

        GameInput.Instance.OnMenuButtonPressed += GameInput_OnMenuButtonPressed;
        LoadCurrentLevel();
    }

    private void GameInput_OnMenuButtonPressed(object sender, System.EventArgs e)
    {
        PauseUnpauseGame();
    }

    private void Update()
    {
        if (isTimerActive)
        {
            time += Time.deltaTime;
        }
    }

    private void LoadCurrentLevel()
    {
        GameLevel gameLevel = GetGameLevel();
        GameLevel spawnedGameLevel = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
        //In the new level we need to spawn the Lander Instance
        Lander.Instance.transform.position = spawnedGameLevel.GetLanderStartPosition();
        cinemachineCamera.Target.TrackingTarget = spawnedGameLevel.GetCameraStartTargetTransform();
        //In order to set cinemachineCamera zoom we need to create custom class CinemachineCameraZoom2D
        CinemachineCameraZoom2D.Instance.SetTargetOrthographicSize(spawnedGameLevel.GetZoomedOutOrthographicSize());
    }

    private GameLevel GetGameLevel()
    {
        foreach (GameLevel gameLevel in gameLevelList)
        {
            if (gameLevel.GetLevelNumber() == levelNumber)
            {
                return gameLevel;
            }
        }
        return null;
    }

    private void Lander_OnCoinPickup(object sender, System.EventArgs e)
    {
        AddScore(500);
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        AddScore(e.score);
    }

    private void Lander_OnStateChanged(object sender, Lander.OnStateChangedEventArgs e)
    {
        isTimerActive = e.state == Lander.State.Normal;

        if (e.state == Lander.State.Normal)
        {
            cinemachineCamera.Target.TrackingTarget = Lander.Instance.transform;
            CinemachineCameraZoom2D.Instance.SetNormalOrthographicSize();
        }
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

    public void GoToNextLevel()
    {
        levelNumber++;
        totalScore += score;
        if (GetGameLevel() == null)
        {
            // no more levels
            SceneLoader.LoadScene(SceneLoader.Scene.GameOverScene);
        }
        else
        {
            // we still have some levels left
            //we only have one scene. Scene 0 has GameManager.cs that tracks current levelNumber;
            // SceneManager.LoadScene(0);
            SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
        }
    }

    public void RetryLevel()
    {
        // 0 is a magic number. We shouldn't use it!
        // SceneManager.LoadScene(0);
        SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
    }

    public int GetLevelNumber()
    {
        return levelNumber;
    }

    public void PauseUnpauseGame()
    {
        if (Time.timeScale == 1f)
        {
            PauseGame();
        }
        else
        {
            UnpauseGame();
        }
    }

    //this can't be inside PausedUI because it is being setActive to false when unpaused
    public void PauseGame()
    {
        Time.timeScale = 0f;
        OnGamePaused?.Invoke(this, EventArgs.Empty);
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        OnGameUnpaused?.Invoke(this, EventArgs.Empty);
    }

    public int GetTotalScore()
    {
        return totalScore;
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