using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Lander : MonoBehaviour
{
    //singleton pattern
    // static - property belongs to a class and not to a class instance itself
    // public get to property, set is private - only this class can change it.
    public static Lander Instance { get; private set; }

    public event EventHandler OnUpForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler OnCoinPickup;
    //explain this custom args with generics
    public event EventHandler<OnLandedEventArgs> OnLanded;
    public class OnLandedEventArgs: EventArgs {
        public int score;
    }

    private Rigidbody2D landerRigidbody2D;
    private float fuelAmount = 10f;  

    private void Awake()
    {
        Instance = this;
        landerRigidbody2D = GetComponent<Rigidbody2D>(); //get reference to Rigidbody2D from the same game object
    }

    // private void Update()
    // {
        //thrusters are just a visual element, so they shouldn't be placed in main Lander logic.
    //     leftThruster.SetActive(Keyboard.current.leftArrowKey.isPressed);
    // }

    // this runs on fixed interval regardless of framerate = good for physics 
    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);
        // Debug.Log("FuelAmount: " + fuelAmount);

        if(fuelAmount <= 0f) {
            return;
        }

        if(Keyboard.current.upArrowKey.isPressed)
        {
            if(Keyboard.current.leftArrowKey.isPressed && Keyboard.current.rightArrowKey.isPressed) {
                float ultraForce = 1400f;
                landerRigidbody2D.AddForce(ultraForce * transform.up * Time.fixedDeltaTime);    
                ConsumeFuel(1.5f);
            } else {
                float force = 700f;
                // transform.up is object up with local transformations: rotation, position. New Vector2D(0, 1) will give global vector
                // Time.fixedDeltaTime makes super sure we are applying same force on every update. It's not necesary here, but I keep it.
                // You shouldn't use Time.deltaTime inside FixedUpdate, because it defeats the purpose of FixedUpdate.
                landerRigidbody2D.AddForce(force * transform.up * Time.fixedDeltaTime);
                // ENVOKE THE EVENT. this = the MyLander object raising the event. Any subscriber can cast it back to know which lander tirggered the event.
                ConsumeFuel(1.5f);
            }
            OnUpForce?.Invoke(this, EventArgs.Empty);
        }
        if(Keyboard.current.rightArrowKey.isPressed)
        {
            float turnSpeed = -100f; //never use magic numbers. Always assign a value to variable with proper name.
            landerRigidbody2D.AddTorque(turnSpeed * Time.fixedDeltaTime);
            OnRightForce?.Invoke(this, EventArgs.Empty);
            ConsumeFuel();
        }
        if(Keyboard.current.leftArrowKey.isPressed)
        {
            float turnSpeed = +100f;
            landerRigidbody2D.AddTorque(turnSpeed * Time.fixedDeltaTime);
            OnLeftForce?.Invoke(this, EventArgs.Empty);
            ConsumeFuel();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        //collision2D.gameObject - the object you hit. TryGetComponent - tries to find a component of a specific type. Returns true if component exist, otherwise false.
        //GetComponent returns null instead of false, so less clean.
        // Read about ref, out, in in C#
        // LandingPad landingPad = collision2D.gameObject.GetComponent<LandingPad>() is similar to the one with out parameter below, but requires a null check
        // out lets you use landingPad inside if statement, in this method out parameter is required
        // NEVER COMPARE WITH OBJECT'S STRING NAME. ADD LANDINGPAD.CS EMPTY CLASS TO IDENTIFY IT 
        if(!collision2D.gameObject.TryGetComponent(out LandingPad landingPad)) {
            Debug.Log("Crashed on Terrain!");
        }

        //magnitude of vector 2d is just his size. collision2d.relativeVelocity takes x axis and y axis of velocity vector
        float softLandingVelocityMagnitude = 4f;
        float relativeVelocityMagintude = collision2D.relativeVelocity.magnitude;
        if( relativeVelocityMagintude > softLandingVelocityMagnitude)
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

        Debug.Log("Successful landing!");
        float maxScoreAmountLandingAngle = 100;
        float scoreDotVectorMultiplier = 10f;
        float landingAngleScore = maxScoreAmountLandingAngle - Mathf.Abs(dotVector - 1f) * scoreDotVectorMultiplier * maxScoreAmountLandingAngle;

        float maxScoreAmountLandingSpeed = 100;
        float landingSpeedScore = (softLandingVelocityMagnitude - relativeVelocityMagintude) * maxScoreAmountLandingSpeed;

        Debug.Log("landingAngleScore: " + landingAngleScore);  
        Debug.Log("landingSpeedScore: " + landingSpeedScore);  

        int score = Mathf.RoundToInt((landingAngleScore + landingSpeedScore) * landingPad.GetScoreMultiplier());
        OnLanded?.Invoke(this, new OnLandedEventArgs {
            score = score,
        });
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.gameObject.TryGetComponent(out FuelPickup fuelPickup)) {
        float pickedUpFuel = fuelPickup.GetFuel();
        fuelAmount += pickedUpFuel;
        fuelPickup.DestroySelf();
        }
        
        if(collider2D.gameObject.TryGetComponent(out CoinPickup coinPickup)) {
            OnCoinPickup?.Invoke(this, EventArgs.Empty);
            coinPickup.DestroySelf();
        }
    }

    private void ConsumeFuel(float fuelConsumptionAmount = 1f) {
        fuelAmount -= fuelConsumptionAmount * Time.deltaTime;
    }

    public Vector2 GetSpeed() {
        return landerRigidbody2D.linearVelocity;
    }

    public float GetFuelAmount() {
        return fuelAmount;
    }
}


