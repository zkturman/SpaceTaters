using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMaker : MonoBehaviour
{
    [SerializeField]
    private GameObject monsterToSpawn;
    [SerializeField]
    private int[] xCoordRange = new int[2];
    [SerializeField]
    private int[] yCoordRange = new int[2];

    private GameObject monsterInstance;
    public void SpawnMonster()
    {
        if (monsterInstance != null)
        {
            Destroy(monsterInstance);
        }
        monsterInstance = Instantiate(monsterToSpawn);
        monsterInstance.transform.position = randomiseMonsterPosition();
    }

    private Vector3 randomiseMonsterPosition()
    {
        int xDiceRoll = Random.Range(xCoordRange[0], xCoordRange[1]);
        int yDiceRoll = Random.Range(yCoordRange[0], yCoordRange[1]);
        return new Vector3(xDiceRoll, yDiceRoll, 0f);
    }

    public void SpawnComponent()
    {
        GeonicBehaviour geonicMonster = monsterInstance.GetComponent<GeonicBehaviour>();
        geonicMonster.SpawnChild();
    }
}
