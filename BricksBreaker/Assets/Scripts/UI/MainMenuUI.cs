using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button playButton;

    private void Start()
    {
        //onClick is a UnityEvent (Addlistener, RemoveListener, Invoke);
        playButton.onClick.AddListener(PlayButtonClickHandler);
    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(PlayButtonClickHandler);
    }

    private void PlayButtonClickHandler()
    {
        Debug.Log("CLICK!");
        GameSceneManager.Instance.LoadScene(GameSceneManager.Scenes.GameScene);
    }

}
