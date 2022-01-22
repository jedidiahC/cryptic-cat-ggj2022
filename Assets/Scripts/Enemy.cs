using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private RuneWords _counterType;
    [SerializeField] private string _word = "death";
    [SerializeField] private GameObject _segmentPrefab = null;
    [SerializeField] private EnemyMovement _enemyMovement = null;

    private void Awake()
    {
        Debug.Assert(_segmentPrefab != null, "_segmentPrefab is not assigned!");
        Debug.Assert(_enemyMovement != null, "_enemyMovement is not assigned!");
    }

    private void Start()
    {
        Init();
    }

    private void Init() 
    {
        SpawnSegments();
    }

    private void SpawnSegments() 
    {
        Transform[] segments = new Transform[_word.Length];

        for (int c = 0; c < _word.Length; c++)
        {
            GameObject newSegGO = Instantiate(_segmentPrefab, transform.position, Quaternion.identity);
            Letter letter = newSegGO.GetComponent<Letter>();
            letter.SetText(_word[_word.Length - c - 1].ToString());
            segments[c] = newSegGO.transform;
            newSegGO.transform.SetParent(this.transform);
        }

        _enemyMovement.SetSegments(segments);
    }

    public void Damage(float amount, RuneWords counterType)
    {
        if (_counterType == counterType)
        {
            // Neat Destruction animation.
        }
    }
}
