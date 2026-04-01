using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;
using RosMessageTypes.Nav;
using RosMessageTypes.Std;
using UnityEngine.InputSystem;
using TMPro;

public class X3DroneController : MonoBehaviour
{
    [Header("ROS Topics")]
    public string cmdTopic = "/X3/cmd_vel";
    public string odomTopic = "/X3/odometry";
    public string heightTopic = "/drone_height";


    private ROSConnection ros;
    private double lastPublishTime;
    public float publishFrequency = 0.05f;

    float currentHeight = 0;

    [Header("UI Reference")]
     public TMP_Text heightText;
    public TMP_Text speedStatusText;

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();

        ros.RegisterPublisher<TwistMsg>(cmdTopic);

        ros.Subscribe<OdometryMsg>(odomTopic, UpdateDronePosition);
        ros.Subscribe<Float32Msg>(heightTopic, UpdateHeight);
    }

    void Update()
    {
        float forward = 0;
        float lateral = 0;
        float upward = 0;
        float yaw = 0;
       // float pitch = 0;
       // float roll = 0;


        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) forward = 1;
            if (Keyboard.current.sKey.isPressed) forward = -1;
            if (Keyboard.current.aKey.isPressed) lateral = 1;
            if (Keyboard.current.dKey.isPressed) lateral = -1;


         
            if (Keyboard.current.upArrowKey.isPressed) upward = 1;
            if (Keyboard.current.downArrowKey.isPressed) upward = -1;
            if (Keyboard.current.leftArrowKey.isPressed) yaw = 1;
            if (Keyboard.current.rightArrowKey.isPressed) yaw = -1;

            
           // if (Keyboard.current.iKey.isPressed) pitch = 1;
           // if (Keyboard.current.kKey.isPressed) pitch = -1;
            // if (Keyboard.current.jKey.isPressed) roll = 1;  //looks like X3 dosnt support angular inputs for these
            // if (Keyboard.current.lKey.isPressed) roll = -1;
        }
        if (Time.time - lastPublishTime > publishFrequency)
        {
            PublishCommand(forward, lateral, upward, yaw);
            lastPublishTime = Time.time;
        }
    }

    void PublishCommand(float forward, float lateral, float upward, float yaw)
    {
        TwistMsg twist = new TwistMsg();

        // Linear movement 
        twist.linear.x = forward * DroneSettings.Instance.forwardSpeed;
        twist.linear.y = lateral * DroneSettings.Instance.forwardSpeed;
        twist.linear.z = upward * DroneSettings.Instance.verticalSpeed;

        // Angular movement
        twist.angular.z = yaw * DroneSettings.Instance.yawSpeed;
        // twist.angular.x = roll * tiltSpeed;    //looks like X3 dosnt support angular inputs for these
        //  twist.angular.y = pitch * tiltSpeed;   

        ros.Publish(cmdTopic, twist);
    }

    void UpdateHeight(Float32Msg msg)
    {
        currentHeight = msg.data;
        if (heightText != null)
        {
            heightText.text = currentHeight.ToString("F2");
        }
        //    Debug.Log("Received height: " + currentHeight);
    }

    void UpdateDronePosition(OdometryMsg odom)
    {
        var rosPos = odom.pose.pose.position;

        // XY from odom, Z from height topic
        transform.position = new Vector3(-(float)rosPos.y,currentHeight,(float)rosPos.x);

        var rosRot = odom.pose.pose.orientation;

        transform.rotation = new Quaternion(-(float)rosRot.y,(float)rosRot.z,(float)rosRot.x,-(float)rosRot.w);

        UpdateSpeedUI(odom.twist.twist);
    }
    private void UpdateSpeedUI(RosMessageTypes.Geometry.TwistMsg twist)
    {
        if (speedStatusText == null) return;

        // Get the three components of linear velocity
        double vx = twist.linear.x;
        double vy = twist.linear.y;
     

        // Calculate the magnitude (Total Speed in m/s)
        float speed = (float)System.Math.Sqrt(vx * vx + vy * vy );

        // Update the UI 
        speedStatusText.text = speed.ToString("F2");
    }
  
}
