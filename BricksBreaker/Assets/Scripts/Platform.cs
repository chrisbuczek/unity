using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Platform : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector3 startPosition = new Vector3(0, -4.5f, 0);



    void Start()
    {
        transform.position = startPosition;
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(object sender, GameManager.GameStateChangedEvent e)
    {
        if(e.currentGameState == GameManager.GameState.WaitingToStart)
        {
            ResetPlatform();
        }
    }

    void FixedUpdate()
    {
        if(Keyboard.current.leftArrowKey.isPressed)
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        if(Keyboard.current.rightArrowKey.isPressed)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
    }

    public void ResetPlatform()
    {
        transform.position = startPosition;        
    }
}
