using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeonicComponentGizmo : MonoBehaviour
{
    private GeonicComponentBehaviour geonicComponent;
    private GeometryTransformer geometryTransformer;
    private int numberOfSides;

    private void OnDrawGizmos()
    {
        geonicComponent = GetComponent<GeonicComponentBehaviour>();
        geometryTransformer = new GeometryTransformer(gameObject);
        numberOfSides = geonicComponent.NumberOfSides;
    }

    private void OnDrawGizmosSelected()
    {
        drawComponentCentroid();
        drawRaysToNeighbour();
        logAvailableEdges();
    }

    private void drawComponentCentroid()
    {
        Gizmos.color = Color.cyan;
        Vector3 centroid = geometryTransformer.GetCentroid(numberOfSides);
        Gizmos.DrawSphere(centroid, 0.1f);
    }

    private void drawRaysToNeighbour()
    {
        Gizmos.color = Color.cyan;
        Vector3 centroid = geometryTransformer.GetCentroid(numberOfSides);
        for (int i = 1; i <= numberOfSides; i++)
        {
            float zRoation = geometryTransformer.GetShapeRotation(i, numberOfSides);
            Quaternion rotationToEdge = Quaternion.Euler(0, 0, geometryTransformer.GetShapeRotation(i, numberOfSides));
            Vector3 rayDirection = rotationToEdge * transform.up;
            float rayMagnitude = geometryTransformer.GetCentroidHeight(numberOfSides) * 2 + .15f;
            Gizmos.DrawRay(centroid, rayDirection * rayMagnitude);
            Gizmos.color = Color.red;
        }
    }

    private void logAvailableEdges()
    {
        if (geonicComponent.CanSpawnChild())
        {
            Debug.Log(geonicComponent.GetAvailableEdges());
        }
    }
}
