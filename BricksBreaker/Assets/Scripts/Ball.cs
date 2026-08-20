using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ball : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private Vector2 startDirection = new Vector2(1f, 1f); //45 degree angle
    [SerializeField] private Vector3 startPosition = new Vector3(0, 0, 0);

    private Rigidbody2D ballRigidBody;
    private Vector2 velocityBeforeCollision;

    public event EventHandler OnDeathTriggerEntered;
    public event EventHandler<BrickDestroyedEventArgs> OnBrickDestroyed;

    private State state;

    private enum State {
        WaitingToStart,
        Playing, 
    }


    public class BrickDestroyedEventArgs : EventArgs
    {
        public int points;
    }

    private void Awake()
    {
        ballRigidBody = GetComponent<Rigidbody2D>();
        transform.position = startPosition;
    }

    private void Start()
    {
        ballRigidBody.linearVelocity = startDirection.normalized * moveSpeed;
        velocityBeforeCollision = ballRigidBody.linearVelocity;
        state = State.WaitingToStart;
        ballRigidBody.simulated = false;
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(object sender, GameManager.GameStateChangedEvent e)
    {
        if(e.currentGameState == GameManager.GameState.Playing)
        {
            state = State.Playing;
            // Launch once, on the transition. simulated must be true first, because velocity
            // assigned to a non-simulated body is discarded.
            ballRigidBody.simulated = true;
            ballRigidBody.linearVelocity = startDirection.normalized * moveSpeed;
        }
        if(e.currentGameState == GameManager.GameState.WaitingToStart) {
            Reset();
        }
    }

    private void FixedUpdate()
    {
        switch(state){
            case State.Playing:
                ballRigidBody.simulated = true;
                break;
            case State.WaitingToStart:
                ballRigidBody.simulated = false;
                transform.position = startPosition;
                ballRigidBody.linearVelocity = Vector2.zero;
                velocityBeforeCollision = startDirection.normalized * moveSpeed;
                break;
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
            Reset();
        }       
        
    }

    public void Reset()
    {
        state = State.WaitingToStart;
        gameObject.transform.position = startPosition;
    }
}
