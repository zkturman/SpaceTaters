using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentaGeonicBehaviour : GeonicBehaviour
{
    [SerializeField] List<int> customGenerationEdges = new List<int>();

    protected override void spawnNewChild(GeonicComponentBehaviour parentComponent)
    {
        if (parentComponent != null)
        {
            GameObject childObject = parentComponent.SpawnChild(componentPrefab, componentGap);
            childObject.transform.SetParent(componentParentObject.transform);
            GeonicComponentBehaviour childComponent = childObject.GetComponent<GeonicComponentBehaviour>();
            List<int> customEdges = new List<int>(customGenerationEdges);
            childComponent.CustomiseAvailableEdges(customEdges);
            childComponent.SetSpriteColor(Color.green);
            availableComponents.Add(childObject);
        }
    }

    protected override void updateAvailableComponents()
    {
        List<GameObject> newAvailableComponents = new List<GameObject>();
        for (int i = 0; i < availableComponents.Count; i++)
        {
            GameObject spawningObject = availableComponents[i];
            GeonicComponentBehaviour spawningComponent = spawningObject.GetComponent<GeonicComponentBehaviour>();
            spawningComponent.UpdateNeighbours(componentGap);
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
