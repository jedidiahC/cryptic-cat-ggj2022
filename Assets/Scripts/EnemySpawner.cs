using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab = null;
    [SerializeField] private EnemyPath[] _enemyPaths = null;
    [SerializeField] private Transform[] _spawnPoints = null;
    [SerializeField] private Enemy[] _tutorialEnemies = null;

    [SerializeField] private float _spawnInterval = 5.0f;
    private float _timer = 0;
    private bool _tutorialEnded = false;

    private void Awake()
    {
        Debug.Assert(_enemyPrefab != null, "_enemyPrefab is not assigned!");
        Debug.Assert(_enemyPaths != null && _enemyPaths.Length > 0, "_enemyPaths are not assigned!");
        Debug.Assert(_spawnPoints != null && _spawnPoints.Length > 0, "_spawnPoints are not assigned!");
    }

    void Start()
    {
        _timer = _spawnInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_tutorialEnded)
        {
            foreach (var enemy in _tutorialEnemies)
            {
                if (!enemy.IsDead) { return; }
            }
            _tutorialEnded = true;
        }

        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            Vector3 spawnPt = _spawnPoints[Random.Range(0, _spawnPoints.Length)].position;
            EnemyPath enemyPath = _enemyPaths[Random.Range(0, _enemyPaths.Length)];

            Enemy newEnemy = Instantiate<Enemy>(_enemyPrefab, spawnPt, Quaternion.identity);
            string[] strings = { "SELF-DOUBT", "FEAR", "I CAN'T DO THIS", "RUMINATION", 
                "I'M GONNA LOSE MY JOB", "NOTHING EVER WORKS OUT" , 
                ""};
            newEnemy.Init(strings[Random.Range(0, strings.Length)], RuneWords.Belief, enemyPath);

            _timer = _spawnInterval;
        }
    }
}
