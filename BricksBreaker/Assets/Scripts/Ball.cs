using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector2 startDirection = new Vector2(1f, 1f); //45 degree angle

    private Rigidbody2D ballRigidBody;
    private Vector2 velocityBeforeCollision;

    private void Awake()
    {
        ballRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        ballRigidBody.linearVelocity = startDirection.normalized * moveSpeed;
        velocityBeforeCollision = ballRigidBody.linearVelocity;
    }

    private void FixedUpdate()
    {
        // Runs before the physics step, so this is the velocity the ball hits the wall with.
        velocityBeforeCollision = ballRigidBody.linearVelocity;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        // No contact points means no surface normal to reflect off, so there is nothing to do.
        // Happens with trigger-like overlaps and collisions the solver already pushed apart.
        if (other.contactCount == 0) return;

        // Average the contacts so corners give a sane normal.
        Vector2 normal = Vector2.zero;
        for (int i = 0; i < other.contactCount; i++)
            normal += other.GetContact(i).normal;
        normal.Normalize();

        Vector2 incoming = velocityBeforeCollision.normalized;
        Vector2 reflected = Vector2.Reflect(incoming, normal);

        ballRigidBody.linearVelocity = reflected * moveSpeed;
    }
}
