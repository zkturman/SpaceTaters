using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeonicMoveController : MonoBehaviour
{
    public bool CanMove = false;
    [SerializeField]
    private float minRotation = -75f;
    [SerializeField]
    private float maxRotation = 75f;
    [SerializeField]
    private float spinDuration = 2f;
    [SerializeField]
    private MovementType[] movementSequence;
    private Queue<MovementType> movementOrder = new Queue<MovementType>();
    public IGeonicMovement movementPattern;
    private MovementTypeLocator movementFinder;
    // Start is called before the first frame update
    void Start()
    {
        movementOrder = new Queue<MovementType>(movementSequence);
        movementFinder = new MovementTypeLocator(transform);
        MovementType startMovement = movementOrder.Peek();
        movementPattern = movementFinder.FindMovement(startMovement);
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove)
        {
            movementPattern.MoveGeonic();
            checkForNextMovement();
        }
    }

    private void checkForNextMovement()
    {
        if (movementPattern.IsDone())
        {
            MovementType lastMovement = movementOrder.Dequeue();
            movementOrder.Enqueue(lastMovement);
            MovementType nextMovement = movementOrder.Peek();
            movementPattern = movementFinder.FindMovement(nextMovement);
        }
    }
}
