using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using TMPro;

public class RuntimeRosSettings : MonoBehaviour
{
    public TMP_InputField ipInputField;
    public TMP_InputField portInputField;

    private string fullIP; // Stores the actual IP

    void Awake()
    {
        // Load IP
        fullIP = PlayerPrefs.GetString("SavedRosIP", "000.00.00.000");
        int savedPort = PlayerPrefs.GetInt("SavedRosPort", 10000);

        // Display masked version
        ipInputField.text = MaskIP(fullIP);
        portInputField.text = savedPort.ToString();

        // Apply Ip to ROS
        var ros = ROSConnection.GetOrCreateInstance();
        ros.RosIPAddress = fullIP;
        ros.RosPort = savedPort;
    }

    public void UpdateConnectionSettings()
    {
        var ros = ROSConnection.GetOrCreateInstance();

        // Check if the user typed a new IP or kept the masked one
        string input = ipInputField.text;
        if (!input.Contains("*"))
        {
            fullIP = input; // User typed IP
        }

        if (int.TryParse(portInputField.text, out int newPort))
        {
            ros.Disconnect();
            ros.RosIPAddress = fullIP;
            ros.RosPort = newPort;
            ros.Connect();

            PlayerPrefs.SetString("SavedRosIP", fullIP);
            PlayerPrefs.SetInt("SavedRosPort", newPort);
            PlayerPrefs.Save();

            // Refresh UI and show masked version
            ipInputField.text = MaskIP(fullIP);

            Debug.Log($"Connected to masked IP ending in: {fullIP.Substring(Mathf.Max(0, fullIP.Length - 3))}");
        }
    }

    // method to mask the string
    private string MaskIP(string ip)
    {
        if (string.IsNullOrEmpty(ip) || ip.Length <= 3) return ip;

        string maskedPart = new string('*', ip.Length - 3);
        string visiblePart = ip.Substring(ip.Length - 3);

        return maskedPart + visiblePart;
    }
}