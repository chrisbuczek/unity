using TMPro;
using UnityEngine;

public class GameSceneUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI livesTextMesh;
    [SerializeField] private TextMeshProUGUI scoreTextMesh;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        livesTextMesh.text = "Lives: " + GameManager.Instance.GetLives();
        scoreTextMesh.text = "Score: " + GameManager.Instance.GetScore();
    }
}
