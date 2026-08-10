using UnityEngine;
using UnityEngine.InputSystem;

public class Platform : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    void Start()
    {
        
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
}
