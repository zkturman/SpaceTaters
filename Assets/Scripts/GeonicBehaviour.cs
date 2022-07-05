using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeonicBehaviour : MonoBehaviour
{
    [SerializeField]
    private int numberOfSides;
    private GeonicComponentBehaviour rootComponent;
    [SerializeField]
    protected GameObject componentParentObject;
    [SerializeField]
    protected GameObject componentPrefab;
    [SerializeField]
    protected float spawnDelay = 1f;
    [SerializeField]
    protected float componentGap = 0.15f;
    private bool canSpawn = false;

    [SerializeField]
    protected bool automaticSpawning = true;
    private float health;
    protected List<GameObject> availableComponents;

    // Start is called before the first frame update
    void Start()
    {
        availableComponents = new List<GameObject>();
        GameObject rootObject = Instantiate(componentPrefab, componentParentObject.transform);
        rootComponent = rootObject.GetComponent<GeonicComponentBehaviour>();
        availableComponents.Add(rootObject);
        rootComponent.SpawnRootComponent(numberOfSides);
        StartCoroutine(spawnChildRoutine());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canSpawn && automaticSpawning)
        {
            StartCoroutine(spawnChildRoutine());
        }
    }

    private IEnumerator spawnChildRoutine()
    {
        canSpawn = false;
        yield return new WaitForSeconds(spawnDelay);
        SpawnChild();
    }

    public virtual void SpawnChild()
    {
        resetAvailableComponentsColor();
        GeonicComponentBehaviour spawningComponent = determineSpawningComponent(); 
        spawnNewChild(spawningComponent);
        updateAvailableComponents();
        canSpawn = true;
    }

    private void resetAvailableComponentsColor()
    {
        foreach (GameObject child in availableComponents)
        {
            child.GetComponent<GeonicComponentBehaviour>().ResetSpriteColor();
        }
    }

    private GeonicComponentBehaviour determineSpawningComponent()
    {
        GeonicComponentBehaviour spawningComponent = null;
        if (availableComponents.Count > 0)
        {
            int diceRoll = Random.Range(0, availableComponents.Count);
            GameObject spawningObject = availableComponents[diceRoll];
            spawningComponent = spawningObject.GetComponent<GeonicComponentBehaviour>();
            spawningComponent.SetSpriteColor(Color.yellow);
        }
        return spawningComponent;
    }

    protected virtual void spawnNewChild(GeonicComponentBehaviour parentComponent)
    {
        GameObject childObject = parentComponent.SpawnChild(componentPrefab, componentGap);
        childObject.transform.SetParent(componentParentObject.transform);
        childObject.GetComponent<GeonicComponentBehaviour>().SetSpriteColor(Color.green);
        availableComponents.Add(childObject);
    }

    protected virtual void updateAvailableComponents()
    {
        List<GameObject> newAvailableComponents = new List<GameObject>();
        for (int i = 0; i < availableComponents.Count; i++)
        {
            GameObject spawningObject = availableComponents[i];
            GeonicComponentBehaviour spawningComponent = spawningObject.GetComponent<GeonicComponentBehaviour>();
            if (spawningComponent.CanSpawnChild())
            {
                newAvailableComponents.Add(spawningObject); 
            }
            else
            {
                spawningComponent.SetSpriteColor(Color.red);
            }
        }
        availableComponents = newAvailableComponents;
    }
}
