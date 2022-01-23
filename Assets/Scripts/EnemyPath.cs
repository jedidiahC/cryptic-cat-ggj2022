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

    private void OnDrawGizmos()
    {
        for (int i = 1; i < WayPoints.Length; i++)
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawLine(WayPoints[i - 1].position, WayPoints[i].position);
        }
    }
}