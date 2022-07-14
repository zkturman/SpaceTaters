using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class GeonicComponentBehaviour : MonoBehaviour
{
    [SerializeField]
    private int numberOfSides;
    [SerializeField]
    private Color baseColor;
    public int NumberOfSides
    {
        get => numberOfSides;
    }
    GeonicComponentBehaviour[] neighbouringComponents;
    List<int> availableEdges;
    GeometryTransformer geometryTransformer;

    // Start is called before the first frame update
    void Awake()
    {
        ResetSpriteColor();
    }

    // Update is called once per frame
    void Update()
    {
        //performSpriteAction();
    }

    public void SpawnRootComponent(int numberOfSides)
    {
        this.numberOfSides = numberOfSides;
        neighbouringComponents = new GeonicComponentBehaviour[numberOfSides];
        geometryTransformer = new GeometryTransformer(gameObject);
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
    
    public void CustomiseAvailableEdges(List<int> newEdges)
    {
        availableEdges = newEdges;
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
        Physics.SyncTransforms();
        childBehaviour.UpdateNeighbours(gapSize);
        this.UpdateNeighbours(gapSize);
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
        Vector3 rotationPoint = geometryTransformer.GetCentroid(numberOfSides);
        transform.RotateAround(rotationPoint, Vector3.forward, angleToRotate);
    }

    public bool CanSpawnChild()
    {
        return availableEdges.Count > 0;
    }

    public string GetAvailableEdges()
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < availableEdges.Count; i++)
        {
            builder.Append(availableEdges[i]);
            if (i < availableEdges.Count - 1)
            {
                builder.Append(",");
            }
        }
        return builder.ToString();
    }

    public void ResetSpriteColor()
    {
        SetSpriteColor(baseColor);
    }

    public void SetSpriteColor(Color colorToSet)
    {
        SpriteRenderer componentSprite = GetComponentInChildren<SpriteRenderer>();
        componentSprite.color = colorToSet;
    }

    public void UpdateNeighbours(float gapSize)
    {
        Vector3 centroid = geometryTransformer.GetCentroid(numberOfSides);
        float castDistance = geometryTransformer.GetCentroidHeight(numberOfSides) * 2 + gapSize;
        int[] edgeDataCopy = availableEdges.ToArray(); 
        for (int i = 0; i < edgeDataCopy.Length; i++)
        {
            int edge = edgeDataCopy[i];
            Vector3 castDirection = Quaternion.Euler(0, 0, geometryTransformer.GetShapeRotation(edge, numberOfSides)) * transform.up;
            RaycastHit[] allHits = Physics.RaycastAll(centroid, castDirection, castDistance);
            if (neighboursComponent(allHits))
            {
                GeonicComponentBehaviour neighbourComponent = getNeighbourComponetFromRayCast(allHits);
                neighbouringComponents[edge - 1] = neighbourComponent;
                availableEdges.Remove(edge);
                neighbourComponent.UpdateNeighbours(gapSize);
            }
        }
    }

    private bool neighboursComponent(RaycastHit[] nearbyHits)
    {
        bool hitNeighbour = false;
        for (int i = 0; i < nearbyHits.Length; i++)
        {
            if (nearbyHits[i].collider.gameObject != this)
            {
                hitNeighbour = true;
            }
        }
        return hitNeighbour;
    }

    private GeonicComponentBehaviour getNeighbourComponetFromRayCast(RaycastHit[] nearbyHits)
    {
        GeonicComponentBehaviour componentNeighbour = null;
        for (int i = 0; i < nearbyHits.Length; i++)
        {
            Collider colliderToTest = nearbyHits[i].collider;
            if (colliderToTest.gameObject != this)
            {
                if (colliderToTest.TryGetComponent<GeonicComponentBehaviour>(out GeonicComponentBehaviour testBehaviour))
                {
                    componentNeighbour = testBehaviour;
                }
            }
        }
        return componentNeighbour;
    }
}