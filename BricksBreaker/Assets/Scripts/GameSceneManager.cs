using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance { get; private set; }

    // Names must match the scene asset names in Build Settings.
    public enum Scenes
    {
        MainMenuScene,
        GameScene,
        GameOverScene
    }

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

    public void LoadScene(Scenes scene)
    {
        // Clear any freeze left over from a game over, otherwise the next scene loads frozen.
        Time.timeScale = 1f;
        SceneManager.LoadScene(scene.ToString());
    }
}
