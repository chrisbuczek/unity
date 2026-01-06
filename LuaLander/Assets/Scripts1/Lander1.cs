using UnityEngine;
using UnityEngine.InputSystem;

public class Lander1 : MonoBehaviour
{
    private Rigidbody2D landerRigidbody2D;

    private void Awake()
    {
        landerRigidbody2D = GetComponent<Rigidbody2D>(); //get reference to Rigidbody2D from the same game object
        // Application.targetFrameRate = 10; // test physics with desired framerate
    }

    private void Start()
    {
        Debug.Log("Start");        
    }

    // this runs on fixed interval regardless of framerate = good for physics 
    private void FixedUpdate()
    {
        if(Keyboard.current.upArrowKey.isPressed)
        {
            // transform.up is object up with local transformations: rotation, position. New Vector2D(0, 1) will give global vector
            // Time.fixedDeltaTime makes super sure we are applying same force on every update. It's not necesary here, but I keep it.
            // You shouldn't use Time.deltaTime inside FixedUpdate, because it defeats the purpose of FixedUpdate.
            landerRigidbody2D.AddForce(transform.up);
        }
        if(Keyboard.current.rightArrowKey.isPressed)
        {
            Debug.Log("Right");
        }
        if(Keyboard.current.leftArrowKey.isPressed)
        {
            Debug.Log("Left");
        }
    }
}
