using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerMovement
{
    public float MovementSpeed { get; set; }
    public float MaximumSpeed { get; set; }
    public float MinimumSpeed { get; set; }
    public float DecelerationSpeed { get; set; }
    public Rigidbody PlayerRigidBody { get; set; }
    public Transform PlayerTransform { get; set; }
    public void ApplyLeftThrusterForce();
    public void ApplyRightThrusterForce();
    public void DecelerateThrusters();
}
