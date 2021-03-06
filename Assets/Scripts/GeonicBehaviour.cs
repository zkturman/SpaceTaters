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
    [SerializeField]
    protected int maxComponents = 30;
    protected int numberOfComponents = 0;
    private bool canSpawn = false;

    [SerializeField]
    protected bool automaticSpawning = true;
    [SerializeField]
    protected Color newComponentColor = Color.green;
    [SerializeField]
    protected Color spawningComponentColor = Color.yellow;
    [SerializeField]
    protected Color completeComponentColor = Color.red;
    private float health;
    protected List<GameObject> availableComponents;
    protected List<GameObject> allComponents;
    private bool inExpandMode = false;

    // Start is called before the first frame update
    void Start()
    {
        availableComponents = new List<GameObject>();
        allComponents = new List<GameObject>();
        GameObject rootObject = Instantiate(componentPrefab, componentParentObject.transform);
        rootComponent = rootObject.GetComponent<GeonicComponentBehaviour>();
        availableComponents.Add(rootObject);
        rootComponent.SpawnRootComponent(numberOfSides);
        StartCoroutine(spawnChildRoutine());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (inSpawningMode())
        {
            StartCoroutine(spawnChildRoutine());
        }

        if (!underMaximumComponents() && !inExpandMode)
        {
            enterExpandMode();
        }
    }

    private void enterExpandMode()
    {
        inExpandMode = true;
        for (int i = 0; i < allComponents.Count; i++)
        {
            GeonicComponentMovement component = allComponents[i].GetComponent<GeonicComponentMovement>();
            component.ShouldExpand = true;
            component.ID = i;
        }
        GetComponent<GeonicMoveController>().CanMove = true;
    }

    private bool inSpawningMode()
    {
        bool underMaximum = underMaximumComponents();
        return canSpawn && automaticSpawning && underMaximum;
    }

    protected virtual bool underMaximumComponents()
    {
        return numberOfComponents < maxComponents;
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
            spawningComponent.SetSpriteColor(spawningComponentColor);
        }
        return spawningComponent;
    }

    protected virtual void spawnNewChild(GeonicComponentBehaviour parentComponent)
    {
        GameObject childObject = parentComponent.SpawnChild(componentPrefab, componentGap);
        childObject.transform.SetParent(componentParentObject.transform);
        childObject.GetComponent<GeonicComponentBehaviour>().SetSpriteColor(newComponentColor);
        availableComponents.Add(childObject);
        allComponents.Add(childObject);
        numberOfComponents++;
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
                spawningComponent.SetSpriteColor(completeComponentColor);
            }
        }
        availableComponents = newAvailableComponents;
    }
}
