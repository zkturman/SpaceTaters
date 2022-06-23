using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeonicComponentBehaviour : MonoBehaviour
{
    [SerializeField]
    private int numberOfSides;
    GeonicComponentBehaviour[] neighbouringComponents;
    List<int> availableEdges;
    GeometryTransformer geometryTransformer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnRootComponent(int numberOfSides)
    {
        this.numberOfSides = numberOfSides;
        neighbouringComponents = new GeonicComponentBehaviour[numberOfSides];
        availableEdges = new List<int>();
        for (int i = 1; i <= neighbouringComponents.Length; i++)
        {
            availableEdges.Add(i);
        }
    }

    public void SpawnComponent(int numberOfSides, GeonicComponentBehaviour parentComponent)
    {
        this.numberOfSides = numberOfSides;
        neighbouringComponents = new GeonicComponentBehaviour[numberOfSides];
        int parentEdge = getEdgeConnectingToParent();
        neighbouringComponents[parentEdge - 1] = parentComponent;
        geometryTransformer = new GeometryTransformer(gameObject);
        availableEdges = new List<int>();
        for (int i = 1; i <= neighbouringComponents.Length; i++)
        {
            if (i != parentEdge)
            {
                availableEdges.Add(i);
            }
        }
    }

    private int getEdgeConnectingToParent()
    {
        return numberOfSides / 2 + numberOfSides % 2;
    }

    public GameObject SpawnChild(GameObject prefab, float gapSize)
    {
        int diceRoll = Random.Range(0, availableEdges.Count);
        GameObject childComponent = Instantiate(prefab, transform.parent);
        childComponent.transform.rotation = transform.rotation;
        childComponent.transform.position = transform.position;
        GeonicComponentBehaviour childBehaviour = childComponent.GetComponent<GeonicComponentBehaviour>();
        childBehaviour.SpawnComponent(numberOfSides, this);
        childBehaviour.RotateToEdge(availableEdges[diceRoll]);
        childBehaviour.TranslateToEdge(gapSize);
        neighbouringComponents[availableEdges[diceRoll] - 1] = childBehaviour;
        availableEdges.RemoveAt(diceRoll);
        return childComponent;
    }

    public void TranslateToEdge(float gapSize)
    {
        float translateDistance = geometryTransformer.GetCentroidHeight(numberOfSides) * 2f + gapSize;
        transform.Translate(Vector3.up * translateDistance, Space.Self);
    }

    public void RotateToEdge(int edgeNumber)
    {
        float angleToRotate = geometryTransformer.GetShapeRotation(edgeNumber, numberOfSides);
        Vector3 rotationPoint = transform.rotation * geometryTransformer.GetCentroid(numberOfSides) + transform.position;
        transform.RotateAround(rotationPoint, Vector3.forward, angleToRotate);
    }

    public bool CanSpawnChild()
    {
        return availableEdges.Count > 0;
    }

    public void SetSpriteColor(Color colorToSet)
    {
        SpriteRenderer componentSprite = GetComponent<SpriteRenderer>();
        componentSprite.color = colorToSet;
    }

    private void OnDrawGizmosSelected()
    {
        GeometryTransformer transformer = new GeometryTransformer(gameObject);
        Gizmos.color = Color.cyan;
        Vector3 centroid = transformer.GetCentroid(numberOfSides);
        Vector3 untranslatedCentroid = transform.rotation * centroid;
        Gizmos.DrawLine(untranslatedCentroid, centroid);
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(untranslatedCentroid, 0.1f);
        Gizmos.color = Color.yellow;
        Vector3 componentCentroid = untranslatedCentroid + transform.position;
        Gizmos.DrawSphere(componentCentroid, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(componentCentroid, Quaternion.Euler(0, 0, 60) * transform.up * 0.5f);
        Gizmos.DrawRay(componentCentroid, Quaternion.Euler(0, 0, 180) * transform.up * 0.5f);
        Gizmos.DrawRay(componentCentroid, Quaternion.Euler(0, 0, 300) * transform.up * 0.5f);


    }
}