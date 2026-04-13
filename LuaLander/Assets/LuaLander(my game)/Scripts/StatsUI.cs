using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsTextMesh;

    private void UpdateStatsTextMesh()
    {
        // Unity puts a GameManager object in the scene
        // Then GameManager's Awake() runs and assings Instance = this - storing a reference to itself in the static property
        // Now any other script can call GameManager.Instance
        statsTextMesh.text = GameManager.Instance.GetScore().ToString() + '\n';
    }
}
