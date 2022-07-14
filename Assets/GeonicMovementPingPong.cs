using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeonicMovementPingPong : IGeonicMovement
{
    public Transform GeonicTransform { get; set; }
    private Vector3 initialPosition;
    public float Duration 
    { 
        get => duration;
        set => duration = value; 
    }
    private float periodDuration = 3f;
    private float duration = 7.5f;
    private float elapsedTime = 0f;
    private float periodElapsedTime = 0f;

    public float EndPosition
    {
        get => endPosition;
        set => endPosition = value;
    }
    private float endPosition = -10f;

    public float StartPosition
    {
        get => startPosition;
        set => startPosition = value;
    }
    private float startPosition = 10f;


    public GeonicMovementPingPong(Transform geonicTransform)
    {
        GeonicTransform = geonicTransform;
        initialPosition = new Vector3(geonicTransform.position.x, geonicTransform.position.y, geonicTransform.position.z);
        elapsedTime = simulateElapsedTime();
        periodElapsedTime = elapsedTime;
    }

    private float simulateElapsedTime()
    {
        float totalDistance = startPosition - endPosition;
        float currentDistance = startPosition - GeonicTransform.position.x;
        float traveledPortion = currentDistance / totalDistance;
        return traveledPortion * periodDuration;
    }

    public bool IsDone()
    {
        return elapsedTime > duration;
    }

    public void MoveGeonic()
    {
        Vector3 startValue = new Vector3(startPosition, GeonicTransform.position.y, 0f);
        Vector3 endValue = new Vector3(endPosition, GeonicTransform.position.y, 0f);
        if (periodElapsedTime < periodDuration)
        {
            float interpolationValue = periodElapsedTime / periodDuration;
            Vector3 frameDestination = Vector3.Lerp(startValue, endValue, interpolationValue);
            GeonicTransform.position = frameDestination;
            periodElapsedTime += Time.deltaTime;
            elapsedTime += Time.deltaTime;
        }
        else
        {
            GeonicTransform.position = endValue;
            periodElapsedTime = 0f;
            float tmp = startPosition;
            startPosition = endPosition;
            endPosition = tmp;
        }
        if (elapsedTime > duration)
        {
            GeonicTransform.position = initialPosition;
        }
    }
}
