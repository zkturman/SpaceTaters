using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeonicComponentMovement : MonoBehaviour
{
    private bool shouldExpand = false;
    public bool ShouldExpand 
    {
        get => shouldExpand; 
        set
        {
            shouldExpand = value;
            startPosition = transform.localPosition;
            endPosition = 1.3f * startPosition + 0.2f * transform.forward;
        }
    }
    private float expandElapsedTime = 0f;
    private float expandDuration = 1f;
    private bool isExpandCycleFinished = true;
    private Vector3 startPosition;
    private Vector3 endPosition;
    public int ID;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        performSpriteAction();
        if (canExpand())
        {
            moveShape();
            Debug.Log("Moved shape");
        }
    }

    private bool canExpand()
    {
        return ShouldExpand && isExpandCycleFinished;
    }

    private void spinShape()
    {
        int diceRoll = Random.Range(0, 2000);
        if (diceRoll == 0)
        {
            Animator componentSpinner = GetComponentInChildren<Animator>();
            componentSpinner.SetTrigger("Rotate");
        }
    }
    private void pushFromCentre()
    {

    }

    private void pullTowardsCentre()
    {

    }

    private void moveShape()
    {
        Vector3 adjustedStartPosition = startPosition;
        Vector3 adjustedEndPosition = endPosition;
        
        if (expandElapsedTime < expandDuration)
        {
            float interpolationPoint = expandElapsedTime / expandDuration;
            Vector3 movement = Vector3.Lerp(adjustedStartPosition, adjustedEndPosition, interpolationPoint);
            transform.localPosition = movement;
            expandElapsedTime += Time.deltaTime;
        }
        else
        {
            transform.localPosition = adjustedEndPosition;
            Vector3 tmp = startPosition;
            startPosition = endPosition;
            endPosition = tmp;
            expandElapsedTime = 0f;
        }
    }

    private void performSpriteAction()
    {
        spinShape();
    }
}
