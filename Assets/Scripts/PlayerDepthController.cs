using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDepthController : MonoBehaviour
{
    [SerializeField]
    private OrbitDefinitions orbitDefintions;
    [SerializeField]
    private ThrusterBehaviour thrusterBehaviour;
    private float maxDepth;
    private float minScaling = 0.5f;

    private void Awake()
    {
        maxDepth = orbitDefintions.OrbitRadius * -2;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float scaleFactor = getDepthScaling();
        setObjectScaling(scaleFactor);
    }

    private float getDepthScaling()
    {
        float maxDepthFraction = transform.position.z / maxDepth;
        float nextScaleFraction = maxDepthFraction * minScaling;
        return 1 - nextScaleFraction;
    }

    private void setObjectScaling(float scaleFactor)
    {
        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        thrusterBehaviour.SetThrusterScale(scaleFactor);
    }
}
