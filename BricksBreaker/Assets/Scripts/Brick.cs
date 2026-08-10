using System;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private int points = 1;

    // public event EventHandler<OnBrickDestroyArgs> OnBrickDestoyed;

    // public class OnBrickDestroyArgs: EventArgs
    // {
    //     public int points;
    // }

    public float GetPoints()
    {
        return points;
    }

    public void DestroySelf()
    {
        // A per-instance event on a brick is awkward because nobody is subscribed at the moment it fires. 
        // Your GameManager would need a reference to every brick, subscribing to each one as it spawns.
        // OnBrickDestoyed.Invoke(this, new OnBrickDestroyArgs{ points = points });
        Destroy(gameObject);
    }
}
