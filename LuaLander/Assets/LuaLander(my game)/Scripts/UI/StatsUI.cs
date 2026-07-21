using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private GameObject speedUpArrowGameObject;
    [SerializeField] private GameObject speedDownArrowGameObject;
    [SerializeField] private GameObject speedLeftArrowGameObject;
    [SerializeField] private GameObject speedRightArrowGameObject;
    [SerializeField] private Image fuelBar;

    private void Update()
    {
        UpdateStatsTextMesh();
    }

    private void UpdateStatsTextMesh()
    {
        // if(Mathf.Round(Lander.Instance.GetSpeed().x) > 0)
        // {
        //     speedRightArrowGameObject.SetActive(true);
        //     speedLeftArrowGameObject.SetActive(false);
        // } else
        // {
        //     speedRightArrowGameObject.SetActive(false);
        //     speedLeftArrowGameObject.SetActive(true);
        // }

        // if(Mathf.Round(Lander.Instance.GetSpeed().y) > 0)
        // {
        //     speedUpArrowGameObject.SetActive(true);
        //     speedDownArrowGameObject.SetActive(false);
        // } else
        // {
        //     speedUpArrowGameObject.SetActive(false);
        //     speedDownArrowGameObject.SetActive(true);
        // }

        // if(Mathf.Round(Lander.Instance.GetSpeed().x) == 0) {
        //     speedRightArrowGameObject.SetActive(false);
        //     speedLeftArrowGameObject.SetActive(false);
        // }

        // if(Mathf.Round(Lander.Instance.GetSpeed().y) == 0) { 
        //     speedUpArrowGameObject.SetActive(false);
        //     speedDownArrowGameObject.SetActive(false);
        // }
        // all of above can be replaced with just 4 lines
        speedUpArrowGameObject.SetActive(Mathf.Round(Lander.Instance.GetSpeed().y) > 0);
        speedDownArrowGameObject.SetActive(Mathf.Round(Lander.Instance.GetSpeed().y) < 0);
        speedLeftArrowGameObject.SetActive(Mathf.Round(Lander.Instance.GetSpeed().x) < 0);
        speedRightArrowGameObject.SetActive(Mathf.Round(Lander.Instance.GetSpeed().x) > 0);

        // Unity puts a GameManager object in the scene
        // Then GameManager's Awake() runs and assings Instance = this - storing a reference to itself in the static property
        // Now any other script can call GameManager.Instance
        statsTextMesh.text = GameManager.Instance.GetLevelNumber().ToString() + '\n'
        + GameManager.Instance.GetScore().ToString() + '\n'
        + Mathf.Round(GameManager.Instance.GetTime()).ToString() + '\n'
        + Mathf.Round(Lander.Instance.GetSpeed().x * 10f) + '\n'
        + Mathf.Round(Lander.Instance.GetSpeed().y * 10f) + '\n'
        + Lander.Instance.GetFuelAmount().ToString("F1");

        // fuelBar.fillAmount = Lander.Instance.GetFuelAmount() / Lander.Instance.GetFuelAmountMax(); i did it before like this
        fuelBar.fillAmount = Lander.Instance.GetFuelAmountNormalized();
    }


}
