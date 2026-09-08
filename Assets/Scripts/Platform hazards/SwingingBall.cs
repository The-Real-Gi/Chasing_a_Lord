using UnityEngine;

public class SwingingBall : MonoBehaviour
{
    [SerializeField] float swingAngle = 90f;
    [SerializeField] float motorSpeed = 90f;
    [SerializeField] float maxMotorTorque = 1000f;

    HingeJoint2D hinge;
    float swingDirection = 1f;

    void Awake()
    {
        hinge = GetComponent<HingeJoint2D>();

        JointAngleLimits2D limits = hinge.limits;
        limits.min = -swingAngle;
        limits.max = swingAngle;
        hinge.limits = limits;
        hinge.useLimits = true;

        hinge.useMotor = true;
        SetMotorSpeed();
    }

    void FixedUpdate()
    {
        if (hinge.jointAngle >= swingAngle - 1f)
        {
            swingDirection = -1f;
            SetMotorSpeed();
        }
        else if (hinge.jointAngle <= -swingAngle + 1f)
        {
            swingDirection = 1f;
            SetMotorSpeed();
        }
    }

    void SetMotorSpeed()
    {
        JointMotor2D motor = hinge.motor;
        motor.motorSpeed = swingDirection * motorSpeed;
        motor.maxMotorTorque = maxMotorTorque;
        hinge.motor = motor;
    }

}
