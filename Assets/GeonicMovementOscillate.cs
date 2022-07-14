using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeonicMovementOscillate : IGeonicMovement
{
    public float MinRotation 
    { 
        get => minRotation; 
        set => minRotation = value; 
    } 
    private float minRotation = -75f;
    
    public float MaxRotation
    {
        get => maxRotation;
        set => maxRotation = value;
    }
    private float maxRotation = 75f;

    public float Duration
    {
        get => duration;
        set => duration = value;
    }
    private float duration = 5f;
    private float elapsedTime = 0f;

    private float spinDuration = 2f;

    private float spinElapsedTime = 0f;
    public Transform GeonicTransform { get; set; }

    public GeonicMovementOscillate(Transform geonicTransform)
    {
        GeonicTransform = geonicTransform;
        elapsedTime = getTimeFromStartRotation();
        spinElapsedTime = elapsedTime;
    }

    private float getTimeFromStartRotation()
    {
        float totalRotation = maxRotation - minRotation;
        float offsetRotation = totalRotation - maxRotation;
        float currentOffset = GeonicTransform.localEulerAngles.z + offsetRotation;
        float simulatedElapsedTime = (currentOffset / totalRotation) * spinDuration;
        return simulatedElapsedTime;
    }

    public void MoveGeonic()
    {
        oscillateGeonic();
    }

    private void oscillateGeonic()
    {
        if (spinElapsedTime < spinDuration)
        {
            Quaternion startRotation = Quaternion.Euler(new Vector3(0, 0, MinRotation));
            Quaternion endRotation = Quaternion.Euler(new Vector3(0, 0, maxRotation));
            float spinInterpolation = spinElapsedTime / spinDuration;
            Quaternion newRotation = Quaternion.Slerp(startRotation, endRotation, spinInterpolation);
            GeonicTransform.localRotation = newRotation;
            spinElapsedTime += Time.deltaTime;
            elapsedTime += Time.deltaTime;
        }
        else
        {
            GeonicTransform.rotation = Quaternion.Euler(new Vector3(0, 0, maxRotation));
            float tmp = MinRotation;
            MinRotation = maxRotation;
            maxRotation = tmp;
            spinElapsedTime = 0f;
        }
        if (elapsedTime >= duration)
        {
            GeonicTransform.rotation = Quaternion.identity;
        }
    }

    public bool IsDone()
    {
        return elapsedTime >= Duration;
    }
}
