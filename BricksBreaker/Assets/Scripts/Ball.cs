using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ball : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private Vector2 startDirection = new Vector2(1f, 1f); //45 degree angle

    private Rigidbody2D ballRigidBody;
    private Vector2 velocityBeforeCollision;

    public event EventHandler OnDeathTriggerEntered;
    public event EventHandler<BrickDestroyedEventArgs> OnBrickDestroyed;

    private State state;

    private enum State {
        WaitingToStart,
        Normal, 
        GameOver
    }

    public class BrickDestroyedEventArgs : EventArgs
    {
        public int points;
    }

    private void Awake()
    {
        ballRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        ballRigidBody.linearVelocity = startDirection.normalized * moveSpeed;
        velocityBeforeCollision = ballRigidBody.linearVelocity;
        state = State.WaitingToStart;
        ballRigidBody.simulated = false;
    }

    private void FixedUpdate()
    {
        switch(state){
            case State.Normal:
                ballRigidBody.simulated = true;
                break;
            case State.WaitingToStart | State.GameOver:
                ballRigidBody.simulated = false;
                break;
        }

        if(Keyboard.current.leftArrowKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            state = State.Normal;
        }

        // Runs before the physics step, so this is the velocity the ball hits the wall with.
        velocityBeforeCollision = ballRigidBody.linearVelocity;

    }


    private void OnCollisionEnter2D(Collision2D collider2D) {
        // No contact points means no surface normal to reflect off, so there is nothing to do.
        // Happens with trigger-like overlaps and collisions the solver already pushed apart.
        if (collider2D.contactCount == 0) return;

        // Use the primary contact normal. Averaging multiple normals at corners produces
        // a synthetic direction that doesn't align with either surface, breaking reflection.
        Vector2 normal = collider2D.GetContact(0).normal;

        Vector2 incoming = velocityBeforeCollision.normalized;
        Vector2 reflected = Vector2.Reflect(incoming, normal);

        ballRigidBody.linearVelocity = reflected * moveSpeed;

        if(collider2D.gameObject.TryGetComponent(out Brick brick)) {
            OnBrickDestroyed?.Invoke(this, new BrickDestroyedEventArgs { points = brick.GetPoints() });
            brick.DestroySelf();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.gameObject.TryGetComponent(out DeathTriggerZone deathTriggerZone)) {
            OnDeathTriggerEntered.Invoke(this, EventArgs.Empty);
            state = State.GameOver;
        }       
        
    }
}
