using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeonicFormationPulse : IGeonicFormation
{
    public Transform ComponentTransform { get; set; }
    public float Duration
    {
        get => duration;
        set => duration = value;
    }
    private float duration = 2f;
    private float elapsedTime = 0f;
    private float expandElapsedTime = 0f;
    private float expandDuration = 1f;

    private Vector3 startPosition;
    private Vector3 endPosition;
    public int ComponentId
    {
        get => componentId;
        private set => componentId = value;
    }
    private int componentId;

    public GeonicFormationPulse(Transform componentTransform, int componentId)
    {
        startPosition = transform.localPosition;
        endPosition = 1.3f * startPosition + 0.2f * transform.forward;
        this.componentId = componentId;
    }


    public bool IsDone()
    {
        return elapsedTime > duration;
    }

    public void MoveComponent()
    {
        Vector3 adjustedStartPosition = startPosition;
        Vector3 adjustedEndPosition = endPosition;

        if (expandElapsedTime < expandDuration)
        {
            float interpolationPoint = expandElapsedTime / expandDuration;
            Vector3 movement = Vector3.Lerp(startPosition, endPosition, interpolationPoint);
            transform.localPosition = movement;
            expandElapsedTime += Time.deltaTime;
            elapsedTime += Time.deltaTime;
        }
        else
        {
            transform.localPosition = endPosition;
            Vector3 tmp = startPosition;
            startPosition = endPosition;
            endPosition = tmp;
            expandElapsedTime = 0f;
        }
    }
}
