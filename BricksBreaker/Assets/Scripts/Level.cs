using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private int bricksRemaining;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bricksRemaining = GetComponentsInChildren<Brick>().Length;  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleOnBrickDestroyed()
    {
        bricksRemaining--;
        if(bricksRemaining <= 0)
        {
            LevelManager.Instance.LoadNextLevel();
        }
    }
}
