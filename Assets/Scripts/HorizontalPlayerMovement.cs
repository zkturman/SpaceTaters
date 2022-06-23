using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalPlayerMovement : IPlayerMovement
{
    public float MovementSpeed { get; set; }
    public float MaximumSpeed { get; set; }
    public float MinimumSpeed { get; set; }
    public float DecelerationSpeed { get; set; }
    public Rigidbody PlayerRigidBody { get; set; }
    public Transform PlayerTransform { get; set; }

    public void ApplyLeftThrusterForce()
    {
        applyThrusterForce(PlayerTransform.right);
    }

    public void ApplyRightThrusterForce()
    {
        Debug.Log("movement direction is " + PlayerTransform.right * -1f);
        applyThrusterForce(PlayerTransform.right * -1f);
    }

    private void applyThrusterForce(Vector3 forceDirection)
    {
        if (playerUnderMaxSpeed(PlayerRigidBody.velocity))
        {
            PlayerRigidBody.AddForce(forceDirection * MovementSpeed);
        }
        else
        {
            PlayerRigidBody.velocity = forceDirection * MaximumSpeed;
        }
    }

    private bool playerUnderMaxSpeed(Vector3 currentSpeed)
    {
        float horizontalVelocity = currentSpeed.x;
        horizontalVelocity = Mathf.Abs(horizontalVelocity);
        return horizontalVelocity < MaximumSpeed;
    }

    public void DecelerateThrusters()
    {
        if (!isMinimumSpeed(PlayerRigidBody.velocity))
        {
            float previousVelocity = PlayerRigidBody.velocity.x;
            Vector3 decelerationAmount = new Vector3(getOpposingAcceleration(), 0f, 0f);
            PlayerRigidBody.velocity += decelerationAmount * Time.deltaTime * DecelerationSpeed;
            if (playerDirectionChanged(PlayerRigidBody.velocity.x, previousVelocity))
            {
                PlayerRigidBody.velocity = Vector3.zero;
                PlayerRigidBody.angularVelocity = Vector3.zero;
            }
        }
    }

    private bool isMinimumSpeed(Vector3 playerVelocity)
    {
        return Mathf.Approximately(playerVelocity.x, MinimumSpeed);
    }
    private float getOpposingAcceleration()
    {
        Vector3 currentVelocity = PlayerRigidBody.velocity;
        float opposingSign = Mathf.Sign(currentVelocity.x) * -1;
        float opposingAcceleration = MovementSpeed * opposingSign;
        return opposingAcceleration;
    }

    private bool playerDirectionChanged(float currentVelocity, float previousVelocity)
    {
        float currentSign = Mathf.Sign(currentVelocity);
        float previousSign = Mathf.Sign(previousVelocity);

        return !Mathf.Approximately(currentSign, previousSign);
    }
}
