using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    [SerializeField] public Transform[] WayPoints = null;

    private void Awake()
    {
        Debug.Assert(WayPoints != null && WayPoints.Length > 0, "_wayPoints are not assigned!");
    }
}
