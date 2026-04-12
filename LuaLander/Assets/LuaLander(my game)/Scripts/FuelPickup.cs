using UnityEngine;

public class FuelPickup : MonoBehaviour
{
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
