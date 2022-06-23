using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitBoundary : MonoBehaviour
{
    public bool IsActive { get; private set; }
    
    public void ActivateBoundary()
    {
        IsActive = true;
    }
    
    public void DeactivateBoundary()
    {
        IsActive = false;
    }

    public Vector3 GetBoundaryRotationPoint(float orbitRadius)
    {
        BoxCollider orbitArcArea = GetComponent<BoxCollider>();
        float centerXCoord;
        if (orbitArcArea.bounds.center.x > 0)
        {
            centerXCoord = orbitArcArea.bounds.min.x;
        }
        else
        {
            centerXCoord = orbitArcArea.bounds.max.x;
        }
        return new Vector3(centerXCoord, 1, -orbitRadius);
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
