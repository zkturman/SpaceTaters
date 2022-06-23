using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeonicBehaviour : MonoBehaviour
{
    [SerializeField]
    private int numberOfSides;
    private GeonicComponentBehaviour rootComponent;
    [SerializeField]
    private GameObject componentPrefab;
    [SerializeField]
    private float spawnDelay = 1f;
    [SerializeField]
    private float componentGap = 0.15f;
    private bool canSpawn = false;

    [SerializeField]
    private bool automaticSpawning = true;
    private float health;
    private List<GameObject> availableComponents;

    // Start is called before the first frame update
    void Start()
    {
        availableComponents = new List<GameObject>();
        GameObject rootObject = Instantiate(componentPrefab);
        rootComponent = rootObject.GetComponent<GeonicComponentBehaviour>();
        availableComponents.Add(rootObject);
        rootComponent.SpawnRootComponent(numberOfSides);
        StartCoroutine(spawnChildRoutine());
    }

    // Update is called once per frame
    void Update()
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
        Debug.Log("spawning child 8D");
        SpawnChild();
    }

    public void SpawnChild()
    {
        foreach(GameObject child in availableComponents)
        {
            child.GetComponent<GeonicComponentBehaviour>().SetSpriteColor(Color.white);
        }
        int diceRoll = Random.Range(0, availableComponents.Count);
        GameObject spawningObject = availableComponents[diceRoll];
        GeonicComponentBehaviour spawningComponent = spawningObject.GetComponent<GeonicComponentBehaviour>();
        spawningComponent.SetSpriteColor(Color.yellow);
        GameObject childObject = spawningComponent.SpawnChild(componentPrefab, componentGap);
        childObject.GetComponent<GeonicComponentBehaviour>().SetSpriteColor(Color.green);
        availableComponents.Add(childObject);
        if (!spawningComponent.CanSpawnChild())
        {
            spawningComponent.SetSpriteColor(Color.red);
            availableComponents.RemoveAt(diceRoll);
        }
        canSpawn = true;
    }
}
