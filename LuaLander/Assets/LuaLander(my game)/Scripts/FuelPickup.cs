using UnityEngine;

public class FuelPickup : MonoBehaviour
{
    //In CircleCollider2D remember to check Is Trigger property so it doesn't have physical collision box and can be passed through
    [SerializeField] private float fuel = 10f;

    public float GetFuel()
    {
        return fuel;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
