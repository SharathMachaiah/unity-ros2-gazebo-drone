
#!/bin/bash

# Source ROS
source /opt/ros/jazzy/setup.bash
source ~/drone_project/ros2_ws/install/setup.bash

echo "Cleaning up old ports..."
sudo fuser -k 10000/tcp > /dev/null 2>&1

# Get IP
MY_IP=$(hostname -I | awk '{print $1}')
echo "------------------------------------------"
echo "Current IP: $MY_IP"
echo "------------------------------------------"

# Set Gazebo model path
export GAZEBO_MODEL_PATH=$GAZEBO_MODEL_PATH:~/drone_project/gazebo/models

echo "Starting ROS <-> Gazebo bridge..."
ros2 run ros_gz_bridge parameter_bridge \
X3/cmd_vel@geometry_msgs/msg/Twist@gz.msgs.Twist \
X3/odometry@nav_msgs/msg/Odometry@gz.msgs.Odometry &

sleep 2

echo "Starting ROS TCP Endpoint (Unity)..."
ros2 run ros_tcp_endpoint default_server_endpoint --ros-args -p ROS_IP:=0.0.0.0 &

sleep 2

echo "Starting Height Reader..."
ros2 run height_reader height_reader &

sleep 2

echo "Launching Gazebo..."
gz sim ~/drone_project/gazebo/worlds/Quadworld.sdf
