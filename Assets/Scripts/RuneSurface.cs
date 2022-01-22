using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneSurface : MonoBehaviour
{
    [SerializeField] private Transform _runeSpot = null;
    [SerializeField] private GameObject _runePrefab = null;

    private void Awake()
    {
        Debug.Assert(_runeSpot != null, "_runeSpot is not assigned!");
    }

    public void SpawnRune()
    {
        Instantiate(_runePrefab, _runeSpot.transform.position, Quaternion.identity);
    }
}
