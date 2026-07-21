using UnityEngine;
using UnityEngine.UI;

public class PausedUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.PauseUnpauseGame();
        });
    }

    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnPaused;

        Hide();
    }

    private void GameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();
    }

    private void GameManager_OnGameUnPaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    // HOW I TRIED TO DO IT, BUT ITS WRONG
    // you can't make PausedUI active to false when it has logic inside
    // private void Update()
    // {
    //     if (GameInput.Instance.IsPauseActionPressed())
    //     {
    //         if (isGamePaused)
    //         {
    //             GameManager.Instance.UnpauseGame();
    //             gameObject.SetActive(false);
    //         }
    //         else
    //         {
    //             GameManager.Instance.PauseGame();
    //             gameObject.SetActive(true);
    //         }
    //         isGamePaused = !isGamePaused;
    //     }
    // }
}
