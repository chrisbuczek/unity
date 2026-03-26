using UnityEngine;

public class LandingPad : MonoBehaviour
{
    // SerializeField expose variable in the editor
    [SerializeField] private int scoreMultiplier;   

    public int GetScoreMultiplier()
    {
        return scoreMultiplier;
    }
}
