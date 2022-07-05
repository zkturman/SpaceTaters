using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMaker : MonoBehaviour
{
    [SerializeField]
    private GameObject[] allMonsters;
    private GameObject monsterToSpawn;
    [SerializeField]
    private int[] xCoordRange = new int[2];
    [SerializeField]
    private int[] yCoordRange = new int[2];
    public bool RandomiseLocation { get; set; }
    public int XCoord { get; set; }
    public int YCoord { get; set; }
    public void SetGeonicPrefab(int shapeId)
    {
        if (shapeId > 0 && shapeId < allMonsters.Length)
        {
            monsterToSpawn = allMonsters[shapeId];
        }
        else
        {
            monsterToSpawn = allMonsters[0];
        }
    }

    private Queue<GameObject> monsterInstances = new Queue<GameObject>();
    public void SpawnMonster()
    {
        GameObject monsterInstance = Instantiate(monsterToSpawn);
        monsterInstance.transform.position = determineSpawnLocation();
        monsterInstances.Enqueue(monsterInstance);
    }

    private Vector3 determineSpawnLocation()
    {
        Vector3 spawnLocation;
        if (RandomiseLocation)
        {
            spawnLocation = randomiseMonsterPosition();
        }
        else
        {
            spawnLocation = new Vector3(XCoord, YCoord, 0);
        }
        return spawnLocation;
    }

    private Vector3 randomiseMonsterPosition()
    {
        int xDiceRoll = Random.Range(xCoordRange[0], xCoordRange[1]);
        int yDiceRoll = Random.Range(yCoordRange[0], yCoordRange[1]);
        return new Vector3(xDiceRoll, yDiceRoll, 0f);
    }

    public void SpawnComponent()
    {
        GameObject monsterInstance = monsterInstances.Peek();
        GeonicBehaviour geonicMonster = monsterInstance.GetComponent<GeonicBehaviour>();
        geonicMonster.SpawnChild();
    }

    public void ClearOldestMonster()
    {
        GameObject monsterInstance = monsterInstances.Dequeue();
        Destroy(monsterInstance);
    }
}
