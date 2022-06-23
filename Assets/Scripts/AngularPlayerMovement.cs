using UnityEngine;

public class AngularPlayerMovement : IPlayerMovement
{
    public float MovementSpeed { get; set; }
    public float MaximumSpeed { get; set; }
    public float MinimumSpeed { get; set; }
    public float DecelerationSpeed { get; set; }
    public Rigidbody PlayerRigidBody { get; set; }

    private Transform playerTransform;
    public Transform PlayerTransform 
    { 
        get => playerTransform; 
        set
        {
            playerTransform = value;
            playerStartingRotation = playerTransform.rotation.eulerAngles;
        } 
    }

    private OrbitDefinitions orbitDefinitions;
    private Vector3 curvePivot;
    private Vector3 playerStartingRotation;

    public AngularPlayerMovement()
    {
        orbitDefinitions = MonoBehaviour.FindObjectOfType<OrbitDefinitions>();
        curvePivot = orbitDefinitions.GetOrbitCenter();
    }

    public void ApplyLeftThrusterForce()
    {
        applyThrusterForce(true, PlayerTransform.right);
    }

    public void ApplyRightThrusterForce()
    {
        applyThrusterForce(false, PlayerTransform.right * -1);
    }

    private void applyThrusterForce(bool isClockwiseRotation, Vector3 exitSpeedDirection)
    {
        if (!hasReachedEndRotation(isClockwiseRotation))
        {
            PlayerRigidBody.velocity = Vector3.zero;
            PlayerRigidBody.angularVelocity = Vector3.zero;
            Quaternion playerOrbitRotation = Quaternion.AngleAxis(determineMovementSpeed(isClockwiseRotation), Vector3.up);
            PlayerRigidBody.MovePosition(playerOrbitRotation * (PlayerTransform.position - curvePivot) + curvePivot);
            PlayerRigidBody.MoveRotation(PlayerTransform.rotation * playerOrbitRotation);
        }
        else
        {
            PlayerRigidBody.AddForce(MaximumSpeed * exitSpeedDirection, ForceMode.Acceleration);
        }
    }

    private float determineMovementSpeed(bool isClockwiseRotation)
    {
        float rotationSpeed = MovementSpeed;
        if (!isClockwiseRotation)
        {
            rotationSpeed *= -1;
        }
        return rotationSpeed;
    }

    private float determineEndRotation(bool isClockwiseRotation)
    {
        float endRotation;
        bool isPlayerInTopLevel = Mathf.Approximately(playerStartingRotation.y, 0);
        if (isClockwiseRotation)
        {
            if (curvePivot.x > 0)
            {
                endRotation = 180f;
            }
            else
            {
                endRotation = 0f;
            }
        }
        else
        {
            if (curvePivot.x > 0)
            {
                endRotation = 0f;

            }
            else
            {
                endRotation = 180f;
            }
        }
        return endRotation;
    }

    private bool hasReachedEndRotation(bool isClockwiseRotation)
    {
        float playerYRotation = Mathf.Round(PlayerTransform.rotation.eulerAngles.y);
        return Mathf.Approximately(playerYRotation, determineEndRotation(isClockwiseRotation));
    }

    public void DecelerateThrusters()
    {
        PlayerRigidBody.velocity = Vector3.zero;
        PlayerRigidBody.angularVelocity = Vector3.zero;
    }
}
