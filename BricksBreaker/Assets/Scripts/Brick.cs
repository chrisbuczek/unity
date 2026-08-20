using System;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private int points = 1;

    public int GetPoints()
    {
        return points;
    }

    public void DestroySelf()
    {
        GetComponentInParent<Level>()?.HandleOnBrickDestroyed();
        Destroy(gameObject);
    }
}
