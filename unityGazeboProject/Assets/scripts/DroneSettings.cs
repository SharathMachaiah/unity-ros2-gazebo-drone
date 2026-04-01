using UnityEngine;

public class DroneSettings : MonoBehaviour
{
    public static DroneSettings Instance { get; private set; }

    [Header("Current Flight Values")]
    public float forwardSpeed = 2.0f;
    public float verticalSpeed = 2.0f;
    public float yawSpeed = 2.0f;

    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
