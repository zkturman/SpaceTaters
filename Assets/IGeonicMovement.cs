using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGeonicMovement
{
    public Transform GeonicTransform { get; set; }
    public float Duration { get; set; }
    public void MoveGeonic();
    public bool IsDone();
}
