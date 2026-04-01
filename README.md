# unity-ros2-gazebo-drone
 Unity project for controlling Gazebo X3 drone
Unity + ROS2 + Gazebo Drone Simulation
Overview

This project demonstrates a real-time drone simulation system where:

Unity → visualization & user input
ROS2 → communication layer
Gazebo → physics simulation

The drone is simulated in Gazebo, controlled from Unity, and data flows through ROS2.

## Project Structure
```text
drone_project/
├── unityGazeboProject/          # Unity project folder
├── ros2_ws/        # ROS2 workspace in Ubuntu
│   ├── src/
│   │   └── height_reader/  # Custom ROS2 node
│   ├── build/      
│   ├── install/   
│   └── log/        
├── gazebo/         # Gazebo simulation assets in Ubuntu
│   └── worlds/
│       └── Quadworld.sdf
└── scripts/
    └── fly.sh      # Script to launch entire system in Ubuntu

```

## Prerequisites

Make sure you have:

Ubuntu (version 24 recommended)  
ROS2 (Jazzy)  
Gazebo (Harmonic)  
Unity (unity6 recommended)  

## Build Instructions (Required First Time Only)  
Before running the project for the first time, you must build the ROS2 workspace.  
cd ros2_ws  
source /opt/ros/jazzy/setup.bash  
colcon build  
source install/setup.bash

## Launch ROS bridge and required Gazebo world (during every session)
./scripts/fly.sh
