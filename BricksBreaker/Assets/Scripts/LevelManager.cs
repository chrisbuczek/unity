using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public event EventHandler OnSpawnCurrentLevel;

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDestroy() => SceneManager.sceneLoaded -= OnSceneLoaded;

    [SerializeField] List<GameObject> levelList;
    private GameObject currentLevel;

    private int currentLevelIndex = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //what is this doing?
        DontDestroyOnLoad(gameObject);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name != GameSceneManager.Scenes.GameScene.ToString()) return;
        currentLevelIndex = 0;
        SpawnCurrentLevel();   
    }

    public void LoadNextLevel()
    {
        if(currentLevelIndex + 1 >= levelList.Count)
        {
            GameSceneManager.Instance.LoadScene(GameSceneManager.Scenes.GameOverScene);
        } else
        {
        currentLevelIndex++;
        SpawnCurrentLevel();   
        }
    }

    private void SpawnCurrentLevel()
    {
        if(currentLevel != null) Destroy(currentLevel);
        // SceneManager.sceneLoaded is triggered after Awake() but before Start() - OnSceneLoaded() in this file
        // GameManager subscribes to OnSpawnCurrentLevel in Start(). OnSpawnCurrentLevel has zero subscribers - it's null. That is why we need OnSpawnCurrentLevel?.
        OnSpawnCurrentLevel?.Invoke(this, EventArgs.Empty);
        currentLevel = Instantiate(levelList[currentLevelIndex]);
    }
}
