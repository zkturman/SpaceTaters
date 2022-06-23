using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerInputBehaviour : MonoBehaviour
{
    [SerializeField]
    private float playerAcceleration = 100f;
    [SerializeField]
    private float maxPlayerSpeed = 500f;
    [SerializeField]
    private float minPlayerSpeed = 0f;
    [SerializeField]
    private float decelerationDelay = 0.1f;
    [SerializeField]
    private float playerAngularAcceleration = 5f;
    private Rigidbody playerRigidBody;
    private IPlayerMovement forceHandler;

    private void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        forceHandler = new HorizontalPlayerMovement();
        setupLinearForceHandler(forceHandler);
    }

    private void setupLinearForceHandler(IPlayerMovement linearHandler)
    {
        linearHandler.MovementSpeed = playerAcceleration;
        linearHandler.MaximumSpeed = maxPlayerSpeed;
        linearHandler.MinimumSpeed = minPlayerSpeed;
        linearHandler.PlayerRigidBody = playerRigidBody;
        linearHandler.PlayerTransform = transform;
        linearHandler.DecelerationSpeed = decelerationDelay;
    }

    private void setupAngularForceHandler(IPlayerMovement angularHandler)
    {
        angularHandler.MovementSpeed = playerAngularAcceleration;
        angularHandler.MaximumSpeed = maxPlayerSpeed;
        angularHandler.MinimumSpeed = playerAngularAcceleration;
        angularHandler.PlayerRigidBody = playerRigidBody;
        angularHandler.PlayerTransform = transform;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "OrbitalBoundary")
        {
            Debug.Log("Entered boundary");
            other.GetComponent<OrbitBoundary>().ActivateBoundary();
            forceHandler = new AngularPlayerMovement();
            setupAngularForceHandler(forceHandler);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "OrbitalBoundary")
        {
            Debug.Log("Left boundary");
            forceHandler = new HorizontalPlayerMovement();
            setupLinearForceHandler(forceHandler);
            other.GetComponent<OrbitBoundary>().DeactivateBoundary();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        handleAcceleration();
        handleDeceleration();
    }

    private void handleAcceleration()
    {
        Vector3 forceDirection = Vector3.zero;
        if (isLeftAcceleration())
        {
            forceHandler.ApplyRightThrusterForce();
        }
        else if (isRightAcceleration())
        {
            forceHandler.ApplyLeftThrusterForce();
        }
    }

    private void handleDeceleration()
    {
        Vector3 forceDirection = Vector3.zero;

        if (shouldDecelerate())
        {
            forceHandler.DecelerateThrusters();
        }
    }

    private bool isLeftAcceleration()
    {
        return InputController.IsKeyInGroupPressed(InputController.LeftKeyCodes);
    }

    private bool isRightAcceleration()
    {
        return InputController.IsKeyInGroupPressed(InputController.RightKeyCodes);
    }

    private bool shouldDecelerate()
    {
        return !isRightAcceleration() && !isLeftAcceleration();
    }
}
