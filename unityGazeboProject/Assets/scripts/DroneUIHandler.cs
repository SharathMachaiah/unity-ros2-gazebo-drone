using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DroneUIHandler : MonoBehaviour
{
    [Header("Horizontal Speed UI")]
    public Slider forwardSlider;
    public TMP_Text forwardValueText;

    [Header("Vertical Speed UI")]
    public Slider verticalSlider;
    public TMP_Text verticalValueText;

    [Header("Yaw Speed UI")]
    public Slider yawSlider;
    public TMP_Text yawValueText;

    void Start()
    {
        // Initialize Slider current values
        forwardSlider.value = DroneSettings.Instance.forwardSpeed;
        verticalSlider.value = DroneSettings.Instance.verticalSpeed;
        yawSlider.value = DroneSettings.Instance.yawSpeed;

        // Listeners
        forwardSlider.onValueChanged.AddListener(OnForwardSliderChanged);
        verticalSlider.onValueChanged.AddListener(OnVerticalSliderChanged);
        yawSlider.onValueChanged.AddListener(OnYawSliderChanged);

        UpdateLabels();
    }

    void OnForwardSliderChanged(float value)
    {
        DroneSettings.Instance.forwardSpeed = value;
        UpdateLabels();
    }

    void OnVerticalSliderChanged(float value)
    {
        DroneSettings.Instance.verticalSpeed = value;
        UpdateLabels();
    }
    void OnYawSliderChanged(float value)
    {
        DroneSettings.Instance.yawSpeed = value;
        UpdateLabels();
    }

    void UpdateLabels()
    {
        forwardValueText.text = $"{DroneSettings.Instance.forwardSpeed:F1} m/s";
        verticalValueText.text = $"{DroneSettings.Instance.verticalSpeed:F1} m/s";
        yawValueText.text = $"{DroneSettings.Instance.yawSpeed:F1} m/s";
    }
}
