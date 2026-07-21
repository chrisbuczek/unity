using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    public enum Scene
    {
        //names in enum MUST match scene names inside Unity Editor, otherwise it won't work
        MainMenuScene,
        GameScene
    }

    public static void LoadScene(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }
}
