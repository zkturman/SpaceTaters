using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeometryTransformer
{
    private GameObject geometricShape;
    public GeometryTransformer(GameObject geometricShape)
    {
        this.geometricShape = geometricShape;
    }
    public Vector3 GetCentroid(int numSides)
    {
        if (isEven(numSides))
        {
            return geometricShape.transform.position;
        }
        else
        {
            Vector3 baseCentroid = Vector3.zero;
            Vector3 currentCentroid = geometricShape.transform.rotation * baseCentroid + geometricShape.transform.position;
            return currentCentroid;
        }
    }

    public float GetCentroidHeight(int numSides)
    {

        if (isEven(numSides))
        {
            return Mathf.Abs((geometricShape.transform.lossyScale.y) / 2f);
        }
        else
        {
            float bisectedCentroidAngle = findCentroidAngle(numSides) / 2f;
            float angleInRadians = bisectedCentroidAngle * Mathf.PI / 180f;
            float centroidHeight = Mathf.Abs(geometricShape.transform.lossyScale.x) / (2 * Mathf.Tan(angleInRadians));
            return centroidHeight;
        }
    }

    public float GetShapeRotation(int edgeNumber, int numSides)
    {
        float angleBetweenEdges = findCentroidAngle(numSides);
        if (isEven(numSides))
        {
            return edgeNumber * (180f - angleBetweenEdges);
        }
        else
        {
            return (edgeNumber - 1) * (angleBetweenEdges) + angleBetweenEdges / 2;
        }
    }

    private float findCentroidAngle(int numberOfSides)
    {
        return 180f - ((numberOfSides - 2f) * 180f / numberOfSides);
    }

    private bool isEven(int numSides)
    {
        return (numSides % 2 == 0);
    }
}
