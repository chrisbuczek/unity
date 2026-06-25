using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Lander : MonoBehaviour
{
    private const float GRAVITY_NORMAL = 0.7f;

    //singleton pattern
    // static - property belongs to a class and not to a class instance itself
    // public get to property, set is private - only this class can change it.
    public static Lander Instance { get; private set; }

    public event EventHandler OnUpForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler OnCoinPickup;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    //explain this custom args with generics
    public event EventHandler<OnLandedEventArgs> OnLanded;
    public class OnLandedEventArgs : EventArgs
    {
        public LandingType landingType;
        public int score;
        public float dotVector;
        public float landingSpeed;
        public float scoreMultiplier;
    }

    public enum LandingType
    {
        Success,
        WrongLandingArea,
        TooSteepAngle,
        TooFastLanding
    }

    //state machine
    public enum State
    {
        WaitingToStart,
        Normal,
        GameOver
    }

    private Rigidbody2D landerRigidbody2D;
    private float fuelAmountMax = 10f;
    private float fuelAmount;
    private State state;

    private void Awake()
    {
        Instance = this;
        landerRigidbody2D = GetComponent<Rigidbody2D>(); //get reference to Rigidbody2D from the same game object
        landerRigidbody2D.gravityScale = 0f;
        fuelAmount = fuelAmountMax;
        state = State.WaitingToStart;
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

        switch (state)
        {
            case State.WaitingToStart:
                // if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.leftArrowKey.isPressed || Keyboard.current.upArrowKey.isPressed)
                if (GameInput.Instance.IsUpActionPressed() || GameInput.Instance.IsLeftActionPressed() || GameInput.Instance.IsRightActionPressed())
                {
                    //pressing any input
                    landerRigidbody2D.gravityScale = GRAVITY_NORMAL;
                    SetState(State.Normal);
                }
                break;
            case State.Normal:
                if (fuelAmount <= 0f)
                {
                    return;
                }

                // if (Keyboard.current.upArrowKey.isPressed)
                if (GameInput.Instance.IsUpActionPressed())
                {
                    // if (Keyboard.current.leftArrowKey.isPressed && Keyboard.current.rightArrowKey.isPressed)
                    if (GameInput.Instance.IsLeftActionPressed() && GameInput.Instance.IsRightActionPressed())
                    {
                        float ultraForce = 1400f;
                        landerRigidbody2D.AddForce(ultraForce * transform.up * Time.fixedDeltaTime);
                        ConsumeFuel(1.5f);
                    }
                    else
                    {
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
                // if (Keyboard.current.rightArrowKey.isPressed)
                if (GameInput.Instance.IsRightActionPressed())
                {
                    float turnSpeed = -100f; //never use magic numbers. Always assign a value to variable with proper name.
                    landerRigidbody2D.AddTorque(turnSpeed * Time.fixedDeltaTime);
                    OnRightForce?.Invoke(this, EventArgs.Empty);
                    ConsumeFuel();
                }
                // if (Keyboard.current.leftArrowKey.isPressed)
                if (GameInput.Instance.IsLeftActionPressed())
                {
                    float turnSpeed = +100f;
                    landerRigidbody2D.AddTorque(turnSpeed * Time.fixedDeltaTime);
                    OnLeftForce?.Invoke(this, EventArgs.Empty);
                    ConsumeFuel();
                }
                break;
            case State.GameOver:
                break;
            default:
                return;

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
        if (!collision2D.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            Debug.Log("Crashed on Terrain!");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                score = 0,
                landingType = LandingType.WrongLandingArea,
                dotVector = 0f,
                landingSpeed = 0f,
                scoreMultiplier = 0
            });
            SetState(State.GameOver);
        }

        //magnitude of vector 2d is just his size. collision2d.relativeVelocity takes x axis and y axis of velocity vector
        float softLandingVelocityMagnitude = 4f;
        float relativeVelocityMagintude = collision2D.relativeVelocity.magnitude;
        if (relativeVelocityMagintude > softLandingVelocityMagnitude)
        {
            Debug.Log("Landed too hard!");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                score = 0,
                landingType = LandingType.TooFastLanding,
                dotVector = 0f,
                landingSpeed = relativeVelocityMagintude,
                scoreMultiplier = 0
            });
            SetState(State.GameOver);
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
        if (dotVector < minDotVector)
        {
            Debug.Log("Landed on a too steep angle!");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                score = 0,
                landingType = LandingType.TooSteepAngle,
                dotVector = dotVector,
                landingSpeed = relativeVelocityMagintude,
                scoreMultiplier = 0
            });
            SetState(State.GameOver);
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
        OnLanded?.Invoke(this, new OnLandedEventArgs
        {
            score = score,
            landingType = LandingType.Success,
            dotVector = dotVector,
            landingSpeed = relativeVelocityMagintude,
            scoreMultiplier = landingPad.GetScoreMultiplier()
        });
        SetState(State.GameOver);
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.TryGetComponent(out FuelPickup fuelPickup))
        {
            float pickedUpFuel = fuelPickup.GetFuel();
            fuelAmount += pickedUpFuel;
            if (fuelAmount > fuelAmountMax)
            {
                fuelAmount = fuelAmountMax;
            }
            fuelPickup.DestroySelf();
        }

        if (collider2D.gameObject.TryGetComponent(out CoinPickup coinPickup))
        {
            OnCoinPickup?.Invoke(this, EventArgs.Empty);
            coinPickup.DestroySelf();
        }
    }

    private void SetState(State state)
    {
        this.state = state;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            state = state
        });
    }

    private void ConsumeFuel(float fuelConsumptionAmount = 1f)
    {
        fuelAmount -= fuelConsumptionAmount * Time.deltaTime;
    }

    public Vector2 GetSpeed()
    {
        return landerRigidbody2D.linearVelocity;
    }

    public float GetFuelAmount()
    {
        return fuelAmount;
    }

    public float GetFuelAmountNormalized()
    {
        return fuelAmount / fuelAmountMax;
    }
}


