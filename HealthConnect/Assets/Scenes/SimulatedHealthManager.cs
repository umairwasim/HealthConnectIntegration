using UnityEngine;
using TMPro;
using DG.Tweening;  // For Tweening

public class SimulatedHealthManager : MonoBehaviour
{
    public TextMeshProUGUI stepsText;
    public TextMeshProUGUI heartRateText;
    public TextMeshProUGUI sleepText;
    public RectTransform stepsIcon;
    public RectTransform heartRateIcon;
    public RectTransform sleepIcon;

    private int steps;
    private int heartRate;
    private float sleepHours;

    void Start()
    {
        // Initialize with random data
        GenerateSimulatedData();
        UpdateUI();

        // Apply animations on start
        AnimateUI();
    }

    void GenerateSimulatedData()
    {
        steps = Random.Range(3000, 12000);  // Randomized steps count
        heartRate = Random.Range(60, 100);  // Random heart rate
        sleepHours = Random.Range(4.5f, 9f);  // Random sleep hours
    }

    void UpdateUI()
    {
        stepsText.text = $"{steps} STEPS";
        heartRateText.text = $"Heart Rate: {heartRate} bpm";
        sleepText.text = $"Sleep: {sleepHours:F1} hrs";
    }

    void AnimateUI()
    {
        // Simple tweening animations for text and icons
        stepsIcon.DOScale(1.2f, 0.5f).SetEase(Ease.OutBounce);  // Icon for steps scales up
        heartRateIcon.DOScale(1.2f, 0.5f).SetEase(Ease.OutBounce);  // Icon for heart rate scales up
        sleepIcon.DOScale(1.2f, 0.5f).SetEase(Ease.OutBounce);  // Icon for sleep scales up

        stepsText.DOFade(1f, 0.5f).From(0f);  // Fade in text for steps
        heartRateText.DOFade(1f, 0.5f).From(0f);  // Fade in text for heart rate
        sleepText.DOFade(1f, 0.5f).From(0f);  // Fade in text for sleep
    }
}
