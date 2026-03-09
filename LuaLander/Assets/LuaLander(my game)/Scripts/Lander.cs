using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class MyLander : MonoBehaviour
{
    private Rigidbody2D landerRigidbody2D;

    private void Awake()
    {
        landerRigidbody2D = GetComponent<Rigidbody2D>(); //get reference to Rigidbody2D from the same game object
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
            float force = 700f;
            // transform.up is object up with local transformations: rotation, position. New Vector2D(0, 1) will give global vector
            // Time.fixedDeltaTime makes super sure we are applying same force on every update. It's not necesary here, but I keep it.
            // You shouldn't use Time.deltaTime inside FixedUpdate, because it defeats the purpose of FixedUpdate.
            landerRigidbody2D.AddForce(force * transform.up * Time.fixedDeltaTime);
        }
        if(Keyboard.current.rightArrowKey.isPressed)
        {
            float turnSpeed = -100f; //never use magic numbers. Always assign a value to variable with proper name.
            landerRigidbody2D.AddTorque(turnSpeed * Time.fixedDeltaTime);
        }
        if(Keyboard.current.leftArrowKey.isPressed)
        {
            float turnSpeed = +100f;
            landerRigidbody2D.AddTorque(turnSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        //collision2D.gameObject - the object you hit. TryGetComponent - tries to find a component of a specific type. Returns true if component exist, otherwise false.
        //GetComponent returns null instead of false, so less clean.
        // Read about ref, out, in in C#
        // LandingPad landingPad = collision2D.gameObject.GetComponent<LandingPad>() is the similar as with out parameter below, but requires a null check
        // out lets you use landingPad inside if statement, in this method out parameter is required
        if(collision2D.gameObject.TryGetComponent(out LandingPad landingPad)) {
            Debug.Log($"LandingPad: {landingPad.gameObject.name}");
        }

        //magnitude of vector 2d is just his size. collision2d.relativeVelocity takes x axis and y axis of velocity vector
        float softLandingVelocityMagnitude = 4f;
        if(collision2D.relativeVelocity.magnitude > softLandingVelocityMagnitude)
        {
            //Landed too hard!
            Debug.Log("Landed too hard!");
            return;
        }

        //my solution without dot product
        // float landerAngle = transform.eulerAngles.z;
        // float angleAllowedForLanding = 30f;
        // if(landerAngle > angleAllowedForLanding && landerAngle < 360f - angleAllowedForLanding) {
        //     Debug.Log("Bad landing angle!");
        //     return;
        // }

        //Dot product. Pointing same direction = 1. 90 degrees = 0. Opposite direction = -1. 45 degrees = 0.5
        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float minDotVector = .90f;
        if(dotVector < minDotVector) {
            Debug.Log("Landed on a too steep angle!");
            return;
        }
    }
}

