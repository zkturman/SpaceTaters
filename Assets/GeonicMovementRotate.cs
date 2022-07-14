using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeonicMovementRotate : IGeonicMovement
{
    public float RotationSpeedInSeconds
    {
        get => rotationSpeedInSeconds;
        set => rotationSpeedInSeconds = value;
    }
    private float rotationSpeedInSeconds = 90;
    
    public float Duration 
    { 
        get => duration; 
        set => duration = value; 
    }
    private float duration = 4f;
    private float elapsedTime = 0f;
    public Transform GeonicTransform { get; set; }

    public GeonicMovementRotate(Transform geonicTransform)
    { 
        GeonicTransform = geonicTransform;
    }

    public void MoveGeonic()
    {
        rotateGeonic();
    }

    private void rotateGeonic()
    {
        Vector3 rotationVelocity = Vector3.forward * RotationSpeedInSeconds;
        Vector3 frameRotation = rotationVelocity * Time.deltaTime;
        GeonicTransform.Rotate(frameRotation);
        elapsedTime += Time.deltaTime;
    }

    public bool IsDone()
    {
        return elapsedTime >= duration;
    }
}
