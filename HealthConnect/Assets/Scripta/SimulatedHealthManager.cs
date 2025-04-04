using UnityEngine;
using UnityEngine.UI;

public class SimulatedHealthManager : MonoBehaviour
{
    public Text stepCountText;
    public Button simulateButton;

    private int simulatedStepCount = 0;

    private void Start()
    {
        simulateButton.onClick.AddListener(SimulateStepIncrease);
        UpdateStepDisplay();
    }

    private void SimulateStepIncrease()
    {
        simulatedStepCount += Random.Range(200, 1000); // Simulate steps
        UpdateStepDisplay();
    }

    private void UpdateStepDisplay()
    {
        stepCountText.text = $"Steps Today: {simulatedStepCount}";
    }
}
