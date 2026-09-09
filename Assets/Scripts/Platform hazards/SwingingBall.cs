using UnityEngine;

public class SwingingBall : MonoBehaviour
{
    [SerializeField] float minimumSpeed = 10f;
    [SerializeField] float maximumSpeed = 180f;
    [SerializeField] float maximumMotorTorque = 1000f;

    HingeJoint2D hinge;
    float swingDirection = 1f;

    void Awake()
    {
        hinge = GetComponent<HingeJoint2D>();
        hinge.useMotor = true;
        UpdateMotor();
    }

    void FixedUpdate()
    {
        float angle = hinge.jointAngle;
        float lowerLimit = hinge.limits.min;
        float upperLimit = hinge.limits.max;

        if (angle >= upperLimit - 1f)
        {
            swingDirection = -1f;
        }
        else if (angle <= lowerLimit + 1f)
        {
            swingDirection = 1f;
        }

        UpdateMotor();
    }

    void UpdateMotor()
    {
        float lowerLimit = hinge.limits.min;
        float upperLimit = hinge.limits.max;
        float normalizedAngle = Mathf.InverseLerp(lowerLimit, upperLimit, hinge.jointAngle);
        float speedCurve = Mathf.Sin(normalizedAngle * Mathf.PI);
        float speed = Mathf.Lerp(minimumSpeed, maximumSpeed, speedCurve);

        JointMotor2D motor = hinge.motor;
        motor.motorSpeed = swingDirection * speed;
        motor.maxMotorTorque = maximumMotorTorque;
        hinge.motor = motor;
    }
}
