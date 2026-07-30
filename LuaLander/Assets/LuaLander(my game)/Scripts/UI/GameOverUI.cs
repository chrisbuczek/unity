using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] TextMeshProUGUI scoreTextMesh;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenuScene);
        });
    }

    private void Start()
    {
        scoreTextMesh.text = "TOTAL SCORE: " + GameManager.Instance.GetTotalScore().ToString();
        mainMenuButton.Select(); //this is needed for gamepad support, one button has to be preselected in the scene
    }
}

