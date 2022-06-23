using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitDefinitions : MonoBehaviour
{
    [SerializeField]
    private OrbitBoundary leftBoundary;
    [SerializeField]
    private OrbitBoundary rightBoundary;
    [SerializeField]
    [Range(2f, 5f)]
    private float orbitRadius = 2f;

    public float OrbitRadius 
    { 
        get => orbitRadius; 
        private set => orbitRadius = value; 
    }
 
    //public Vector3 CurveCenter { get; private set; }

    public Vector3 GetOrbitCenter()
    {
        Vector3 orbitCenter = Vector3.zero;
        if (leftBoundary.IsActive)
        {
            orbitCenter = leftBoundary.GetBoundaryRotationPoint(orbitRadius);
        }
        else if (rightBoundary.IsActive)
        {
            orbitCenter = rightBoundary.GetBoundaryRotationPoint(orbitRadius);
        }
        return orbitCenter;
    }

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
