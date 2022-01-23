using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    public UnityEvent OnBreakStart;
    public UnityEvent OnBreakEnd;

    [SerializeField] private Enemy _enemyPrefab = null;
    [SerializeField] private EnemyPath[] _enemyPaths = null;
    [SerializeField] private Transform[] _spawnPoints = null;
    [SerializeField] private Enemy[] _tutorialEnemies = null;

    [SerializeField] private float _spawnInterval = 5.0f;
    [SerializeField] private float _spawnDecrement = 0.05f;
    [SerializeField] private float _breakTime = 30f;
    [SerializeField] private float _difficulty0Time = 60f;

    [Space]
    [SerializeField] private float _gameTimer = 0;
    [SerializeField] private float _spawnTimer = 0;
    [SerializeField] private int _difficulty = 0;
    [SerializeField] private bool _tutorialEnded = false;
    [SerializeField] private bool _inBreak = false;

    private void Awake()
    {
        Debug.Assert(_enemyPrefab != null, "_enemyPrefab is not assigned!");
        Debug.Assert(_enemyPaths != null && _enemyPaths.Length > 0, "_enemyPaths are not assigned!");
        Debug.Assert(_spawnPoints != null && _spawnPoints.Length > 0, "_spawnPoints are not assigned!");
    }

    void Start()
    {
        _spawnTimer = _spawnInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_tutorialEnded)
        {
            bool enemyDown = false;

            for (int i = 0; i < _tutorialEnemies.Length; i++)
            {
                enemyDown = _tutorialEnemies[i] == null || _tutorialEnemies[i].IsDead;

                if (enemyDown)
                {
                    break;
                }
            }
            if (!enemyDown) { return; }

            for (int i = 0; i < _tutorialEnemies.Length; i++)
            {
                if (_tutorialEnemies[i] == null || _tutorialEnemies[i].IsDead) { continue; }
                _tutorialEnemies[i].OnDeath();
            }

            _tutorialEnded = true;
            _gameTimer = _difficulty0Time;
            _inBreak = false;
        }

        _spawnTimer -= Time.deltaTime;
        _gameTimer -= Time.deltaTime;

        if (_spawnTimer <= 0 && !_inBreak)
        {
            Vector3 spawnPt = _spawnPoints[Random.Range(0, _spawnPoints.Length)].position;
            EnemyPath enemyPath = _enemyPaths[Random.Range(0, _enemyPaths.Length)];

            Enemy newEnemy = Instantiate<Enemy>(_enemyPrefab, spawnPt, Quaternion.identity);
            string[] strings = {  "FEAR",  "REGRET",  "FAILURE", "PAIN", "ANXIETY",
            "SELF-DOUBT","I CAN'T DO THIS", "RUMINATION", "DEPRESSION",
            "THIS IS THE END", "NOTHING EVER WORKS OUT", "WORLD IS ENDING", "THINGS ARE ONLY GETTING WORSE", "EVERYTHING I DO DOESN'T WORK OUT"
            };

            int easyRange = 5;
            int mediumRange = 9;
            int hardRange = strings.Length;

            int range = strings.Length;
            switch (_difficulty)
            {
                case 0:
                    range = easyRange;
                    break;
                case 1:
                    range = mediumRange;
                    break;
                case 2:
                    range = hardRange;
                    break;
                default:
                    range = strings.Length;
                    break;
            }

            newEnemy.Init(strings[Random.Range(0, range)], RuneWords.Belief, enemyPath);

            _spawnInterval -= _spawnDecrement;
            _spawnInterval = Mathf.Clamp(_spawnInterval, 1.5f, Mathf.Infinity);

            _spawnTimer = _spawnInterval;
        }

        if (_gameTimer <= 0)
        {
            if (!_inBreak)
            {
                OnBreakStart.Invoke();
                _gameTimer = _breakTime;
                _inBreak = true;
            }
            else
            {
                _gameTimer = _difficulty0Time;
                _difficulty++;
                _inBreak = false;
                OnBreakEnd.Invoke();
            }
        }
    }
}
