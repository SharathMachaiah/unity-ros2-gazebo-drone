using UnityEngine;
using TMPro; 
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private TMP_Text fpsStatusText; 

    [Header("FPS Configuration")]
    private readonly int[] fpsOptions = { 30, 60, 120, -1 };
    private int currentFpsIndex = 1; // Default index 1 is 60 FPS

    void Start()
    {
        
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        // Default to 60 FPS
        ApplyFrameRate();
    }

    // Toggle UI Panel visibility
    public void ToggleSettingsPanel()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }
    }

    // Cycle FPS Up
    public void IncreaseFPS()
    {
        currentFpsIndex = (currentFpsIndex + 1) % fpsOptions.Length;
        ApplyFrameRate();
    }

    // Cycle FPS Down
    public void DecreaseFPS()
    {
        currentFpsIndex--;
        if (currentFpsIndex < 0) currentFpsIndex = fpsOptions.Length - 1;
        ApplyFrameRate();
    }

    private void ApplyFrameRate()
    {
        int target = fpsOptions[currentFpsIndex];
        Application.targetFrameRate = target;

        // Update UI Text field
        if (fpsStatusText != null)
        {
            string displayVal = target == -1 ? "Unlimited" : target.ToString() + " FPS";
            fpsStatusText.text = displayVal;
        }
    }
    public void QuitApplication()
    {
       
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Debug.Log("Application has quit.");
    }
}