#include <rclcpp/rclcpp.hpp>
#include <std_msgs/msg/float32.hpp>

#include <gz/transport/Node.hh>
#include <gz/msgs/pose_v.pb.h>

class HeightNode : public rclcpp::Node
{
public:
  HeightNode() : Node("height_node")
  {
    pub_ = this->create_publisher<std_msgs::msg::Float32>(
        "/drone_height", 10);

    gz_node_.Subscribe(
        "/world/quadcopter_teleop/dynamic_pose/info",
        &HeightNode::cb,
        this);
  }

private:
  void cb(const gz::msgs::Pose_V &msg)
  {
    for (int i = 0; i < msg.pose_size(); i++)
    {
      auto p = msg.pose(i);

      if (p.name() == "X3")
      {
        std_msgs::msg::Float32 m;
        m.data = p.position().z();

        pub_->publish(m);
      }
    }
  }

  gz::transport::Node gz_node_;
  rclcpp::Publisher<std_msgs::msg::Float32>::SharedPtr pub_;
};

int main(int argc, char **argv)
{
  rclcpp::init(argc, argv);
  rclcpp::spin(std::make_shared<HeightNode>());
  rclcpp::shutdown();
  return 0;
}
