using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTypeLocator
{
    private Transform objectTransform;
    public MovementTypeLocator(Transform objectTransform)
    {
        this.objectTransform = objectTransform;
    }

    public IGeonicMovement FindMovement(MovementType typeOfMovement)
    {
        IGeonicMovement locatedMovement;
        switch (typeOfMovement)
        {
            case MovementType.Oscillate:
                locatedMovement = new GeonicMovementOscillate(objectTransform);
                break;
            case MovementType.PingPong:
                locatedMovement = new GeonicMovementPingPong(objectTransform);
                break;
            case MovementType.Rotate:
            default:
                locatedMovement = new GeonicMovementRotate(objectTransform);
                break;
        }
        return locatedMovement;
    }

}
