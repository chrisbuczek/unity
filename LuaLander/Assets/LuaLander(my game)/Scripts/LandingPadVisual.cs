using TMPro;
using UnityEngine;

public class LandingPadVisual : MonoBehaviour
{
    [SerializeField] private TextMeshPro scoreMultipierTextMesh;

    //awake function
    private void Awake()
    {
        LandingPad landingPad = GetComponent<LandingPad>();
        // you can drag and drop TextMeshPro inside editor instead of line below.
        // scoreMultipierTextMesh = GetComponentInChildren<TextMeshPro>();
        scoreMultipierTextMesh.text = "x" + landingPad.GetScoreMultiplier().ToString();
    }
}
