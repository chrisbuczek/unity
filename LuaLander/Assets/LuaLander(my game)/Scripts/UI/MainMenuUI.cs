using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        Time.timeScale = 1f; // if we pause game and from PausedUI we go to MainMenu, the timeScale is 0f when we start game

        playButton.onClick.AddListener(() =>
        {
            GameManager.ResetStaticData();
            SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
        });

        quitButton.onClick.AddListener(() =>
        {
            // this won't work in unity editor. Only in build!
            Application.Quit();
        });
    }

    //preselect button for gamepad (not necessary for pc)
    private void Start()
    {
        playButton.Select();
    }
}
